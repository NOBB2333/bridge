using System.Text.Json.Serialization;
using UnityBridge.Crawler.Tieba.Models;

namespace UnityBridge.Crawler.Tieba;

/// <summary>
/// 百度贴吧 JSON 序列化上下文（AOT 源生成器）。
/// </summary>
[JsonSerializable(typeof(TiebaSearchRequest))]
[JsonSerializable(typeof(TiebaForumRequest))]
[JsonSerializable(typeof(TiebaPostDetailRequest))]
[JsonSerializable(typeof(TiebaPost))]
[JsonSerializable(typeof(TiebaCommentRequest))]
[JsonSerializable(typeof(TiebaSubCommentRequest))]
[JsonSerializable(typeof(TiebaComment))]
[JsonSerializable(typeof(TiebaCreatorPostsRequest))]
[JsonSerializable(typeof(TiebaCreatorPostsResponse))]
[JsonSerializable(typeof(TiebaCreatorPostsData))]
[JsonSerializable(typeof(TiebaCreatorPostItem))]
[JsonSerializable(typeof(TiebaCreator))]
[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    WriteIndented = false)]
public partial class TiebaJsonSerializerContext : JsonSerializerContext
{
}
