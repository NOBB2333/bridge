namespace UnityBridge.Crawler.BiliBili;

/// <summary>
/// B站 API 请求基类。
/// </summary>
public abstract class BiliRequest : ICommonRequest
{
    /// <summary>
    /// 获取或设置自定义请求头。
    /// </summary>
    [JsonIgnore]
    public Dictionary<string, string>? ExtraHeaders { get; set; }

    public int? Timeout { get; set; }
}

/// <summary>
/// B站 API 响应基类。
/// </summary>
public class BiliResponse : CommonResponseBase
{
    /// <summary>
    /// 获取或设置错误码。
    /// </summary>
    [JsonPropertyName("code")]
    public int Code { get; set; }

    /// <summary>
    /// 获取或设置错误消息。
    /// </summary>
    [JsonPropertyName("message")]
    public string? Message { get; set; }

    /// <summary>
    /// 判断响应是否成功。
    /// </summary>
    public bool IsSuccessful() => Code == 0;
}

/// <summary>
/// B站带数据响应基类。
/// </summary>
public class BiliResponse<T> : BiliResponse
{
    /// <summary>
    /// 获取或设置数据。
    /// </summary>
    [JsonPropertyName("data")]
    public T? Data { get; set; }
}
