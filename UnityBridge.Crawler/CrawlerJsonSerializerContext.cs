using System.Text.Json.Serialization;

namespace UnityBridge.Crawler;

/// <summary>
/// 爬虫主程序 JSON 序列化上下文（AOT 源生成器）。
/// 聚合所有平台的配置类型。
/// </summary>
[JsonSerializable(typeof(CrawlerOptions))]
[JsonSerializable(typeof(DatabaseOptions))]
[JsonSerializable(typeof(DelayOptions))]
[JsonSerializable(typeof(PlatformsOptions))]
[JsonSerializable(typeof(PlatformConfig))]
[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    WriteIndented = true)]
public partial class CrawlerJsonSerializerContext : JsonSerializerContext
{
}
