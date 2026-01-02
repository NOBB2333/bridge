namespace UnityBridge.Crawler.Core.SignService.Bilibili;

using System.Security.Cryptography;
using System.Text;
using System.Web;

/// <summary>
/// Bilibili WBI 签名算法实现。
/// 参考: https://socialsisteryi.github.io/bilibili-API-collect/docs/misc/sign/wbi.html
/// </summary>
public class BilibiliWbiSign
{
    private readonly string _imgKey;
    private readonly string _subKey;

    /// <summary>
    /// WBI 混淆表 - 用于生成 mixin_key
    /// </summary>
    private static readonly int[] MixinKeyEncTab =
    [
        46, 47, 18, 2, 53, 8, 23, 32, 15, 50, 10, 31, 58, 3, 45, 35, 27, 43, 5, 49,
        33, 9, 42, 19, 29, 28, 14, 39, 12, 38, 41, 13, 37, 48, 7, 16, 24, 55, 40,
        61, 26, 17, 0, 1, 60, 51, 30, 4, 22, 25, 54, 21, 56, 59, 6, 63, 57, 62, 11,
        36, 20, 34, 44, 52
    ];

    public BilibiliWbiSign(string imgKey, string subKey)
    {
        _imgKey = imgKey;
        _subKey = subKey;
    }

    /// <summary>
    /// 获取混淆后的加盐 key (mixin_key)
    /// </summary>
    private string GetMixinKey()
    {
        var rawKey = _imgKey + _subKey;
        var sb = new StringBuilder(32);
        foreach (var i in MixinKeyEncTab)
        {
            if (i < rawKey.Length)
            {
                sb.Append(rawKey[i]);
            }
        }
        return sb.ToString()[..32];
    }

    /// <summary>
    /// 对请求参数进行 WBI 签名。
    /// </summary>
    /// <param name="reqData">请求参数字典</param>
    /// <returns>包含 wts 和 w_rid 的签名后参数字典</returns>
    public Dictionary<string, string> Sign(Dictionary<string, string> reqData)
    {
        // 添加时间戳
        var wts = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
        reqData["wts"] = wts;

        // 按 key 字典序排序
        var sortedParams = reqData.OrderBy(x => x.Key).ToList();

        // 过滤特殊字符并 URL 编码
        var queryParts = new List<string>();
        foreach (var kvp in sortedParams)
        {
            var filteredValue = FilterChars(kvp.Value);
            var encodedKey = HttpUtility.UrlEncode(kvp.Key);
            var encodedValue = HttpUtility.UrlEncode(filteredValue);
            queryParts.Add($"{encodedKey}={encodedValue}");
        }

        var query = string.Join("&", queryParts);
        var mixinKey = GetMixinKey();

        // 计算 MD5 签名
        using var md5 = MD5.Create();
        var hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(query + mixinKey));
        var wRid = Convert.ToHexStringLower(hashBytes);

        reqData["w_rid"] = wRid;
        return reqData;
    }

    /// <summary>
    /// 过滤 value 中的特殊字符: !'()*
    /// </summary>
    private static string FilterChars(string value)
    {
        if (string.IsNullOrEmpty(value)) return value;

        var sb = new StringBuilder(value.Length);
        foreach (var c in value)
        {
            if (c != '!' && c != '\'' && c != '(' && c != ')' && c != '*')
            {
                sb.Append(c);
            }
        }
        return sb.ToString();
    }
}
