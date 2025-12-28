namespace UnityBridge.Core.Interceptors;

/// <summary>
/// 日志拦截器，记录请求和响应的详细信息。
/// </summary>
public class LoggingInterceptor : HttpInterceptor
{
    private readonly Action<string>? _logger;
    private readonly LoggingInterceptorOptions _options;

    /// <summary>
    /// 创建日志拦截器。
    /// </summary>
    /// <param name="logger">日志输出函数，默认使用 Console.WriteLine。</param>
    /// <param name="options">日志选项。</param>
    public LoggingInterceptor(Action<string>? logger = null, LoggingInterceptorOptions? options = null)
    {
        _logger = logger ?? Console.WriteLine;
        _options = options ?? new LoggingInterceptorOptions();
    }

    /// <inheritdoc/>
    public override Task BeforeCallAsync(HttpInterceptorContext context, CancellationToken cancellationToken = default)
    {
        var traceId = context.TraceId.ToString("N")[..8];
        var method = context.HttpMethod?.Method ?? "?";
        var url = context.RequestUrl ?? "?";

        Log($"[{traceId}] ▶ REQUEST: {method} {url}");

        if (_options.LogRequestHeaders && context.FlurlRequest is not null)
        {
            foreach (var header in context.FlurlRequest.Headers)
            {
                var value = _options.SensitiveHeaders.Contains(header.Name, StringComparer.OrdinalIgnoreCase)
                    ? "***"
                    : header.Value;
                Log($"[{traceId}]   Header: {header.Name}: {value}");
            }
        }

        if (_options.LogRequestBody && !string.IsNullOrEmpty(context.RequestBody))
        {
            var body = TruncateIfNeeded(context.RequestBody);
            Log($"[{traceId}]   Body: {body}");
        }

        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public override Task AfterCallAsync(HttpInterceptorContext context, CancellationToken cancellationToken = default)
    {
        var traceId = context.TraceId.ToString("N")[..8];
        var elapsed = context.Elapsed.TotalMilliseconds;

        if (context.Exception is not null)
        {
            Log($"[{traceId}] ✗ ERROR: {context.Exception.GetType().Name} - {context.Exception.Message} ({elapsed:F0}ms)");
            return Task.CompletedTask;
        }

        var status = context.StatusCode ?? 0;
        var statusText = context.IsSuccess ? "OK" : "FAILED";
        Log($"[{traceId}] ◀ RESPONSE: {status} {statusText} ({elapsed:F0}ms)");

        if (_options.LogResponseBody && !string.IsNullOrEmpty(context.ResponseBody))
        {
            var body = TruncateIfNeeded(context.ResponseBody);
            Log($"[{traceId}]   Body: {body}");
        }

        return Task.CompletedTask;
    }

    private void Log(string message)
    {
        try
        {
            _logger?.Invoke(message);
        }
        catch
        {
            // 忽略日志错误
        }
    }

    private string TruncateIfNeeded(string text)
    {
        if (_options.MaxBodyLength > 0 && text.Length > _options.MaxBodyLength)
        {
            return text[.._options.MaxBodyLength] + "... (truncated)";
        }
        return text;
    }
}

/// <summary>
/// 日志拦截器选项。
/// </summary>
public class LoggingInterceptorOptions
{
    /// <summary>
    /// 是否记录请求头，默认 false。
    /// </summary>
    public bool LogRequestHeaders { get; set; } = false;

    /// <summary>
    /// 是否记录请求体，默认 true。
    /// </summary>
    public bool LogRequestBody { get; set; } = true;

    /// <summary>
    /// 是否记录响应体，默认 false（可能很大）。
    /// </summary>
    public bool LogResponseBody { get; set; } = false;

    /// <summary>
    /// 请求/响应体最大长度，超过则截断。0 表示不限制，默认 2000。
    /// </summary>
    public int MaxBodyLength { get; set; } = 2000;

    /// <summary>
    /// 敏感头部名称列表，这些头部的值会被隐藏。
    /// </summary>
    public HashSet<string> SensitiveHeaders { get; set; } = new(StringComparer.OrdinalIgnoreCase)
    {
        "Authorization",
        "Token",
        "Cookie",
        "Set-Cookie",
        "X-Api-Key",
        "OVERTOKEN"
    };
}
