namespace UnityBridge.Crawler.Core.SignService.Bilibili;

using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Caching.Memory;

/// <summary>
/// Bilibili 签名提供者，自动获取并缓存 img_key/sub_key。
/// </summary>
public class BilibiliSignProvider
{
    private const string NavApiUrl = "https://api.bilibili.com/x/web-interface/nav";
    private const string BilibiliReferer = "https://www.bilibili.com/";
    private const string CacheKeyImgKey = "bilibili_img_key";
    private const string CacheKeySubKey = "bilibili_sub_key";
    private static readonly TimeSpan CacheExpiration = TimeSpan.FromMinutes(10);

    // 固定的备用 key (当 API 获取失败时使用)
    private const string FallbackImgKey = "7cd084941338484aae1ad9425b84077c";
    private const string FallbackSubKey = "4932caff0ff746eab6f01bf08b70ac45";

    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _cache;

    public BilibiliSignProvider(HttpClient? httpClient = null, IMemoryCache? cache = null)
    {
        _httpClient = httpClient ?? new HttpClient();
        _cache = cache ?? new MemoryCache(new MemoryCacheOptions());
    }

    /// <summary>
    /// 对请求参数进行签名。
    /// </summary>
    public async Task<BilibiliSignResult> SignAsync(
        Dictionary<string, string> reqData,
        string? cookies = null,
        CancellationToken ct = default)
    {
        var (imgKey, subKey) = await GetWbiKeysAsync(cookies, ct);
        var sign = new BilibiliWbiSign(imgKey, subKey);
        var signedParams = sign.Sign(reqData);

        return new BilibiliSignResult
        {
            Wts = signedParams.GetValueOrDefault("wts", ""),
            WRid = signedParams.GetValueOrDefault("w_rid", "")
        };
    }

    /// <summary>
    /// 获取 WBI keys (img_key, sub_key)，优先从缓存获取。
    /// </summary>
    private async Task<(string imgKey, string subKey)> GetWbiKeysAsync(
        string? cookies,
        CancellationToken ct)
    {
        // 尝试从缓存获取
        if (_cache.TryGetValue(CacheKeyImgKey, out string? cachedImgKey) &&
            _cache.TryGetValue(CacheKeySubKey, out string? cachedSubKey) &&
            !string.IsNullOrEmpty(cachedImgKey) &&
            !string.IsNullOrEmpty(cachedSubKey))
        {
            return (cachedImgKey, cachedSubKey);
        }

        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, NavApiUrl);
            request.Headers.Add("Referer", BilibiliReferer);
            request.Headers.Add("User-Agent", GetUserAgent());
            if (!string.IsNullOrEmpty(cookies))
            {
                request.Headers.Add("Cookie", cookies);
            }

            var response = await _httpClient.SendAsync(request, ct);
            response.EnsureSuccessStatusCode();

            var navResp = await response.Content.ReadFromJsonAsync<NavApiResponse>(ct);
            if (navResp?.Code == 0 && navResp.Data?.WbiImg != null)
            {
                var imgUrl = navResp.Data.WbiImg.ImgUrl ?? "";
                var subUrl = navResp.Data.WbiImg.SubUrl ?? "";

                var imgKey = ExtractKeyFromUrl(imgUrl);
                var subKey = ExtractKeyFromUrl(subUrl);

                if (!string.IsNullOrEmpty(imgKey) && !string.IsNullOrEmpty(subKey))
                {
                    // 缓存
                    _cache.Set(CacheKeyImgKey, imgKey, CacheExpiration);
                    _cache.Set(CacheKeySubKey, subKey, CacheExpiration);
                    return (imgKey, subKey);
                }
            }
        }
        catch
        {
            // 获取失败，使用备用 key
        }

        return (FallbackImgKey, FallbackSubKey);
    }

    /// <summary>
    /// 从 URL 提取 key。
    /// 例如: https://i0.hdslb.com/bfs/wbi/7cd084941338484aae1ad9425b84077c.png -> 7cd084941338484aae1ad9425b84077c
    /// </summary>
    private static string ExtractKeyFromUrl(string url)
    {
        if (string.IsNullOrEmpty(url)) return "";

        var lastSlashIndex = url.LastIndexOf('/');
        if (lastSlashIndex < 0) return "";

        var fileName = url[(lastSlashIndex + 1)..];
        var dotIndex = fileName.IndexOf('.');
        return dotIndex > 0 ? fileName[..dotIndex] : fileName;
    }

    private static string GetUserAgent() =>
        "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36";

    #region API Response Models

    private class NavApiResponse
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }

        [JsonPropertyName("data")]
        public NavData? Data { get; set; }
    }

    private class NavData
    {
        [JsonPropertyName("wbi_img")]
        public WbiImgData? WbiImg { get; set; }
    }

    private class WbiImgData
    {
        [JsonPropertyName("img_url")]
        public string? ImgUrl { get; set; }

        [JsonPropertyName("sub_url")]
        public string? SubUrl { get; set; }
    }

    #endregion
}
