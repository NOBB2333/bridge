namespace UnityBridge.Crawler.XiaoHongShu;

/// <summary>
/// 小红书 API 响应基类。
/// </summary>
public class XhsResponse : CommonResponseBase
{
    /// <summary>
    /// 获取或设置是否成功。
    /// </summary>
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    /// <summary>
    /// 获取或设置错误码。
    /// </summary>
    [JsonPropertyName("code")]
    public int? Code { get; set; }

    /// <summary>
    /// 获取或设置错误消息。
    /// </summary>
    [JsonPropertyName("msg")]
    public string? Message { get; set; }

    /// <summary>
    /// 判断响应是否成功。
    /// </summary>
    public bool IsSuccessful() => Success || Code == 0 || Code is null;
}
