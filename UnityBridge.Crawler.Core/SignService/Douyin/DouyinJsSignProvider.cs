namespace UnityBridge.Crawler.Core.SignService.Douyin;

using System.Reflection;
using Jint;

/// <summary>
/// 抖音 JavaScript 签名提供者
/// </summary>
public class DouyinJsSignProvider : IDisposable
{
    private readonly Engine _jsEngine;
    private readonly object _lock = new();
    private bool _initialized;

    public DouyinJsSignProvider()
    {
        _jsEngine = new Engine(cfg => cfg
            .LimitRecursion(1000)
            .TimeoutInterval(TimeSpan.FromSeconds(30))
            .LimitMemory(100_000_000) // 100MB (douyin.js 较大)
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

            var jsCode = LoadEmbeddedResource("douyin.js");
            if (!string.IsNullOrEmpty(jsCode))
            {
                _jsEngine.Execute(jsCode);
            }

            _initialized = true;
        }
    }

    /// <summary>
    /// 生成抖音 a-bogus 签名
    /// </summary>
    /// <param name="queryParams">请求的 query 参数字符串</param>
    /// <param name="postData">POST 数据 (可选)</param>
    /// <param name="userAgent">User-Agent</param>
    public DouyinSignResult Sign(string queryParams, string postData = "", string? userAgent = null)
    {
        Initialize();

        userAgent ??= GetDefaultUserAgent();

        lock (_lock)
        {
            try
            {
                // 调用 JS get_abogus 函数
                var result = _jsEngine.Invoke("get_abogus", queryParams, postData, userAgent);
                var aBogus = result?.ToString() ?? "";

                return new DouyinSignResult { ABogus = aBogus };
            }
            catch
            {
                // JS 执行失败
            }
        }

        return new DouyinSignResult { ABogus = "" };
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

    private static string GetDefaultUserAgent() =>
        "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36";

    public void Dispose()
    {
        _jsEngine.Dispose();
        GC.SuppressFinalize(this);
    }
}

/// <summary>
/// 抖音签名结果
/// </summary>
public class DouyinSignResult
{
    public string ABogus { get; set; } = "";
}
