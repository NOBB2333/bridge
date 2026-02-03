namespace UnityBridge.Crawler.Kuaishou;

/// <summary>
/// 快手客户端配置项。
/// </summary>
public class KuaishouClientOptions : ClientOptions
{
    /// <summary>
    /// 获取或设置 GraphQL API 端点。
    /// </summary>
    public string Endpoint { get; set; } = KuaishouEndpoints.API;

    /// <summary>
    /// 初始化默认配置。
    /// </summary>
    public KuaishouClientOptions()
    {
        UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/125.0.0.0 Safari/537.36";
        Referer = "https://www.kuaishou.com/";
        Origin = "https://www.kuaishou.com";
    }

    /// <summary>
    /// 获取或设置 Origin。
    /// </summary>
    public string Origin { get; set; } = "https://www.kuaishou.com";
}
