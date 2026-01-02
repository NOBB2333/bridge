using UnityBridge.Crawler.XiaoHongShu;
using UnityBridge.Crawler.BiliBili;
using UnityBridge.Crawler.Douyin;
using UnityBridge.Crawler.Tieba;
using UnityBridge.Crawler.Kuaishou;
using UnityBridge.Crawler.Zhihu;
using UnityBridge.Crawler.Weibo;
using UnityBridge.Crawler.Core.SignService;
using UnityBridge.Crawler.Core.AccountPool;

namespace UnityBridge.Crawler;

/// <summary>
/// 爬虫客户端工厂。
/// </summary>
public static class CrawlerFactory
{
    // 共享的本地签名客户端 (线程安全)
    private static readonly Lazy<LocalSignClient> SharedLocalSignClient = new(() => new LocalSignClient());

    /// <summary>
    /// 获取共享的本地签名客户端。
    /// </summary>
    public static ISignClient LocalSignClient => SharedLocalSignClient.Value;

    /// <summary>
    /// 获取签名客户端。如果提供了 signServerUrl 则使用 HTTP 客户端，否则使用本地签名。
    /// </summary>
    private static ISignClient GetSignClient(string? signServerUrl) =>
        string.IsNullOrEmpty(signServerUrl) ? LocalSignClient : new SignServerClient(signServerUrl);

    #region 小红书 (XiaoHongShu)

    /// <summary>
    /// 创建小红书客户端。
    /// </summary>
    /// <param name="cookies">Cookies</param>
    /// <param name="signServerUrl">签名服务地址，null 则使用本地签名（默认）</param>
    /// <param name="accountPool">账号池</param>
    public static XhsClient CreateXhsClient(
        string cookies,
        string? signServerUrl = null,
        AccountPoolManager? accountPool = null)
    {
        var options = new XhsClientOptions { Cookies = cookies };
        return new XhsClient(options, GetSignClient(signServerUrl), accountPool);
    }

    /// <summary>
    /// 创建带账号池的小红书客户端。
    /// </summary>
    public static async Task<XhsClient> CreateXhsClientWithPoolAsync(
        AccountPoolManager accountPool,
        string? signServerUrl = null,
        CancellationToken ct = default)
    {
        var options = new XhsClientOptions();
        var client = new XhsClient(options, GetSignClient(signServerUrl), accountPool);

        await accountPool.LoadAccountsAsync("xhs", ct);
        await client.SwitchToNextAccountAsync(ct);

        return client;
    }

    #endregion

    #region B站 (BiliBili)

    /// <summary>
    /// 创建B站客户端。
    /// </summary>
    /// <param name="cookies">Cookies</param>
    /// <param name="signServerUrl">签名服务地址，null 则使用本地签名（默认）</param>
    /// <param name="accountPool">账号池</param>
    public static BiliClient CreateBiliClient(
        string cookies,
        string? signServerUrl = null,
        AccountPoolManager? accountPool = null)
    {
        var options = new BiliClientOptions { Cookies = cookies };
        return new BiliClient(options, GetSignClient(signServerUrl), accountPool);
    }

    /// <summary>
    /// 创建带账号池的B站客户端。
    /// </summary>
    public static async Task<BiliClient> CreateBiliClientWithPoolAsync(
        AccountPoolManager accountPool,
        string? signServerUrl = null,
        CancellationToken ct = default)
    {
        var options = new BiliClientOptions();
        var client = new BiliClient(options, GetSignClient(signServerUrl), accountPool);

        await accountPool.LoadAccountsAsync("bilibili", ct);
        await client.SwitchToNextAccountAsync(ct);

        return client;
    }

    #endregion

    #region 抖音 (Douyin)

    /// <summary>
    /// 创建抖音客户端。
    /// </summary>
    /// <param name="cookies">Cookies</param>
    /// <param name="signServerUrl">签名服务地址，null 则使用本地签名（默认）</param>
    /// <param name="accountPool">账号池</param>
    public static DouyinClient CreateDouyinClient(
        string cookies,
        string? signServerUrl = null,
        AccountPoolManager? accountPool = null)
    {
        var options = new DouyinClientOptions { Cookies = cookies };
        return new DouyinClient(options, GetSignClient(signServerUrl), accountPool);
    }

    /// <summary>
    /// 创建带账号池的抖音客户端。
    /// </summary>
    public static async Task<DouyinClient> CreateDouyinClientWithPoolAsync(
        AccountPoolManager accountPool,
        string? signServerUrl = null,
        CancellationToken ct = default)
    {
        var options = new DouyinClientOptions();
        var client = new DouyinClient(options, GetSignClient(signServerUrl), accountPool);

        await accountPool.LoadAccountsAsync("douyin", ct);
        await client.SwitchToNextAccountAsync(ct);

        return client;
    }

    #endregion

    #region 百度贴吧 (Tieba)

    /// <summary>
    /// 创建贴吧客户端。
    /// </summary>
    public static TiebaClient CreateTiebaClient(
        string cookies,
        AccountPoolManager? accountPool = null)
    {
        var options = new TiebaClientOptions { Cookies = cookies };
        return new TiebaClient(options, accountPool);
    }

    /// <summary>
    /// 创建带账号池的贴吧客户端。
    /// </summary>
    public static async Task<TiebaClient> CreateTiebaClientWithPoolAsync(
        AccountPoolManager accountPool,
        CancellationToken ct = default)
    {
        var options = new TiebaClientOptions();
        var client = new TiebaClient(options, accountPool);

        await accountPool.LoadAccountsAsync("tieba", ct);
        await client.SwitchToNextAccountAsync(ct);

        return client;
    }

    #endregion

    #region 快手 (Kuaishou)

    /// <summary>
    /// 创建快手客户端。
    /// </summary>
    public static KuaishouClient CreateKuaishouClient(
        string cookies,
        AccountPoolManager? accountPool = null)
    {
        var options = new KuaishouClientOptions { Cookies = cookies };
        return new KuaishouClient(options, accountPool);
    }

    /// <summary>
    /// 创建带账号池的快手客户端。
    /// </summary>
    public static async Task<KuaishouClient> CreateKuaishouClientWithPoolAsync(
        AccountPoolManager accountPool,
        CancellationToken ct = default)
    {
        var options = new KuaishouClientOptions();
        var client = new KuaishouClient(options, accountPool);

        await accountPool.LoadAccountsAsync("kuaishou", ct);
        await client.SwitchToNextAccountAsync(ct);

        return client;
    }

    #endregion

    #region 知乎 (Zhihu)

    /// <summary>
    /// 创建知乎客户端。
    /// </summary>
    /// <param name="cookies">Cookies</param>
    /// <param name="signServerUrl">签名服务地址，null 则使用本地签名（默认）</param>
    /// <param name="accountPool">账号池</param>
    public static ZhihuClient CreateZhihuClient(
        string cookies,
        string? signServerUrl = null,
        AccountPoolManager? accountPool = null)
    {
        var options = new ZhihuClientOptions { Cookies = cookies };
        return new ZhihuClient(options, GetSignClient(signServerUrl), accountPool);
    }

    /// <summary>
    /// 创建带账号池的知乎客户端。
    /// </summary>
    public static async Task<ZhihuClient> CreateZhihuClientWithPoolAsync(
        AccountPoolManager accountPool,
        string? signServerUrl = null,
        CancellationToken ct = default)
    {
        var options = new ZhihuClientOptions();
        var client = new ZhihuClient(options, GetSignClient(signServerUrl), accountPool);

        await accountPool.LoadAccountsAsync("zhihu", ct);
        await client.SwitchToNextAccountAsync(ct);

        return client;
    }

    #endregion

    #region 微博 (Weibo)

    /// <summary>
    /// 创建微博客户端。
    /// </summary>
    public static WeiboClient CreateWeiboClient(
        string cookies,
        AccountPoolManager? accountPool = null)
    {
        var options = new WeiboClientOptions { Cookies = cookies };
        return new WeiboClient(options, accountPool);
    }

    /// <summary>
    /// 创建带账号池的微博客户端。
    /// </summary>
    public static async Task<WeiboClient> CreateWeiboClientWithPoolAsync(
        AccountPoolManager accountPool,
        CancellationToken ct = default)
    {
        var options = new WeiboClientOptions();
        var client = new WeiboClient(options, accountPool);

        await accountPool.LoadAccountsAsync("weibo", ct);
        await client.SwitchToNextAccountAsync(ct);

        return client;
    }

    #endregion
}
