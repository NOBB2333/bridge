using System.Collections.Concurrent;
using System.Net;
using System.Text.Json;
using Polly;
using Polly.Retry;
using UnityBridge.Core.Interceptors;
using UnityBridge.Crawler.Core.AccountPool;

namespace UnityBridge.Crawler.Core;

/// <summary>
/// 爬虫客户端基类，提供代理池、Cookie管理、重试策略、并发控制等通用功能。
/// 平台特定的逻辑（如 headers、认证）由子类实现。
/// </summary>
public abstract class CrawlerClientBase : CommonClientBase
{
    private readonly ClientOptions _clientOptions;
    private readonly AccountPoolManager? _accountPool;
    private readonly ProxyPoolManager? _proxyPoolManager;
    private readonly CookieManager? _cookieManager;
    private readonly SemaphoreSlim _semaphore;
    private readonly ConcurrentDictionary<string, DateTime> _requestCache = new();
    private readonly AsyncRetryPolicy<IFlurlResponse> _retryPolicy;

    /// <summary>
    /// 获取客户端配置项。
    /// </summary>
    public ClientOptions ClientOptions => _clientOptions;


    /// <summary>
    /// 获取当前使用的账号信息。
    /// </summary>
    public AccountInfo? CurrentAccount { get; protected set; }

    /// <summary>
    /// 获取平台名称（由子类实现，用于 Cookie 隔离）。
    /// </summary>
    protected abstract string PlatformName { get; }

    /// <summary>
    /// 初始化爬虫客户端基类。
    /// </summary>
    /// <param name="options">客户端配置项。</param>
    /// <param name="httpClient">可选的 HttpClient 实例。如果为 null，将自动创建带代理支持的 HttpClient。</param>
    /// <param name="disposeClient">是否在释放时销毁 HttpClient。</param>
    /// <param name="jsonOptions">JSON 序列化选项。</param>
    protected CrawlerClientBase(
        ClientOptions options, 
        AccountPoolManager? accountPool = null,
        HttpClient? httpClient = null,
        bool disposeClient = true, 
        JsonSerializerOptions? jsonOptions = null)
        : base(CreateHttpClientWithProxy(options, httpClient), disposeClient, jsonOptions)
    {
        _clientOptions = options ?? throw new ArgumentNullException(nameof(options));
        _accountPool = accountPool;

        // 初始化代理池
        if (_clientOptions.EnableProxyPool && _clientOptions.ProxyPool.Count > 0)
        {
            _proxyPoolManager = new ProxyPoolManager(_clientOptions.ProxyPool, _clientOptions.ProxySelectionStrategy);
        }

        // 初始化 Cookie 管理器
        if (_clientOptions.EnableCookieManagement)
        {
            _cookieManager = new CookieManager();
        }

        // 初始化并发控制
        _semaphore = new SemaphoreSlim(_clientOptions.MaxConcurrency, _clientOptions.MaxConcurrency);

        // 配置重试策略
        _retryPolicy = Policy
            .HandleResult<IFlurlResponse>(r => !r.ResponseMessage.IsSuccessStatusCode)
            .Or<HttpRequestException>()
            .Or<TaskCanceledException>()
            .WaitAndRetryAsync(
                _clientOptions.MaxRetries,
                retryAttempt => TimeSpan.FromMilliseconds(_clientOptions.RetryDelayMs * Math.Pow(2, retryAttempt - 1)),
                onRetry: (outcome, timespan, retryCount, context) =>
                {
                    // 可以在这里记录重试日志
                });
    }

    /// <summary>
    /// 创建带代理支持的 HttpClient。
    /// </summary>
    private static HttpClient? CreateHttpClientWithProxy(ClientOptions options, HttpClient? existingClient)
    {
        if (existingClient != null)
            return existingClient;

        if (options.EnableProxyPool && options.ProxyPool.Count > 0)
        {
            var handler = new HttpClientHandler
            {
                UseProxy = true,
                Proxy = new WebProxy(options.ProxyPool[0]) // 初始代理，实际使用时通过代理池动态选择
            };
            return new HttpClient(handler);
        }

        return null; // 使用默认的 HttpClient
    }

    /// <summary>
    /// 添加拦截器。
    /// </summary>
    public void AddInterceptor(HttpInterceptor interceptor)
    {
        if (interceptor != null)
        {
            Interceptors.Add(interceptor);
        }
    }

    /// <summary>
    /// 移除拦截器。
    /// </summary>
    public void RemoveInterceptor(HttpInterceptor interceptor)
    {
        Interceptors.Remove(interceptor);
    }

    /// <summary>
    /// 创建 Flurl 请求（重写基类方法，添加通用处理逻辑）。
    /// </summary>
    public override IFlurlRequest CreateFlurlRequest(object request, HttpMethod httpMethod, params object[] urlSegments)
    {
        var flurlRequest = base.CreateFlurlRequest(request, httpMethod, urlSegments);

        // 注意：代理设置需要在创建 HttpClient 时通过 HttpClientHandler 配置
        // 这里可以通过拦截器或子类重写来动态切换代理

        // 应用 Cookie（如果需要）
        if (_cookieManager != null && flurlRequest.Url != null)
        {
            var uri = new Uri(flurlRequest.Url);
            var domain = uri.Host;
            var cookieHeader = _cookieManager.GetCookieHeader(domain, uri);
            if (!string.IsNullOrEmpty(cookieHeader))
            {
                flurlRequest.WithHeader("Cookie", cookieHeader);
            }
        }

        // 调用子类实现，添加平台特定的逻辑（如 headers、认证等）
        flurlRequest = OnCreateRequest(flurlRequest, request, httpMethod, urlSegments);

        return flurlRequest;
    }

    /// <summary>
    /// 子类重写此方法以添加平台特定的请求逻辑（如 headers、认证等）。
    /// </summary>
    /// <param name="flurlRequest">Flurl 请求对象。</param>
    /// <param name="request">请求对象。</param>
    /// <param name="httpMethod">HTTP 方法。</param>
    /// <param name="urlSegments">URL 片段。</param>
    /// <returns>修改后的请求对象。</returns>
    protected virtual IFlurlRequest OnCreateRequest(IFlurlRequest flurlRequest, object request, HttpMethod httpMethod, params object[] urlSegments)
    {
        // 1. 注入通用 Headers
        if (!string.IsNullOrEmpty(ClientOptions.UserAgent))
        {
            flurlRequest.WithHeader("User-Agent", ClientOptions.UserAgent);
        }
        if (!string.IsNullOrEmpty(ClientOptions.Referer))
        {
            flurlRequest.WithHeader("Referer", ClientOptions.Referer);
        }

        // 2. 注入 Cookie (优先级：CurrentAccount > ClientOptions > CookieManager)
        string cookieValue = string.Empty;
        if (CurrentAccount != null && !string.IsNullOrEmpty(CurrentAccount.Cookies))
        {
            cookieValue = CurrentAccount.Cookies;
        }
        else if (!string.IsNullOrEmpty(ClientOptions.Cookies))
        {
            cookieValue = ClientOptions.Cookies;
        }
        else if (_cookieManager != null)
        {
            cookieValue = GetCookies(PlatformName);
        }

        if (!string.IsNullOrEmpty(cookieValue))
        {
            flurlRequest.WithHeader("Cookie", cookieValue);
        }

        return flurlRequest;
    }

    /// <summary>
    /// 切换到下一个可用账号。
    /// </summary>
    public virtual async Task<bool> SwitchToNextAccountAsync(CancellationToken ct = default)
    {
        if (_proxyPoolManager == null && _accountPool == null)
        {
             return false;
        }
        
        // TODO: 集成真正的 AccountPoolManager。
        // 目前这是一个简化实现，假设 AccountPoolManager 会被注入或在子类处理
        // 为了保持兼容性，我们允许子类 override 此方法
        
        return await Task.FromResult(true);
    }

    /// <summary>
    /// 发送请求（带重试、并发控制、拦截器）。
    /// </summary>
    public override async Task<IFlurlResponse> SendFlurlRequestAsync(IFlurlRequest flurlRequest, HttpContent? httpContent = null, CancellationToken cancellationToken = default)
    {
        if (flurlRequest == null)
            throw new ArgumentNullException(nameof(flurlRequest));

        // 请求去重检查
        if (_clientOptions.EnableRequestDeduplication)
        {
            var requestKey = $"{flurlRequest.Verb}_{flurlRequest.Url}";
            if (_requestCache.TryGetValue(requestKey, out var cachedTime))
            {
                if (DateTime.UtcNow - cachedTime < TimeSpan.FromSeconds(_clientOptions.DeduplicationCacheExpirySeconds))
                {
                    throw new InvalidOperationException($"Duplicate request detected: {requestKey}");
                }
            }
            _requestCache[requestKey] = DateTime.UtcNow;
        }

        // 创建拦截器上下文
        var context = new HttpInterceptorContext
        {
            FlurlRequest = flurlRequest
        };
        // 尝试读取请求体（为了日志等）
        if (httpContent != null) 
        {
             try { context.RequestBody = await httpContent.ReadAsStringAsync(cancellationToken).ConfigureAwait(false); } catch { }
        }

        // 并发控制
        await _semaphore.WaitAsync(cancellationToken);
        try
        {
            // 执行请求前拦截器
            context.Stopwatch.Start();
            foreach (var interceptor in Interceptors)
            {
                await interceptor.BeforeCallAsync(context, cancellationToken).ConfigureAwait(false);
            }

            // 执行请求（带重试）
            IFlurlResponse response;
            try
            {
                 response = await _retryPolicy.ExecuteAsync(async () =>
                {
                    if (httpContent != null)
                    {
                        return await flurlRequest.SendAsync(flurlRequest.Verb, httpContent, cancellationToken: cancellationToken);
                    }
                    return await flurlRequest.SendAsync(flurlRequest.Verb, null, cancellationToken: cancellationToken);
                });
                context.FlurlResponse = response;
            }
            catch (Exception ex)
            {
                context.Exception = ex;
                context.Stopwatch.Stop();
                // 异常后拦截器
                foreach (var interceptor in Interceptors)
                {
                    await interceptor.AfterCallAsync(context, cancellationToken).ConfigureAwait(false);
                }
                throw;
            }
            
            context.Stopwatch.Stop();

            // 处理 Cookie（从响应中提取）
            if (_cookieManager != null && flurlRequest.Url != null)
            {
                var uri = new Uri(flurlRequest.Url);
                var domain = uri.Host;
                if (response.ResponseMessage != null)
                {
                    _cookieManager.ExtractCookiesFromResponse(domain, response.ResponseMessage);
                }
            }

            // 执行响应后拦截器
            foreach (var interceptor in Interceptors)
            {
                await interceptor.AfterCallAsync(context, cancellationToken).ConfigureAwait(false);
            }

            return response;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    /// <summary>
    /// 获取指定域名的 Cookie。
    /// </summary>
    public string GetCookies(string domain)
    {
        if (_cookieManager == null)
            return string.Empty;

        var uri = new Uri($"http://{domain}");
        return _cookieManager.GetCookieHeader(domain, uri);
    }

    /// <summary>
    /// 清除指定域名的 Cookie。
    /// </summary>
    public void ClearCookies(string domain)
    {
        _cookieManager?.ClearCookies(domain);
    }

    /// <summary>
    /// 清除所有 Cookie。
    /// </summary>
    public void ClearAllCookies()
    {
        _cookieManager?.ClearAllCookies();
    }

    /// <summary>
    /// 释放资源。
    /// </summary>
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _semaphore?.Dispose();
        }
        base.Dispose(disposing);
    }
}
