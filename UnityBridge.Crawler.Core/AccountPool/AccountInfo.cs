namespace UnityBridge.Crawler.Core.AccountPool;

/// <summary>
/// 账号信息模型。
/// </summary>
public class AccountInfo
{
    /// <summary>
    /// 获取或设置账号名称/标识。
    /// </summary>
    public string AccountName { get; set; } = string.Empty;

    /// <summary>
    /// 获取或设置平台标识（如 xhs, douyin, bilibili）。
    /// </summary>
    public string Platform { get; set; } = string.Empty;

    /// <summary>
    /// 获取或设置 Cookies 字符串。
    /// </summary>
    public string Cookies { get; set; } = string.Empty;

    /// <summary>
    /// 获取或设置关联的代理 IP URL（可选）。
    /// </summary>
    public string? ProxyUrl { get; set; }

    /// <summary>
    /// 获取或设置账号状态。
    /// </summary>
    public AccountStatus Status { get; set; } = AccountStatus.Active;

    /// <summary>
    /// 获取或设置最后使用时间。
    /// </summary>
    public DateTimeOffset? LastUsedAt { get; set; }

    /// <summary>
    /// 获取或设置失败次数。
    /// </summary>
    public int FailureCount { get; set; }

    /// <summary>
    /// 获取或设置扩展数据（如 User-Agent 等）。
    /// </summary>
    public Dictionary<string, string>? Metadata { get; set; }
}

/// <summary>
/// 账号状态枚举。
/// </summary>
public enum AccountStatus
{
    /// <summary>正常可用。</summary>
    Active,

    /// <summary>无效（Cookie 失效）。</summary>
    Invalid,

    /// <summary>被限流。</summary>
    RateLimited,

    /// <summary>已禁用。</summary>
    Disabled
}
