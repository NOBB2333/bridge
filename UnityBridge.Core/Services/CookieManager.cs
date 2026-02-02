using System.Collections.Concurrent;
using System.Net;

namespace UnityBridge.Core.Services;

/// <summary>
/// Cookie 管理器，按域名管理 Cookie（内存存储）。
/// </summary>
internal class CookieManager
{
    private readonly ConcurrentDictionary<string, CookieContainer> _cookieContainers = new();

    /// <summary>
    /// 获取指定域名的 Cookie 容器。
    /// </summary>
    public CookieContainer GetCookieContainer(string domain)
    {
        return _cookieContainers.GetOrAdd(domain, _ => new CookieContainer());
    }

    /// <summary>
    /// 从响应中提取并存储 Cookie。
    /// </summary>
    public void ExtractCookiesFromResponse(string domain, HttpResponseMessage response)
    {
        if (!response.Headers.TryGetValues("Set-Cookie", out var setCookieHeaders))
            return;

        var container = GetCookieContainer(domain);
        var uri = new Uri($"http://{domain}");

        foreach (var setCookieHeader in setCookieHeaders)
        {
            try
            {
                // 简单解析 Set-Cookie 头（实际应该更完善）
                var parts = setCookieHeader.Split(';');
                if (parts.Length > 0)
                {
                    var cookiePart = parts[0].Trim();
                    var keyValue = cookiePart.Split('=', 2);
                    if (keyValue.Length == 2)
                    {
                        var cookie = new Cookie(keyValue[0].Trim(), keyValue[1].Trim())
                        {
                            Domain = domain
                        };
                        container.Add(uri, cookie);
                    }
                }
            }
            catch
            {
                // 忽略解析错误
            }
        }
    }

    /// <summary>
    /// 获取指定域名的 Cookie 字符串。
    /// </summary>
    public string GetCookieHeader(string domain, Uri uri)
    {
        if (!_cookieContainers.TryGetValue(domain, out var container))
            return string.Empty;

        var cookies = container.GetCookies(uri);
        return string.Join("; ", cookies.Cast<Cookie>().Select(c => $"{c.Name}={c.Value}"));
    }

    /// <summary>
    /// 清除指定域名的 Cookie。
    /// </summary>
    public void ClearCookies(string domain)
    {
        _cookieContainers.TryRemove(domain, out _);
    }

    /// <summary>
    /// 清除所有 Cookie。
    /// </summary>
    public void ClearAllCookies()
    {
        _cookieContainers.Clear();
    }
}

