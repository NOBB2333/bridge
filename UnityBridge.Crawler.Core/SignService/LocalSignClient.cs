namespace UnityBridge.Crawler.Core.SignService;

using Microsoft.Extensions.Caching.Memory;
using UnityBridge.Crawler.Core.SignService.Bilibili;
using UnityBridge.Crawler.Core.SignService.Douyin;
using UnityBridge.Crawler.Core.SignService.Xhs;
using UnityBridge.Crawler.Core.SignService.Zhihu;

/// <summary>
/// 本地签名客户端，实现 ISignClient 接口。
/// 直接在本地执行签名算法，无需调用外部服务。
/// </summary>
public class LocalSignClient : ISignClient, IDisposable
{
    private readonly BilibiliSignProvider _bilibiliProvider;
    private readonly XhsJsSignProvider _xhsProvider;
    private readonly DouyinJsSignProvider _douyinProvider;
    private readonly ZhihuJsSignProvider _zhihuProvider;
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _cache;
    private readonly bool _disposeResources;

    public LocalSignClient(HttpClient? httpClient = null, IMemoryCache? cache = null)
    {
        _httpClient = httpClient ?? new HttpClient();
        _cache = cache ?? new MemoryCache(new MemoryCacheOptions());
        _disposeResources = httpClient is null;

        _bilibiliProvider = new BilibiliSignProvider(_httpClient, _cache);
        _xhsProvider = new XhsJsSignProvider();
        _douyinProvider = new DouyinJsSignProvider();
        _zhihuProvider = new ZhihuJsSignProvider();
    }

    /// <inheritdoc/>
    public async Task<XhsSignResult> GetXhsSignAsync(XhsSignRequest request, CancellationToken ct = default)
    {
        await Task.CompletedTask; // 使方法异步

        var result = _xhsProvider.Sign(request.Uri, request.Data, request.Cookies);

        return new XhsSignResult
        {
            XS = result.XS,
            XT = result.XT,
            XSCommon = result.XSCommon,
            XB3TraceId = result.XB3TraceId
        };
    }

    /// <inheritdoc/>
    public async Task<DouyinSignResult> GetDouyinSignAsync(DouyinSignRequest request, CancellationToken ct = default)
    {
        await Task.CompletedTask;

        // 从 URI 提取 query params
        var queryParams = ExtractQueryParams(request.Uri);
        var result = _douyinProvider.Sign(queryParams);

        return new DouyinSignResult
        {
            ABogus = result.ABogus,
            MsToken = "" // msToken 由 cookie 提供，不是签名生成
        };
    }

    /// <inheritdoc/>
    public Task<KuaishouSignResult> GetKuaishouSignAsync(KuaishouSignRequest request, CancellationToken ct = default)
    {
        // 快手签名暂未实现
        return Task.FromResult(new KuaishouSignResult { Did = "" });
    }

    /// <inheritdoc/>
    public async Task<BilibiliSignResult> GetBilibiliSignAsync(BilibiliSignRequest request, CancellationToken ct = default)
    {
        var result = await _bilibiliProvider.SignAsync(request.ReqData, request.Cookies, ct);

        return new BilibiliSignResult
        {
            Wts = result.Wts,
            WRid = result.WRid
        };
    }

    /// <inheritdoc/>
    public async Task<ZhihuSignResult> GetZhihuSignAsync(ZhihuSignRequest request, CancellationToken ct = default)
    {
        await Task.CompletedTask;

        var result = _zhihuProvider.Sign(request.Uri, request.Cookies);

        return new ZhihuSignResult
        {
            XZse96 = result.XZse96,
            XZst81 = result.XZst81
        };
    }

    /// <inheritdoc/>
    public Task<bool> PingAsync(CancellationToken ct = default)
    {
        // 本地签名服务始终可用
        return Task.FromResult(true);
    }

    /// <summary>
    /// 从 URI 提取 query 参数字符串
    /// </summary>
    private static string ExtractQueryParams(string uri)
    {
        if (string.IsNullOrEmpty(uri)) return "";

        var queryIndex = uri.IndexOf('?');
        return queryIndex >= 0 ? uri[(queryIndex + 1)..] : uri;
    }

    public void Dispose()
    {
        _xhsProvider.Dispose();
        _douyinProvider.Dispose();
        _zhihuProvider.Dispose();

        if (_disposeResources)
        {
            _httpClient.Dispose();
            (_cache as IDisposable)?.Dispose();
        }

        GC.SuppressFinalize(this);
    }
}
