namespace UnityBridge.Crawler.Core.SignService.Xhs;

using System.Text;
using System.Text.Json;
using System.Web;

/// <summary>
/// 小红书签名辅助类，移植自 Python logic/xhs/help.py
/// </summary>
public static class XhsSignHelper
{
    /// <summary>
    /// 自定义 Base64 查找表
    /// </summary>
    private static readonly char[] Lookup =
    [
        'Z', 'm', 's', 'e', 'r', 'b', 'B', 'o', 'H', 'Q', 't', 'N', 'P', '+', 'w', 'O',
        'c', 'z', 'a', '/', 'L', 'p', 'n', 'g', 'G', '8', 'y', 'J', 'q', '4', '2', 'K',
        'W', 'Y', 'j', '0', 'D', 'S', 'f', 'd', 'i', 'k', 'x', '3', 'V', 'T', '1', '6',
        'I', 'l', 'U', 'A', 'F', 'M', '9', '7', 'h', 'E', 'C', 'v', 'u', 'R', 'X', '5'
    ];

    /// <summary>
    /// CRC32 查找表 (ie 数组)
    /// </summary>
    private static readonly uint[] Crc32Table =
    [
        0, 1996959894, 3993919788, 2567524794, 124634137, 1886057615, 3915621685,
        2657392035, 249268274, 2044508324, 3772115230, 2547177864, 162941995,
        2125561021, 3887607047, 2428444049, 498536548, 1789927666, 4089016648,
        2227061214, 450548861, 1843258603, 4107580753, 2211677639, 325883990,
        1684777152, 4251122042, 2321926636, 335633487, 1661365465, 4195302755,
        2366115317, 997073096, 1281953886, 3579855332, 2724688242, 1006888145,
        1258607687, 3524101629, 2768942443, 901097722, 1119000684, 3686517206,
        2898065728, 853044451, 1172266101, 3705015759, 2882616665, 651767980,
        1373503546, 3369554304, 3218104598, 565507253, 1454621731, 3485111705,
        3099436303, 671266974, 1594198024, 3322730930, 2970347812, 795835527,
        1483230225, 3244367275, 3060149565, 1994146192, 31158534, 2563907772,
        4023717930, 1907459465, 112637215, 2680153253, 3904427059, 2013776290,
        251722036, 2517215374, 3775830040, 2137656763, 141376813, 2439277719,
        3865271297, 1802195444, 476864866, 2238001368, 4066508878, 1812370925,
        453092731, 2181625025, 4111451223, 1706088902, 314042704, 2344532202,
        4240017532, 1658658271, 366619977, 2362670323, 4224994405, 1303535960,
        984961486, 2747007092, 3569037538, 1256170817, 1037604311, 2765210733,
        3554079995, 1131014506, 879679996, 2909243462, 3663771856, 1141124467,
        855842277, 2852801631, 3708648649, 1342533948, 654459306, 3188396048,
        3373015174, 1466479909, 544179635, 3110523913, 3462522015, 1591671054,
        702138776, 2966460450, 3352799412, 1504918807, 783551873, 3082640443,
        3233442989, 3988292384, 2596254646, 62317068, 1957810842, 3939845945,
        2647816111, 81470997, 1943803523, 3814918930, 2489596804, 225274430,
        2053790376, 3826175755, 2466906013, 167816743, 2097651377, 4027552580,
        2265490386, 503444072, 1762050814, 4150417245, 2154129355, 426522225,
        1852507879, 4275313526, 2312317920, 282753626, 1742555852, 4189708143,
        2394877945, 397917763, 1622183637, 3604390888, 2714866558, 953729732,
        1340076626, 3518719985, 2797360999, 1068828381, 1219638859, 3624741850,
        2936675148, 906185462, 1090812512, 3747672003, 2825379669, 829329135,
        1181335161, 3412177804, 3160834842, 628085408, 1382605366, 3423369109,
        3138078467, 570562233, 1426400815, 3317316542, 2998733608, 733239954,
        1555261956, 3268935591, 3050360625, 752459403, 1541320221, 2607071920,
        3965973030, 1969922972, 40735498, 2617837225, 3943577151, 1913087877,
        83908371, 2512341634, 3803740692, 2075208622, 213261112, 2463272603,
        3855990285, 2094854071, 198958881, 2262029012, 4057260610, 1759359992,
        534414190, 2176718541, 4139329115, 1873836001, 414664567, 2282248934,
        4279200368, 1711684554, 285281116, 2405801727, 4167216745, 1634467795,
        376229701, 2685067896, 3608007406, 1308918612, 956543938, 2808555105,
        3495958263, 1231636301, 1047427035, 2932959818, 3654703836, 1088359270,
        936918000, 2847714899, 3736837829, 1202900863, 817233897, 3183342108,
        3401237130, 1404277552, 615818150, 3134207493, 3453421203, 1423857449,
        601450431, 3009837614, 3294710456, 1567103746, 711928724, 3020668471,
        3272380065, 1510334235, 755167117
    ];

    private static readonly Random Random = new();

    /// <summary>
    /// 生成 x-s-common 签名参数
    /// </summary>
    public static XhsSignResult Sign(string a1, string b1, string xS, string xT)
    {
        var common = new Dictionary<string, object>
        {
            ["s0"] = 3,            // getPlatformCode
            ["s1"] = "",
            ["x0"] = "1",          // localStorage.getItem("b1b1")
            ["x1"] = "3.6.8",      // version
            ["x2"] = "Mac OS",
            ["x3"] = "xhs-pc-web",
            ["x4"] = "4.20.1",
            ["x5"] = a1,           // cookie a1
            ["x6"] = xT,
            ["x7"] = xS,
            ["x8"] = b1,           // localStorage.getItem("b1")
            ["x9"] = Mrc(xT + xS + b1),
            ["x10"] = 1            // getSigCount
        };

        var jsonStr = JsonSerializer.Serialize(common, new JsonSerializerOptions
        {
            WriteIndented = false
        });
        var encodeBytes = EncodeUtf8(jsonStr);
        var xSCommon = B64Encode(encodeBytes);
        var xB3TraceId = GetB3TraceId();

        return new XhsSignResult
        {
            XS = xS,
            XT = xT,
            XSCommon = xSCommon,
            XB3TraceId = xB3TraceId
        };
    }

    /// <summary>
    /// 生成随机的 B3 Trace ID (16 位十六进制字符)
    /// </summary>
    public static string GetB3TraceId()
    {
        const string chars = "abcdef0123456789";
        var sb = new StringBuilder(16);
        for (var i = 0; i < 16; i++)
        {
            sb.Append(chars[Random.Next(chars.Length)]);
        }
        return sb.ToString();
    }

    /// <summary>
    /// 计算 MRC (CRC32 变体)
    /// </summary>
    public static long Mrc(string e)
    {
        uint o = 0xFFFFFFFF;

        for (var n = 0; n < 57 && n < e.Length; n++)
        {
            var index = (int)((o & 0xFF) ^ (byte)e[n]);
            o = Crc32Table[index] ^ (o >> 8);
        }

        // o ^ -1 ^ 3988292384
        // 在 C# 中: o ^ 0xFFFFFFFF ^ 3988292384
        var result = o ^ 0xFFFFFFFF ^ 3988292384;
        return (int)result;
    }

    /// <summary>
    /// 将字符串编码为 UTF-8 字节数组 (参考 encodeUtf8)
    /// </summary>
    public static byte[] EncodeUtf8(string text)
    {
        // URL 编码后解析为字节
        var encoded = HttpUtility.UrlEncode(text, Encoding.UTF8)
            .Replace("+", "%20"); // 空格处理

        var bytes = new List<byte>();
        var i = 0;
        while (i < encoded.Length)
        {
            if (encoded[i] == '%' && i + 2 < encoded.Length)
            {
                var hex = encoded.Substring(i + 1, 2);
                bytes.Add(Convert.ToByte(hex, 16));
                i += 3;
            }
            else
            {
                bytes.Add((byte)encoded[i]);
                i++;
            }
        }
        return bytes.ToArray();
    }

    /// <summary>
    /// 自定义 Base64 编码
    /// </summary>
    public static string B64Encode(byte[] data)
    {
        var len = data.Length;
        var remainder = len % 3;
        var result = new StringBuilder();
        const int chunkSize = 16383;
        var mainLen = len - remainder;

        // 处理主体部分 (3 字节一组)
        for (var i = 0; i < mainLen; i += chunkSize)
        {
            var end = Math.Min(i + chunkSize, mainLen);
            result.Append(EncodeChunk(data, i, end));
        }

        // 处理余数
        if (remainder == 1)
        {
            var f = data[len - 1];
            result.Append(Lookup[f >> 2]);
            result.Append(Lookup[(f << 4) & 63]);
            result.Append("==");
        }
        else if (remainder == 2)
        {
            var f = (data[len - 2] << 8) + data[len - 1];
            result.Append(Lookup[f >> 10]);
            result.Append(Lookup[(f >> 4) & 63]);
            result.Append(Lookup[(f << 2) & 63]);
            result.Append('=');
        }

        return result.ToString();
    }

    private static string EncodeChunk(byte[] data, int start, int end)
    {
        var sb = new StringBuilder();
        for (var i = start; i < end; i += 3)
        {
            if (i + 2 < data.Length)
            {
                var n = ((data[i] & 0xFF) << 16) |
                       ((data[i + 1] & 0xFF) << 8) |
                       (data[i + 2] & 0xFF);
                sb.Append(TripletToBase64(n));
            }
        }
        return sb.ToString();
    }

    private static string TripletToBase64(int triplet)
    {
        return $"{Lookup[(triplet >> 18) & 63]}{Lookup[(triplet >> 12) & 63]}{Lookup[(triplet >> 6) & 63]}{Lookup[triplet & 63]}";
    }

    /// <summary>
    /// 从 cookie 字符串中提取 a1 值
    /// </summary>
    public static string GetA1FromCookies(string cookies)
    {
        if (string.IsNullOrEmpty(cookies)) return "";

        foreach (var part in cookies.Split(';'))
        {
            var trimmed = part.Trim();
            if (trimmed.StartsWith("a1=", StringComparison.OrdinalIgnoreCase))
            {
                return trimmed[3..];
            }
        }
        return "";
    }
}

/// <summary>
/// XHS 签名结果
/// </summary>
public class XhsSignResult
{
    public string XS { get; set; } = "";
    public string XT { get; set; } = "";
    public string XSCommon { get; set; } = "";
    public string XB3TraceId { get; set; } = "";
}
