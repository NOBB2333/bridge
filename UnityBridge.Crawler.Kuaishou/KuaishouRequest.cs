namespace UnityBridge.Crawler.Kuaishou;

/// <summary>
/// 快手请求基类。
/// </summary>
public abstract class KuaishouRequest : ICommonRequest
{
    /// <summary>
    /// 获取或设置自定义请求头。
    /// </summary>
    [JsonIgnore]
    public Dictionary<string, string>? ExtraHeaders { get; set; }

    /// <summary>
    /// 获取或设置请求超时时间（毫秒）。
    /// </summary>
    public int? Timeout { get; set; }
}

/// <summary>
/// 快手 GraphQL 响应基类。
/// </summary>
public class KuaishouResponse : CommonResponseBase
{
    /// <summary>
    /// 获取或设置错误信息。
    /// </summary>
    [JsonPropertyName("errors")]
    public List<KuaishouError>? Errors { get; set; }

    /// <summary>
    /// 判断响应是否成功。
    /// </summary>
    public bool IsSuccessful() => Errors is null or { Count: 0 };
}

/// <summary>
/// 快手错误信息。
/// </summary>
public class KuaishouError
{
    [JsonPropertyName("message")]
    public string? Message { get; set; }

    [JsonPropertyName("extensions")]
    public Dictionary<string, object>? Extensions { get; set; }
}

/// <summary>
/// 快手带数据响应基类。
/// </summary>
public class KuaishouResponse<T> : KuaishouResponse
{
    /// <summary>
    /// 获取或设置数据。
    /// </summary>
    [JsonPropertyName("data")]
    public T? Data { get; set; }
}
