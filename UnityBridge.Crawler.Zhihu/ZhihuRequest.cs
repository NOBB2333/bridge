namespace UnityBridge.Crawler.Zhihu;

/// <summary>
/// 知乎请求基类。
/// </summary>
public abstract class ZhihuRequest : ICommonRequest
{
    /// <summary>
    /// 获取或设置自定义请求头。
    /// </summary>
    [JsonIgnore]
    public Dictionary<string, string>? ExtraHeaders { get; set; }

    public int? Timeout { get; set; }
}

/// <summary>
/// 知乎 API 响应基类。
/// </summary>
public class ZhihuResponse : CommonResponseBase
{
    /// <summary>
    /// 获取或设置错误信息。
    /// </summary>
    [JsonPropertyName("error")]
    public ZhihuError? Error { get; set; }

    /// <summary>
    /// 判断响应是否成功。
    /// </summary>
    public bool IsSuccessful() => Error is null;
}

/// <summary>
/// 知乎错误信息。
/// </summary>
public class ZhihuError
{
    [JsonPropertyName("code")]
    public int Code { get; set; }

    [JsonPropertyName("message")]
    public string? Message { get; set; }
}

/// <summary>
/// 知乎分页响应基类。
/// </summary>
public class ZhihuPagedResponse<T> : ZhihuResponse
{
    [JsonPropertyName("data")]
    public List<T>? Data { get; set; }

    [JsonPropertyName("paging")]
    public ZhihuPaging? Paging { get; set; }
}

/// <summary>
/// 知乎分页信息。
/// </summary>
public class ZhihuPaging
{
    [JsonPropertyName("is_end")]
    public bool IsEnd { get; set; }

    [JsonPropertyName("next")]
    public string? Next { get; set; }

    [JsonPropertyName("previous")]
    public string? Previous { get; set; }

    [JsonPropertyName("totals")]
    public int Totals { get; set; }
}
