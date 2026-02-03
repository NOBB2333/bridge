namespace UnityBridge.Crawler.Weibo;

/// <summary>
/// 微博客户端配置项。
/// </summary>
public class WeiboClientOptions : ClientOptions
{
    /// <summary>
    /// 获取或设置 API 端点。
    /// </summary>
    public string Endpoint { get; set; } = WeiboEndpoints.API;

    /// <summary>
    /// 初始化默认配置。
    /// </summary>
    public WeiboClientOptions()
    {
        UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/125.0.0.0 Safari/537.36";
        Referer = "https://m.weibo.cn/";
        Origin = "https://m.weibo.cn/";
    }

    /// <summary>
    /// 获取或设置 Origin。
    /// </summary>
    public string Origin { get; set; } = "https://m.weibo.cn/";
}
