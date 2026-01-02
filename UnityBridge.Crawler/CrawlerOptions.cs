namespace UnityBridge.Crawler;

/// <summary>
/// 爬虫配置模型。
/// </summary>
public class CrawlerOptions
{
    /// <summary>签名服务地址。</summary>
    public string SignServerUrl { get; set; } = "http://localhost:8888";

    /// <summary>数据库配置。</summary>
    public DatabaseOptions Database { get; set; } = new();

    /// <summary>默认延迟配置。</summary>
    public DelayOptions DefaultDelay { get; set; } = new();

    /// <summary>默认最大页数。</summary>
    public int MaxPages { get; set; } = 10;

    /// <summary>各平台配置。</summary>
    public PlatformsOptions Platforms { get; set; } = new();
}

public class DatabaseOptions
{
    /// <summary>数据库类型：SQLite 或 MySQL。</summary>
    public string Type { get; set; } = "SQLite";

    /// <summary>数据库连接字符串。</summary>
    public string ConnectionString { get; set; } = "Data Source=crawler.db;";
}

public class DelayOptions
{
    /// <summary>最小延迟（毫秒）。</summary>
    public int MinMs { get; set; } = 1000;

    /// <summary>最大延迟（毫秒）。</summary>
    public int MaxMs { get; set; } = 3000;
}

public class PlatformsOptions
{
    public PlatformConfig XiaoHongShu { get; set; } = new();
    public PlatformConfig BiliBili { get; set; } = new();
    public PlatformConfig Douyin { get; set; } = new();
    public PlatformConfig Tieba { get; set; } = new();
    public PlatformConfig Kuaishou { get; set; } = new();
    public PlatformConfig Zhihu { get; set; } = new();
    public PlatformConfig Weibo { get; set; } = new();
}

public class PlatformConfig
{
    /// <summary>是否启用。</summary>
    public bool Enabled { get; set; } = true;

    /// <summary>平台 Cookies。</summary>
    public string Cookies { get; set; } = string.Empty;
}
