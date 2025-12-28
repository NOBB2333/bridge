using System.Collections.Concurrent;
using System.Net;

namespace UnityBridge.Core;

/// <summary>
/// 代理池管理器，负责代理的选择、健康检查等。
/// </summary>
internal class ProxyPoolManager
{
    private readonly ConcurrentQueue<ProxyInfo> _proxyQueue = new();
    private readonly ConcurrentDictionary<string, ProxyInfo> _proxyMap = new();
    private readonly ProxySelectionStrategy _strategy;
    private readonly Random _random = new();
    private int _roundRobinIndex = 0;

    public ProxyPoolManager(List<string> proxyList, ProxySelectionStrategy strategy)
    {
        _strategy = strategy;
        foreach (var proxyUrl in proxyList)
        {
            if (TryParseProxy(proxyUrl, out var proxyInfo))
            {
                _proxyQueue.Enqueue(proxyInfo);
                _proxyMap[proxyUrl] = proxyInfo;
            }
        }
    }

    /// <summary>
    /// 获取下一个可用的代理。
    /// </summary>
    public WebProxy? GetNextProxy()
    {
        if (_proxyQueue.IsEmpty)
            return null;

        ProxyInfo? proxyInfo = null;

        switch (_strategy)
        {
            case ProxySelectionStrategy.RoundRobin:
                var proxies = _proxyQueue.ToArray();
                if (proxies.Length > 0)
                {
                    proxyInfo = proxies[_roundRobinIndex % proxies.Length];
                    _roundRobinIndex++;
                }
                break;

            case ProxySelectionStrategy.Random:
                var proxyArray = _proxyQueue.ToArray();
                if (proxyArray.Length > 0)
                {
                    proxyInfo = proxyArray[_random.Next(proxyArray.Length)];
                }
                break;
        }

        if (proxyInfo == null)
            return null;

        return new WebProxy(proxyInfo.Host, proxyInfo.Port)
        {
            Credentials = proxyInfo.Credentials
        };
    }

    /// <summary>
    /// 标记代理为不可用。
    /// </summary>
    public void MarkProxyAsUnavailable(string proxyUrl)
    {
        if (_proxyMap.TryRemove(proxyUrl, out var proxyInfo))
        {
            // 从队列中移除（简单实现，实际可以更复杂）
            var tempQueue = new ConcurrentQueue<ProxyInfo>();
            while (_proxyQueue.TryDequeue(out var item))
            {
                if (item.Url != proxyUrl)
                {
                    tempQueue.Enqueue(item);
                }
            }
            while (tempQueue.TryDequeue(out var item))
            {
                _proxyQueue.Enqueue(item);
            }
        }
    }

    /// <summary>
    /// 获取代理数量。
    /// </summary>
    public int Count => _proxyQueue.Count;

    private static bool TryParseProxy(string proxyUrl, out ProxyInfo proxyInfo)
    {
        proxyInfo = null!;
        if (string.IsNullOrWhiteSpace(proxyUrl))
            return false;

        try
        {
            var uri = new Uri(proxyUrl);
            var host = uri.Host;
            var port = uri.Port > 0 ? uri.Port : (uri.Scheme == "https" ? 443 : 80);

            NetworkCredential? credentials = null;
            if (!string.IsNullOrEmpty(uri.UserInfo))
            {
                var parts = uri.UserInfo.Split(':');
                if (parts.Length == 2)
                {
                    credentials = new NetworkCredential(parts[0], parts[1]);
                }
            }

            proxyInfo = new ProxyInfo
            {
                Url = proxyUrl,
                Host = host,
                Port = port,
                Scheme = uri.Scheme,
                Credentials = credentials
            };

            return true;
        }
        catch
        {
            return false;
        }
    }

    private class ProxyInfo
    {
        public string Url { get; set; } = string.Empty;
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }
        public string Scheme { get; set; } = "http";
        public NetworkCredential? Credentials { get; set; }
    }
}

