namespace UnityBridge.Core.Clients;

/// <summary>
/// UnityBridge 统一客户端配置项（ClientOptions）。
/// 提供超时、代理池、重试、并发等通用配置。
/// </summary>
public class ClientOptions
{
    /// <summary>
    /// 获取或设置请求超时时间（单位：毫秒）。
    /// </summary>
    public int Timeout { get; set; } = 30 * 1000;

    /// <summary>
    /// 获取或设置代理池列表（格式：http://host:port 或 socks5://host:port）。
    /// </summary>
    public List<string> ProxyPool { get; set; } = new();

    /// <summary>
    /// 获取或设置是否启用代理池。
    /// </summary>
    public bool EnableProxyPool { get; set; } = false;

    /// <summary>
    /// 获取或设置代理选择策略（RoundRobin 或 Random）。
    /// </summary>
    public ProxySelectionStrategy ProxySelectionStrategy { get; set; } = ProxySelectionStrategy.RoundRobin;

    /// <summary>
    /// 获取或设置是否启用 Cookie 自动管理。
    /// </summary>
    public bool EnableCookieManagement { get; set; } = true;

    /// <summary>
    /// 获取或设置最大重试次数。
    /// </summary>
    public int MaxRetries { get; set; } = 3;

    /// <summary>
    /// 获取或设置重试延迟（毫秒）。
    /// </summary>
    public int RetryDelayMs { get; set; } = 1000;

    /// <summary>
    /// 获取或设置最大并发请求数。
    /// </summary>
    public int MaxConcurrency { get; set; } = 10;

    /// <summary>
    /// 获取或设置是否启用请求去重（基于 URL）。
    /// </summary>
    public bool EnableRequestDeduplication { get; set; } = false;

    /// <summary>
    /// 获取或设置请求去重缓存过期时间（秒）。
    /// </summary>
    /// <summary>
    /// 获取或设置请求去重缓存过期时间（秒）。
    /// </summary>
    public int DeduplicationCacheExpirySeconds { get; set; } = 300;

    /// <summary>
    /// 获取或设置默认 User-Agent。
    /// </summary>
    public string UserAgent { get; set; } = string.Empty;

    /// <summary>
    /// 获取或设置默认 Referer。
    /// </summary>
    public string Referer { get; set; } = string.Empty;

    /// <summary>
    /// 获取或设置默认 Cookies 字符串。
    /// </summary>
    public string Cookies { get; set; } = string.Empty;
}

/// <summary>
/// 代理选择策略。
/// </summary>
public enum ProxySelectionStrategy
{
    /// <summary>
    /// 轮询选择。
    /// </summary>
    RoundRobin,

    /// <summary>
    /// 随机选择。
    /// </summary>
    Random
}
