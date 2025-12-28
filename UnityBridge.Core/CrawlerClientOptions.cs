namespace UnityBridge.Core;

/// <summary>
/// 爬虫客户端配置项，提供代理池、重试、并发等通用配置。
/// </summary>
public class CrawlerClientOptions : CommonClientOptions
{
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
    public int DeduplicationCacheExpirySeconds { get; set; } = 300;
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

