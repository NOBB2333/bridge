namespace UnityBridge.Crawler.Douyin;

/// <summary>
/// 抖音 API 请求基类。
/// </summary>
public abstract class DouyinRequest : ICommonRequest
{
    /// <summary>
    /// 获取或设置自定义请求头。
    /// </summary>
    [JsonIgnore]
    public Dictionary<string, string>? ExtraHeaders { get; set; }

    public int? Timeout { get; set; }
}

/// <summary>
/// 抖音 API 响应基类。
/// </summary>
public class DouyinResponse : CommonResponseBase
{
    /// <summary>
    /// 获取或设置状态码。
    /// </summary>
    [JsonPropertyName("status_code")]
    public int StatusCode { get; set; }

    /// <summary>
    /// 获取或设置状态消息。
    /// </summary>
    [JsonPropertyName("status_msg")]
    public string? StatusMsg { get; set; }

    /// <summary>
    /// 判断响应是否成功。
    /// </summary>
    public bool IsSuccessful() => StatusCode == 0;
}

/// <summary>
/// 抖音带数据响应基类。
/// </summary>
public class DouyinResponse<T> : DouyinResponse
{
    /// <summary>
    /// 获取或设置数据。
    /// </summary>
    [JsonPropertyName("data")]
    public T? Data { get; set; }
}
