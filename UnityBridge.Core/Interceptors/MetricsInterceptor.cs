using System.Collections.Concurrent;

namespace UnityBridge.Core.Interceptors;

/// <summary>
/// 指标收集拦截器，统计请求的成功率、耗时等指标。
/// </summary>
public class MetricsInterceptor : HttpInterceptor
{
    private readonly ConcurrentDictionary<string, RequestMetrics> _metricsByEndpoint = new();
    private readonly RequestMetrics _globalMetrics = new("global");

    /// <summary>
    /// 获取全局请求指标。
    /// </summary>
    public RequestMetrics GlobalMetrics => _globalMetrics;

    /// <summary>
    /// 获取按端点分组的指标。
    /// </summary>
    public IReadOnlyDictionary<string, RequestMetrics> MetricsByEndpoint => _metricsByEndpoint;

    /// <inheritdoc/>
    public override Task AfterCallAsync(HttpInterceptorContext context, CancellationToken cancellationToken = default)
    {
        var elapsed = context.Elapsed;
        var isSuccess = context.IsSuccess && context.Exception is null;

        // 更新全局指标
        _globalMetrics.Record(elapsed, isSuccess);

        // 更新端点指标
        var endpoint = ExtractEndpoint(context.RequestUrl);
        if (!string.IsNullOrEmpty(endpoint))
        {
            var metrics = _metricsByEndpoint.GetOrAdd(endpoint, key => new RequestMetrics(key));
            metrics.Record(elapsed, isSuccess);
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// 重置所有指标。
    /// </summary>
    public void Reset()
    {
        _globalMetrics.Reset();
        _metricsByEndpoint.Clear();
    }

    /// <summary>
    /// 获取指标摘要报告。
    /// </summary>
    public string GetSummary()
    {
        var sb = new System.Text.StringBuilder();
        sb.AppendLine("=== Request Metrics Summary ===");
        sb.AppendLine();
        sb.AppendLine($"Global: {_globalMetrics}");
        sb.AppendLine();

        if (_metricsByEndpoint.Count > 0)
        {
            sb.AppendLine("By Endpoint:");
            foreach (var kvp in _metricsByEndpoint.OrderByDescending(x => x.Value.TotalRequests))
            {
                sb.AppendLine($"  {kvp.Key}: {kvp.Value}");
            }
        }

        return sb.ToString();
    }

    private static string ExtractEndpoint(string? url)
    {
        if (string.IsNullOrEmpty(url))
            return string.Empty;

        try
        {
            var uri = new Uri(url);
            // 取路径的前两级作为端点，如 /api/v1/xxx -> /api/v1
            var segments = uri.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);
            return segments.Length >= 2
                ? $"/{segments[0]}/{segments[1]}"
                : uri.AbsolutePath;
        }
        catch
        {
            return string.Empty;
        }
    }
}

/// <summary>
/// 请求指标数据。
/// </summary>
public class RequestMetrics
{
    private long _totalRequests;
    private long _successRequests;
    private long _failedRequests;
    private long _totalDurationMs;
    private long _minDurationMs = long.MaxValue;
    private long _maxDurationMs;
    private readonly object _lock = new();

    /// <summary>
    /// 端点名称。
    /// </summary>
    public string Endpoint { get; }

    /// <summary>
    /// 总请求数。
    /// </summary>
    public long TotalRequests => Interlocked.Read(ref _totalRequests);

    /// <summary>
    /// 成功请求数。
    /// </summary>
    public long SuccessRequests => Interlocked.Read(ref _successRequests);

    /// <summary>
    /// 失败请求数。
    /// </summary>
    public long FailedRequests => Interlocked.Read(ref _failedRequests);

    /// <summary>
    /// 成功率（0-100）。
    /// </summary>
    public double SuccessRate
    {
        get
        {
            var total = TotalRequests;
            return total > 0 ? (double)SuccessRequests / total * 100 : 0;
        }
    }

    /// <summary>
    /// 平均耗时（毫秒）。
    /// </summary>
    public double AverageDurationMs
    {
        get
        {
            var total = TotalRequests;
            return total > 0 ? (double)Interlocked.Read(ref _totalDurationMs) / total : 0;
        }
    }

    /// <summary>
    /// 最小耗时（毫秒）。
    /// </summary>
    public long MinDurationMs
    {
        get
        {
            var min = Interlocked.Read(ref _minDurationMs);
            return min == long.MaxValue ? 0 : min;
        }
    }

    /// <summary>
    /// 最大耗时（毫秒）。
    /// </summary>
    public long MaxDurationMs => Interlocked.Read(ref _maxDurationMs);

    public RequestMetrics(string endpoint)
    {
        Endpoint = endpoint;
    }

    /// <summary>
    /// 记录一次请求。
    /// </summary>
    public void Record(TimeSpan duration, bool success)
    {
        var durationMs = (long)duration.TotalMilliseconds;

        Interlocked.Increment(ref _totalRequests);
        Interlocked.Add(ref _totalDurationMs, durationMs);

        if (success)
            Interlocked.Increment(ref _successRequests);
        else
            Interlocked.Increment(ref _failedRequests);

        // 更新最小/最大值（需要 lock 确保原子性）
        lock (_lock)
        {
            if (durationMs < _minDurationMs)
                Interlocked.Exchange(ref _minDurationMs, durationMs);
            if (durationMs > _maxDurationMs)
                Interlocked.Exchange(ref _maxDurationMs, durationMs);
        }
    }

    /// <summary>
    /// 重置指标。
    /// </summary>
    public void Reset()
    {
        Interlocked.Exchange(ref _totalRequests, 0);
        Interlocked.Exchange(ref _successRequests, 0);
        Interlocked.Exchange(ref _failedRequests, 0);
        Interlocked.Exchange(ref _totalDurationMs, 0);
        Interlocked.Exchange(ref _minDurationMs, long.MaxValue);
        Interlocked.Exchange(ref _maxDurationMs, 0);
    }

    public override string ToString()
    {
        return $"Requests: {TotalRequests} (✓{SuccessRequests} ✗{FailedRequests}) | " +
               $"Success: {SuccessRate:F1}% | " +
               $"Latency: avg={AverageDurationMs:F0}ms, min={MinDurationMs}ms, max={MaxDurationMs}ms";
    }
}
