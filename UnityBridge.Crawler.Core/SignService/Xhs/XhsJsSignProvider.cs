namespace UnityBridge.Crawler.Core.SignService.Xhs;

using System.Reflection;
using Jint;

/// <summary>
/// 小红书 JavaScript 签名提供者，使用 Jint JS 引擎。
/// </summary>
public class XhsJsSignProvider : IDisposable
{
    private readonly Engine _jsEngine;
    private readonly object _lock = new();
    private bool _initialized;

    // 默认的 b1 值 (localStorage.getItem("b1"))
    private const string DefaultB1 = "I38rHdgsjopgIvesdVwgIC+oIELmBZ5e3VwXLgFTIxS3bqwErFeexd0ekncAzMFYnqthIhJeSnMDKutRI3KsYorWHPtGrbV0P9WfIi/eWc6eYqtyQApPI37ekmR1QL+5Ii6sdnoeSfqYHqwl2qt5B0DoIvMzOZQqZVw7IxOeTqwr4qtiIkrOIi/skccxICLdI3Oe0utl2ADZsLveDSKsSPw5IEvsiutJOqw8BVwfPpdeTDWOIx4VIiu6ZPwbPut5IvlaLbgs3qtxIxes1VwHIkumIkIyejgsY/WTge7sjutKrZgedWI9gfKeYWZGI36eWPwyIEJefut0ocVAPBLLI3Aeiqt3cZ7sVom4IESyIhEqQd4AICY24F4gIiifpVwAICZVJo3sWWJs1qwiIvdef97e0ekKIi/e1piS8qwUIE7s1fds6WAeiVwqed5sdut3IxILbd6sdqtDbgKs0PwgIv8aI3z5rqwGBVtwzfTsKD7sdBdskut+Iioed/As1SiiIkKs0F6s3nVuIkge1Pt0IkVkwPwwNVtMI3/e1qtdIkKs1VwVIEesdutA+qwKsuw7IvrRIxDgJfIj2IJexVtVIhiKIi6eDVw/bz4zLadsYjmfIkWo4VtPmVw5IvAe3qtk+LJeTl5sTSEyIEJekdgs3PtsnPwqI35sSPt0Ih/sV04TIk0ejjNsfqw7Iv3sVut04B8qIkWyIvKsxFOekzNsdAKsYPtKIiMFI3MurVtKIvzjIh6s6lFut//sWqtaI3IYbuwl";

    public XhsJsSignProvider()
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

            var jsCode = LoadEmbeddedResource("xhs_xs_new.js");
            if (!string.IsNullOrEmpty(jsCode))
            {
                _jsEngine.Execute(jsCode);
            }

            _initialized = true;
        }
    }

    /// <summary>
    /// 生成 XHS 签名
    /// </summary>
    public XhsSignResult Sign(string uri, string? data, string cookies)
    {
        Initialize();

        var a1 = XhsSignHelper.GetA1FromCookies(cookies);

        lock (_lock)
        {
            try
            {
                // 调用 JS sign 函数: sign(uri, data, cookies)
                var result = _jsEngine.Invoke("sign", uri, data ?? "", cookies);

                if (result?.ToObject() is IDictionary<string, object> resultDict)
                {
                    resultDict.TryGetValue("x-s", out var xsObj);
                    resultDict.TryGetValue("x-t", out var xtObj);
                    var xS = xsObj?.ToString() ?? "";
                    var xT = xtObj?.ToString() ?? "";

                    // 使用 C# 辅助函数生成 x-s-common
                    return XhsSignHelper.Sign(a1, DefaultB1, xS, xT);
                }
            }
            catch
            {
                // JS 执行失败时返回空结果
            }
        }

        return new XhsSignResult
        {
            XS = "",
            XT = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString(),
            XSCommon = "",
            XB3TraceId = XhsSignHelper.GetB3TraceId()
        };
    }

    /// <summary>
    /// 加载嵌入的 JS 资源文件
    /// </summary>
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
