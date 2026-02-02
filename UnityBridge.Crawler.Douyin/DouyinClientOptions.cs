namespace UnityBridge.Crawler.Douyin;

/// <summary>
/// 抖音客户端配置项。
/// </summary>
public class DouyinClientOptions : ClientOptions
{
    /// <summary>
    /// 获取或设置 API 端点。
    /// </summary>
    public string Endpoint { get; set; } = DouyinEndpoints.API;

    /// <summary>
    /// 获取或设置默认 Cookies。
    /// </summary>
    public string? Cookies { get; set; }

    /// <summary>
    /// 获取或设置签名服务地址。
    /// </summary>
    public string SignServerUrl { get; set; } = "http://localhost:8888";

    /// <summary>
    /// 获取或设置 User-Agent（抖音强制要求固定 UA）。
    /// </summary>
    public string UserAgent { get; set; } = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/135.0.0.0 Safari/537.36";

    /// <summary>
    /// 获取或设置 Origin。
    /// </summary>
    public string Origin { get; set; } = "https://www.douyin.com";

    /// <summary>
    /// 获取或设置 Referer。
    /// </summary>
    public string Referer { get; set; } = "https://www.douyin.com/user/self";

    /// <summary>
    /// 获取或设置 webid（验证参数）。
    /// </summary>
    public string? WebId { get; set; }

    /// <summary>
    /// 获取或设置 msToken（验证参数）。
    /// </summary>
    public string? MsToken { get; set; }

    /// <summary>
    /// 获取或设置 verifyFp（验证参数）。
    /// </summary>
    public string? VerifyFp { get; set; }
}
