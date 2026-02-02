namespace UnityBridge.Crawler.Weibo;

/// <summary>
/// 微博请求基类。
/// </summary>
public abstract class WeiboRequest : CommonRequestBase
{
    /// <summary>
    /// 获取或设置自定义请求头。
    /// </summary>
    [JsonIgnore]
    public Dictionary<string, string>? ExtraHeaders { get; set; }

    public int? Timeout { get; set; }
}

/// <summary>
/// 微博 API 响应基类。
/// </summary>
public class WeiboResponse : CommonResponseBase
{
    /// <summary>
    /// 获取或设置 OK 状态。
    /// </summary>
    [JsonPropertyName("ok")]
    public int Ok { get; set; }

    /// <summary>
    /// 获取或设置消息。
    /// </summary>
    [JsonPropertyName("msg")]
    public string? Msg { get; set; }

    /// <summary>
    /// 判断响应是否成功。
    /// </summary>
    public bool IsSuccessful() => Ok == 1 || Ok == 0;
}

/// <summary>
/// 微博带数据响应基类。
/// </summary>
public class WeiboResponse<T> : WeiboResponse
{
    /// <summary>
    /// 获取或设置数据。
    /// </summary>
    [JsonPropertyName("data")]
    public T? Data { get; set; }
}
