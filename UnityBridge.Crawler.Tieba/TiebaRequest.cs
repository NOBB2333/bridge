namespace UnityBridge.Crawler.Tieba;

/// <summary>
/// 贴吧请求基类。
/// </summary>
public abstract class TiebaRequest : ICommonRequest
{
    /// <summary>
    /// 获取或设置自定义请求头。
    /// </summary>
    [JsonIgnore]
    public Dictionary<string, string>? ExtraHeaders { get; set; }

    public int? Timeout { get; set; }
}

/// <summary>
/// 贴吧 API 响应基类（JSON API）。
/// </summary>
public class TiebaResponse : CommonResponseBase
{
    /// <summary>
    /// 获取或设置错误码。
    /// </summary>
    [JsonPropertyName("no")]
    public int No { get; set; }

    /// <summary>
    /// 获取或设置错误消息。
    /// </summary>
    [JsonPropertyName("error")]
    public string? Error { get; set; }

    /// <summary>
    /// 判断响应是否成功。
    /// </summary>
    public bool IsSuccessful() => No == 0;
}

/// <summary>
/// 贴吧带数据响应基类。
/// </summary>
public class TiebaResponse<T> : TiebaResponse
{
    /// <summary>
    /// 获取或设置数据。
    /// </summary>
    [JsonPropertyName("data")]
    public T? Data { get; set; }
}
