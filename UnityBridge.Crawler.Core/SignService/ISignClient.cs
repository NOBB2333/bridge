namespace UnityBridge.Crawler.Core.SignService;

/// <summary>
/// 签名服务客户端接口。
/// </summary>
public interface ISignClient
{
    /// <summary>
    /// 获取小红书签名。
    /// </summary>
    Task<XhsSignResult> GetXhsSignAsync(XhsSignRequest request, CancellationToken ct = default);

    /// <summary>
    /// 获取抖音签名。
    /// </summary>
    Task<DouyinSignResult> GetDouyinSignAsync(DouyinSignRequest request, CancellationToken ct = default);

    /// <summary>
    /// 获取快手签名。
    /// </summary>
    Task<KuaishouSignResult> GetKuaishouSignAsync(KuaishouSignRequest request, CancellationToken ct = default);

    /// <summary>
    /// 获取B站签名。
    /// </summary>
    Task<BilibiliSignResult> GetBilibiliSignAsync(BilibiliSignRequest request, CancellationToken ct = default);

    /// <summary>
    /// 获取知乎签名。
    /// </summary>
    Task<ZhihuSignResult> GetZhihuSignAsync(ZhihuSignRequest request, CancellationToken ct = default);

    /// <summary>
    /// 健康检查。
    /// </summary>
    Task<bool> PingAsync(CancellationToken ct = default);
}

#region Sign Request/Result Models

/// <summary>小红书签名请求。</summary>
public class XhsSignRequest
{
    public string Uri { get; set; } = string.Empty;
    public string? Data { get; set; }
    public string Cookies { get; set; } = string.Empty;
}

/// <summary>小红书签名结果。</summary>
public class XhsSignResult
{
    [JsonPropertyName("x-s")]
    public string XS { get; set; } = string.Empty;

    [JsonPropertyName("x-t")]
    public string XT { get; set; } = string.Empty;

    [JsonPropertyName("x-s-common")]
    public string XSCommon { get; set; } = string.Empty;

    [JsonPropertyName("x-b3-traceid")]
    public string XB3TraceId { get; set; } = string.Empty;
}

/// <summary>抖音签名请求。</summary>
public class DouyinSignRequest
{
    public string Uri { get; set; } = string.Empty;
    public string Cookies { get; set; } = string.Empty;
}

/// <summary>抖音签名结果。</summary>
public class DouyinSignResult
{
    [JsonPropertyName("a-bogus")]
    public string ABogus { get; set; } = string.Empty;

    [JsonPropertyName("msToken")]
    public string MsToken { get; set; } = string.Empty;
}

/// <summary>快手签名请求。</summary>
public class KuaishouSignRequest
{
    public string Uri { get; set; } = string.Empty;
    public string Cookies { get; set; } = string.Empty;
}

/// <summary>快手签名结果。</summary>
public class KuaishouSignResult
{
    [JsonPropertyName("did")]
    public string Did { get; set; } = string.Empty;
}

/// <summary>B站签名请求。</summary>
public class BilibiliSignRequest
{
    public Dictionary<string, string> ReqData { get; set; } = new();
    public string Cookies { get; set; } = string.Empty;
}

/// <summary>B站签名结果。</summary>
public class BilibiliSignResult
{
    [JsonPropertyName("wts")]
    public string Wts { get; set; } = string.Empty;

    [JsonPropertyName("w_rid")]
    public string WRid { get; set; } = string.Empty;
}

/// <summary>知乎签名请求。</summary>
public class ZhihuSignRequest
{
    public string Uri { get; set; } = string.Empty;
    public string Cookies { get; set; } = string.Empty;
}

/// <summary>知乎签名结果。</summary>
public class ZhihuSignResult
{
    [JsonPropertyName("x-zse-96")]
    public string XZse96 { get; set; } = string.Empty;

    [JsonPropertyName("x-zst-81")]
    public string XZst81 { get; set; } = string.Empty;
}

#endregion

