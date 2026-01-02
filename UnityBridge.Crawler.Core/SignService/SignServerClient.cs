namespace UnityBridge.Crawler.Core.SignService;

/// <summary>
/// 签名服务 HTTP 客户端，调用 MediaCrawlerPro-SignSrv。
/// </summary>
public class SignServerClient : ISignClient, IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    private readonly bool _disposeClient;

    /// <summary>
    /// 初始化签名服务客户端。
    /// </summary>
    /// <param name="signServerUrl">签名服务地址（如 http://localhost:8888）。</param>
    /// <param name="httpClient">可选的 HttpClient 实例。</param>
    public SignServerClient(string signServerUrl = "http://localhost:8888", HttpClient? httpClient = null)
    {
        _baseUrl = signServerUrl.TrimEnd('/');
        _disposeClient = httpClient is null;
        _httpClient = httpClient ?? new HttpClient();
    }

    /// <inheritdoc/>
    public async Task<XhsSignResult> GetXhsSignAsync(XhsSignRequest request, CancellationToken ct = default)
    {
        var payload = new
        {
            uri = request.Uri,
            data = request.Data,
            cookies = request.Cookies
        };

        var response = await PostJsonAsync<SignServerResponse<XhsSignResult>>("/api/sign/xhs", payload, ct);
        return response?.Data ?? new XhsSignResult();
    }

    /// <inheritdoc/>
    public async Task<DouyinSignResult> GetDouyinSignAsync(DouyinSignRequest request, CancellationToken ct = default)
    {
        var payload = new
        {
            uri = request.Uri,
            cookies = request.Cookies
        };

        var response = await PostJsonAsync<SignServerResponse<DouyinSignResult>>("/api/sign/douyin", payload, ct);
        return response?.Data ?? new DouyinSignResult();
    }

    /// <inheritdoc/>
    public async Task<KuaishouSignResult> GetKuaishouSignAsync(KuaishouSignRequest request, CancellationToken ct = default)
    {
        var payload = new
        {
            uri = request.Uri,
            cookies = request.Cookies
        };

        var response = await PostJsonAsync<SignServerResponse<KuaishouSignResult>>("/api/sign/kuaishou", payload, ct);
        return response?.Data ?? new KuaishouSignResult();
    }

    /// <inheritdoc/>
    public async Task<BilibiliSignResult> GetBilibiliSignAsync(BilibiliSignRequest request, CancellationToken ct = default)
    {
        var payload = new
        {
            req_data = request.ReqData,
            cookies = request.Cookies
        };

        var response = await PostJsonAsync<SignServerResponse<BilibiliSignResult>>("/api/sign/bilibili", payload, ct);
        return response?.Data ?? new BilibiliSignResult();
    }

    public Task<ZhihuSignResult> GetZhihuSignAsync(ZhihuSignRequest request, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<bool> PingAsync(CancellationToken ct = default)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/api/ping", ct);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    private async Task<T?> PostJsonAsync<T>(string path, object payload, CancellationToken ct)
    {
        var json = JsonSerializer.Serialize(payload);
        using var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"{_baseUrl}{path}", content, ct);
        response.EnsureSuccessStatusCode();

        var responseJson = await response.Content.ReadAsStringAsync(ct);
        return JsonSerializer.Deserialize<T>(responseJson, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }

    public void Dispose()
    {
        if (_disposeClient)
        {
            _httpClient.Dispose();
        }
    }
}

/// <summary>签名服务通用响应包装。</summary>
internal class SignServerResponse<T>
{
    [JsonPropertyName("code")]
    public int Code { get; set; }

    [JsonPropertyName("message")]
    public string? Message { get; set; }

    [JsonPropertyName("data")]
    public T? Data { get; set; }
}
