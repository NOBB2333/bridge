namespace UnityBridge.Core;

/// <summary>
/// 请求拦截器接口，用于在请求发送前后执行自定义逻辑。
/// </summary>
public interface IRequestInterceptor
{
    /// <summary>
    /// 请求发送前调用，可以修改请求。
    /// </summary>
    /// <param name="request">Flurl 请求对象。</param>
    /// <param name="cancellationToken">取消令牌。</param>
    /// <returns>修改后的请求对象。</returns>
    Task<IFlurlRequest> OnRequestAsync(IFlurlRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// 响应接收后调用，可以处理响应。
    /// </summary>
    /// <param name="request">请求对象。</param>
    /// <param name="response">响应对象。</param>
    /// <param name="cancellationToken">取消令牌。</param>
    /// <returns>处理后的响应对象。</returns>
    Task<IFlurlResponse> OnResponseAsync(IFlurlRequest request, IFlurlResponse response, CancellationToken cancellationToken = default);
}

