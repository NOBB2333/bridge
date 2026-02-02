using System.Diagnostics;

namespace UnityBridge.Core.Interceptors;

/// <summary>
/// HTTP 拦截器上下文，携带请求/响应的完整信息。
/// 在整个拦截器管道中共享，可用于传递数据和记录状态。
/// </summary>
public class HttpInterceptorContext
{
    /// <summary>
    /// 请求追踪ID，用于关联同一请求的日志。
    /// </summary>
    public Guid TraceId { get; }

    /// <summary>
    /// 请求计时器。
    /// </summary>
    public Stopwatch Stopwatch { get; }

    /// <summary>
    /// Flurl 请求调用信息（在 BeforeCall 时可能为 null）。
    /// </summary>
    public FlurlCall? FlurlCall { get; set; }

    /// <summary>
    /// Flurl 请求对象。
    /// </summary>
    public IFlurlRequest? FlurlRequest { get; set; }

    /// <summary>
    /// Flurl 响应对象（在 AfterCall 时设置）。
    /// </summary>
    public IFlurlResponse? FlurlResponse { get; set; }

    /// <summary>
    /// 请求体内容（用于日志记录）。
    /// </summary>
    public string? RequestBody { get; set; }

    /// <summary>
    /// 响应体内容（用于日志记录）。
    /// </summary>
    public string? ResponseBody { get; set; }

    /// <summary>
    /// 请求过程中发生的异常（如果有）。
    /// </summary>
    public Exception? Exception { get; set; }

    /// <summary>
    /// 额外的属性，用于在拦截器之间传递数据。
    /// </summary>
    public IDictionary<string, object> Properties { get; }

    /// <summary>
    /// 请求是否已完成。
    /// </summary>
    public bool Completed => FlurlResponse is not null || Exception is not null;

    /// <summary>
    /// 请求是否成功（HTTP 状态码 2xx）。
    /// </summary>
    public bool IsSuccess => FlurlResponse?.StatusCode is >= 200 and < 300;

    /// <summary>
    /// 请求耗时。
    /// </summary>
    public TimeSpan Elapsed => Stopwatch.Elapsed;

    /// <summary>
    /// HTTP 状态码（如果有响应）。
    /// </summary>
    public int? StatusCode => FlurlResponse?.StatusCode;

    /// <summary>
    /// 请求 URL。
    /// </summary>
    public string? RequestUrl => FlurlRequest?.Url?.ToString();

    /// <summary>
    /// 请求方法。
    /// </summary>
    public HttpMethod? HttpMethod => FlurlRequest?.Verb;

    /// <summary>
    /// 创建新的拦截器上下文。
    /// </summary>
    public HttpInterceptorContext()
    {
        TraceId = Guid.NewGuid();
        Stopwatch = new Stopwatch();
        Properties = new Dictionary<string, object>();
    }

    /// <summary>
    /// 使用指定的追踪ID创建拦截器上下文。
    /// </summary>
    /// <param name="traceId">追踪ID。</param>
    public HttpInterceptorContext(Guid traceId)
    {
        TraceId = traceId;
        Stopwatch = new Stopwatch();
        Properties = new Dictionary<string, object>();
    }
}
