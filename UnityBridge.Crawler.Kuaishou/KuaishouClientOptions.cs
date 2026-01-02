namespace UnityBridge.Crawler.Kuaishou;

/// <summary>
/// 快手客户端配置项。
/// </summary>
public class KuaishouClientOptions : CrawlerClientOptions
{
    /// <summary>
    /// 获取或设置 GraphQL API 端点。
    /// </summary>
    public string Endpoint { get; set; } = KuaishouEndpoints.API;

    /// <summary>
    /// 获取或设置默认 Cookies。
    /// </summary>
    public string? Cookies { get; set; }

    /// <summary>
    /// 获取或设置 User-Agent。
    /// </summary>
    public string UserAgent { get; set; } = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/125.0.0.0 Safari/537.36";

    /// <summary>
    /// 获取或设置 Origin。
    /// </summary>
    public string Origin { get; set; } = "https://www.kuaishou.com";

    /// <summary>
    /// 获取或设置 Referer。
    /// </summary>
    public string Referer { get; set; } = "https://www.kuaishou.com/";
}
