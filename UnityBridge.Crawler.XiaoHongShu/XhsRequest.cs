namespace UnityBridge.Crawler.XiaoHongShu;

/// <summary>
/// 小红书 API 请求基类。
/// </summary>
public abstract class XhsRequest : ICommonRequest
{
    /// <summary>
    /// 获取或设置自定义请求头。
    /// </summary>
    [JsonIgnore]
    public Dictionary<string, string>? ExtraHeaders { get; set; }

    public int? Timeout { get; set; }
}
