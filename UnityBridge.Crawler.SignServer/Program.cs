using System.Text.Json.Serialization;
using UnityBridge.Crawler.Core.SignService;

var builder = WebApplication.CreateSlimBuilder(args);

// 配置 JSON 序列化
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, SignServerJsonContext.Default);
});

var app = builder.Build();

// 共享的签名客户端
using var signClient = new LocalSignClient();

// 健康检查
app.MapGet("/", () => Results.Ok(new PongResponse("Healthy")));
app.MapGet("/signsrv/pong", () => Results.Ok(new PongResponse("pong")));

// XHS 签名
app.MapPost("/signsrv/v1/xhs/sign", async (XhsSignApiRequest req) =>
{
    var result = await signClient.GetXhsSignAsync(new XhsSignRequest
    {
        Uri = req.Uri ?? "",
        Data = req.Data,
        Cookies = req.Cookies ?? ""
    });

    return Results.Ok(new ApiResponse<XhsSignApiResponse>(0, "success", new XhsSignApiResponse
    {
        XS = result.XS,
        XT = result.XT,
        XSCommon = result.XSCommon,
        XB3TraceId = result.XB3TraceId
    }));
});

// Bilibili 签名
app.MapPost("/signsrv/v1/bilibili/sign", async (BiliSignApiRequest req) =>
{
    var result = await signClient.GetBilibiliSignAsync(new BilibiliSignRequest
    {
        ReqData = req.ReqData ?? new Dictionary<string, string>(),
        Cookies = req.Cookies ?? ""
    });

    return Results.Ok(new ApiResponse<BiliSignApiResponse>(0, "success", new BiliSignApiResponse
    {
        Wts = result.Wts,
        WRid = result.WRid
    }));
});

// Douyin 签名
app.MapPost("/signsrv/v1/douyin/sign", async (DouyinSignApiRequest req) =>
{
    var result = await signClient.GetDouyinSignAsync(new DouyinSignRequest
    {
        Uri = req.Uri ?? "",
        Cookies = req.Cookies ?? ""
    });

    return Results.Ok(new ApiResponse<DouyinSignApiResponse>(0, "success", new DouyinSignApiResponse
    {
        ABogus = result.ABogus
    }));
});

// Zhihu 签名
app.MapPost("/signsrv/v1/zhihu/sign", async (ZhihuSignApiRequest req) =>
{
    var result = await signClient.GetZhihuSignAsync(new ZhihuSignRequest
    {
        Uri = req.Uri ?? "",
        Cookies = req.Cookies ?? ""
    });

    return Results.Ok(new ApiResponse<ZhihuSignApiResponse>(0, "success", new ZhihuSignApiResponse
    {
        XZse96 = result.XZse96,
        XZst81 = result.XZst81
    }));
});

Console.WriteLine("SignServer running on http://0.0.0.0:8888");
app.Run("http://0.0.0.0:8888");

#region API Models

record PongResponse(string Message);

record ApiResponse<T>(int Code, string Message, T Data);

// XHS
record XhsSignApiRequest(
    [property: JsonPropertyName("uri")] string? Uri,
    [property: JsonPropertyName("data")] string? Data,
    [property: JsonPropertyName("cookies")] string? Cookies
);

record XhsSignApiResponse
{
    [JsonPropertyName("x-s")] public string XS { get; init; } = "";
    [JsonPropertyName("x-t")] public string XT { get; init; } = "";
    [JsonPropertyName("x-s-common")] public string XSCommon { get; init; } = "";
    [JsonPropertyName("x-b3-traceid")] public string XB3TraceId { get; init; } = "";
}

// Bilibili
record BiliSignApiRequest(
    [property: JsonPropertyName("req_data")] Dictionary<string, string>? ReqData,
    [property: JsonPropertyName("cookies")] string? Cookies
);

record BiliSignApiResponse
{
    [JsonPropertyName("wts")] public string Wts { get; init; } = "";
    [JsonPropertyName("w_rid")] public string WRid { get; init; } = "";
}

// Douyin
record DouyinSignApiRequest(
    [property: JsonPropertyName("uri")] string? Uri,
    [property: JsonPropertyName("cookies")] string? Cookies
);

record DouyinSignApiResponse
{
    [JsonPropertyName("a-bogus")] public string ABogus { get; init; } = "";
}

// Zhihu
record ZhihuSignApiRequest(
    [property: JsonPropertyName("uri")] string? Uri,
    [property: JsonPropertyName("cookies")] string? Cookies
);

record ZhihuSignApiResponse
{
    [JsonPropertyName("x-zse-96")] public string XZse96 { get; init; } = "";
    [JsonPropertyName("x-zst-81")] public string XZst81 { get; init; } = "";
}

#endregion

// AOT JSON 序列化上下文
[JsonSerializable(typeof(PongResponse))]
[JsonSerializable(typeof(ApiResponse<XhsSignApiResponse>))]
[JsonSerializable(typeof(ApiResponse<BiliSignApiResponse>))]
[JsonSerializable(typeof(ApiResponse<DouyinSignApiResponse>))]
[JsonSerializable(typeof(ApiResponse<ZhihuSignApiResponse>))]
[JsonSerializable(typeof(XhsSignApiRequest))]
[JsonSerializable(typeof(BiliSignApiRequest))]
[JsonSerializable(typeof(DouyinSignApiRequest))]
[JsonSerializable(typeof(ZhihuSignApiRequest))]
[JsonSerializable(typeof(Dictionary<string, string>))]
internal partial class SignServerJsonContext : JsonSerializerContext
{
}
