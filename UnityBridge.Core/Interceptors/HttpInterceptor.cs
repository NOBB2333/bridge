namespace UnityBridge.Core.Interceptors;

/// <summary>
/// HTTP 拦截器基类，用于在请求发送前后执行自定义逻辑。
/// 参考 SKIT.FlurlHttpClient 的设计模式。
/// </summary>
public abstract class HttpInterceptor
{
    /// <summary>
    /// 请求发送前调用。
    /// </summary>
    /// <param name="context">拦截器上下文，包含请求信息。</param>
    /// <param name="cancellationToken">取消令牌。</param>
    public virtual Task BeforeCallAsync(HttpInterceptorContext context, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// 请求完成后调用（包括成功和失败）。
    /// </summary>
    /// <param name="context">拦截器上下文，包含请求和响应信息。</param>
    /// <param name="cancellationToken">取消令牌。</param>
    public virtual Task AfterCallAsync(HttpInterceptorContext context, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
