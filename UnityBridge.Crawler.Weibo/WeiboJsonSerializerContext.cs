using System.Text.Json.Serialization;
using UnityBridge.Crawler.Weibo.Models;

namespace UnityBridge.Crawler.Weibo;

/// <summary>
/// 微博 JSON 序列化上下文（AOT 源生成器）。
/// </summary>
[JsonSerializable(typeof(WeiboSearchRequest))]
[JsonSerializable(typeof(WeiboSearchResponse))]
[JsonSerializable(typeof(WeiboSearchData))]
[JsonSerializable(typeof(WeiboCard))]
[JsonSerializable(typeof(WeiboNoteDetailRequest))]
[JsonSerializable(typeof(WeiboNote))]
[JsonSerializable(typeof(WeiboPic))]
[JsonSerializable(typeof(WeiboUser))]
[JsonSerializable(typeof(WeiboCommentRequest))]
[JsonSerializable(typeof(WeiboCommentResponse))]
[JsonSerializable(typeof(WeiboCommentData))]
[JsonSerializable(typeof(WeiboComment))]
[JsonSerializable(typeof(WeiboCreatorProfileRequest))]
[JsonSerializable(typeof(WeiboCreatorNotesRequest))]
[JsonSerializable(typeof(WeiboCreatorProfileResponse))]
[JsonSerializable(typeof(WeiboCreatorProfileData))]
[JsonSerializable(typeof(WeiboTabsInfo))]
[JsonSerializable(typeof(WeiboTab))]
[JsonSerializable(typeof(WeiboCreatorNotesResponse))]
[JsonSerializable(typeof(WeiboCreatorNotesData))]
[JsonSerializable(typeof(WeiboCreator))]
[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    WriteIndented = false)]
public partial class WeiboJsonSerializerContext : JsonSerializerContext
{
}
