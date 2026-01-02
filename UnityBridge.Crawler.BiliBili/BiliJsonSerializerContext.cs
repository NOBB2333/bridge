using System.Text.Json.Serialization;
using UnityBridge.Crawler.BiliBili.Models;

namespace UnityBridge.Crawler.BiliBili;

/// <summary>
/// B站 JSON 序列化上下文（AOT 源生成器）。
/// </summary>
[JsonSerializable(typeof(BiliVideoSearchRequest))]
[JsonSerializable(typeof(BiliVideoSearchResponse))]
[JsonSerializable(typeof(BiliVideoSearchData))]
[JsonSerializable(typeof(BiliVideoSearchItem))]
[JsonSerializable(typeof(BiliVideoDetailRequest))]
[JsonSerializable(typeof(BiliVideoDetailResponse))]
[JsonSerializable(typeof(BiliVideoDetailData))]
[JsonSerializable(typeof(BiliVideo))]
[JsonSerializable(typeof(BiliVideoStat))]
[JsonSerializable(typeof(BiliVideoOwner))]
[JsonSerializable(typeof(BiliHomeFeedRequest))]
[JsonSerializable(typeof(BiliHomeFeedResponse))]
[JsonSerializable(typeof(BiliHomeFeedData))]
[JsonSerializable(typeof(BiliHomeFeedItem))]
[JsonSerializable(typeof(BiliCommentRequest))]
[JsonSerializable(typeof(BiliCommentResponse))]
[JsonSerializable(typeof(BiliCommentData))]
[JsonSerializable(typeof(BiliCommentCursor))]
[JsonSerializable(typeof(BiliSubCommentRequest))]
[JsonSerializable(typeof(BiliSubCommentResponse))]
[JsonSerializable(typeof(BiliSubCommentData))]
[JsonSerializable(typeof(BiliCommentPage))]
[JsonSerializable(typeof(BiliComment))]
[JsonSerializable(typeof(BiliCommentContent))]
[JsonSerializable(typeof(BiliCommentMember))]
[JsonSerializable(typeof(BiliUpInfoRequest))]
[JsonSerializable(typeof(BiliUpVideosRequest))]
[JsonSerializable(typeof(BiliUpVideosResponse))]
[JsonSerializable(typeof(BiliUpVideosData))]
[JsonSerializable(typeof(BiliUpVideosList))]
[JsonSerializable(typeof(BiliUpVideoItem))]
[JsonSerializable(typeof(BiliUpVideosPage))]
[JsonSerializable(typeof(BiliCreator))]
[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    WriteIndented = false)]
public partial class BiliJsonSerializerContext : JsonSerializerContext
{
}
