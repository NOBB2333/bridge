using System.Text.Json.Serialization;
using UnityBridge.Crawler.Zhihu.Models;

namespace UnityBridge.Crawler.Zhihu;

/// <summary>
/// 知乎 JSON 序列化上下文（AOT 源生成器）。
/// </summary>
[JsonSerializable(typeof(ZhihuSearchRequest))]
[JsonSerializable(typeof(ZhihuSearchResponse))]
[JsonSerializable(typeof(ZhihuSearchItem))]
[JsonSerializable(typeof(ZhihuAnswerDetailRequest))]
[JsonSerializable(typeof(ZhihuArticleDetailRequest))]
[JsonSerializable(typeof(ZhihuVideoDetailRequest))]
[JsonSerializable(typeof(ZhihuContent))]
[JsonSerializable(typeof(ZhihuAuthor))]
[JsonSerializable(typeof(ZhihuQuestionAnswersRequest))]
[JsonSerializable(typeof(ZhihuQuestionAnswersResponse))]
[JsonSerializable(typeof(ZhihuAnswerItem))]
[JsonSerializable(typeof(ZhihuCommentRequest))]
[JsonSerializable(typeof(ZhihuCommentResponse))]
[JsonSerializable(typeof(ZhihuSubCommentRequest))]
[JsonSerializable(typeof(ZhihuSubCommentResponse))]
[JsonSerializable(typeof(ZhihuComment))]
[JsonSerializable(typeof(ZhihuCommentAuthor))]
[JsonSerializable(typeof(ZhihuCreatorContentRequest))]
[JsonSerializable(typeof(ZhihuCreatorContentResponse))]
[JsonSerializable(typeof(ZhihuCreator))]
[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    WriteIndented = false)]
public partial class ZhihuJsonSerializerContext : JsonSerializerContext
{
}
