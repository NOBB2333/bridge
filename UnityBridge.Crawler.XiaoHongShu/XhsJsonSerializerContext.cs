using System.Text.Json.Serialization;
using UnityBridge.Crawler.XiaoHongShu.Models;

namespace UnityBridge.Crawler.XiaoHongShu;

/// <summary>
/// 小红书 JSON 序列化上下文（AOT 源生成器）。
/// </summary>
[JsonSerializable(typeof(XhsNoteSearchRequest))]
[JsonSerializable(typeof(XhsNoteSearchResponse))]
[JsonSerializable(typeof(XhsNoteSearchData))]
[JsonSerializable(typeof(XhsNoteSearchItem))]
[JsonSerializable(typeof(XhsNoteCard))]
[JsonSerializable(typeof(XhsUserInfo))]
[JsonSerializable(typeof(XhsImageInfo))]
[JsonSerializable(typeof(XhsVideoInfo))]
[JsonSerializable(typeof(XhsInteractInfo))]
[JsonSerializable(typeof(XhsNoteDetailRequest))]
[JsonSerializable(typeof(XhsNoteDetailResponse))]
[JsonSerializable(typeof(XhsNoteDetailData))]
[JsonSerializable(typeof(XhsNoteDetailItem))]
[JsonSerializable(typeof(XhsNoteDetailExtra))]
[JsonSerializable(typeof(XhsHomeFeedRequest))]
[JsonSerializable(typeof(XhsHomeFeedResponse))]
[JsonSerializable(typeof(XhsHomeFeedData))]
[JsonSerializable(typeof(XhsCommentPageRequest))]
[JsonSerializable(typeof(XhsCommentPageResponse))]
[JsonSerializable(typeof(XhsCommentPageData))]
[JsonSerializable(typeof(XhsSubCommentRequest))]
[JsonSerializable(typeof(XhsSubCommentResponse))]
[JsonSerializable(typeof(XhsSubCommentData))]
[JsonSerializable(typeof(XhsComment))]
[JsonSerializable(typeof(XhsCommentUserInfo))]
[JsonSerializable(typeof(XhsCreatorNotesRequest))]
[JsonSerializable(typeof(XhsCreatorNotesResponse))]
[JsonSerializable(typeof(XhsCreatorNotesData))]
[JsonSerializable(typeof(XhsCreator))]
[JsonSerializable(typeof(XhsInteractionItem))]
[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    WriteIndented = false)]
public partial class XhsJsonSerializerContext : JsonSerializerContext
{
}
