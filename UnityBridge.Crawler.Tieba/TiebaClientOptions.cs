namespace UnityBridge.Crawler.Tieba;

/// <summary>
/// 贴吧客户端配置项。
/// </summary>
public class TiebaClientOptions : ClientOptions
{
    /// <summary>
    /// 获取或设置网页端点。
    /// </summary>
    public string Endpoint { get; set; } = TiebaEndpoints.WEB;

    /// <summary>
    /// 初始化默认配置。
    /// </summary>
    public TiebaClientOptions()
    {
        UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/125.0.0.0 Safari/537.36";
        Referer = "https://tieba.baidu.com";
        Origin = "https://tieba.baidu.com";
    }

    /// <summary>
    /// 获取或设置 Origin。
    /// </summary>
    public string Origin { get; set; } = "https://tieba.baidu.com";
}
