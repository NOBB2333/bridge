namespace UnityBridge.Crawler.Core.SignService.Zhihu;

using System.Reflection;
using Jint;

/// <summary>
/// 知乎 JavaScript 签名提供者
/// </summary>
public class ZhihuJsSignProvider : IDisposable
{
    private readonly Engine _jsEngine;
    private readonly object _lock = new();
    private bool _initialized;

    public ZhihuJsSignProvider()
    {
        _jsEngine = new Engine(cfg => cfg
            .LimitRecursion(1000)
            .TimeoutInterval(TimeSpan.FromSeconds(30))
            .LimitMemory(50_000_000) // 50MB
        );
    }

    /// <summary>
    /// 初始化 JS 引擎，加载签名脚本
    /// </summary>
    private void Initialize()
    {
        if (_initialized) return;

        lock (_lock)
        {
            if (_initialized) return;

            var jsCode = LoadEmbeddedResource("zhihu.js");
            if (!string.IsNullOrEmpty(jsCode))
            {
                _jsEngine.Execute(jsCode);
            }

            _initialized = true;
        }
    }

    /// <summary>
    /// 生成知乎签名
    /// </summary>
    public ZhihuSignResult Sign(string uri, string cookies)
    {
        Initialize();

        lock (_lock)
        {
            try
            {
                // 调用 JS get_sign 函数
                var result = _jsEngine.Invoke("get_sign", uri, cookies);

                if (result?.ToObject() is IDictionary<string, object> resultDict)
                {
                    resultDict.TryGetValue("x-zse-96", out var zse96Obj);
                    resultDict.TryGetValue("x-zst-81", out var zst81Obj);
                    return new ZhihuSignResult
                    {
                        XZse96 = zse96Obj?.ToString() ?? "",
                        XZst81 = zst81Obj?.ToString() ?? ""
                    };
                }
            }
            catch
            {
                // JS 执行失败
            }
        }

        return new ZhihuSignResult();
    }

    private static string LoadEmbeddedResource(string resourceName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var fullResourceName = assembly.GetManifestResourceNames()
            .FirstOrDefault(n => n.EndsWith(resourceName, StringComparison.OrdinalIgnoreCase));

        if (fullResourceName == null) return "";

        using var stream = assembly.GetManifestResourceStream(fullResourceName);
        if (stream == null) return "";

        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    public void Dispose()
    {
        _jsEngine.Dispose();
        GC.SuppressFinalize(this);
    }
}

/// <summary>
/// 知乎签名结果
/// </summary>
public class ZhihuSignResult
{
    public string XZse96 { get; set; } = "";
    public string XZst81 { get; set; } = "";
}
