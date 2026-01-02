using System.Text.Json.Serialization;
using UnityBridge.Crawler.Douyin.Models;

namespace UnityBridge.Crawler.Douyin;

/// <summary>
/// 抖音 JSON 序列化上下文（AOT 源生成器）。
/// </summary>
[JsonSerializable(typeof(DouyinSearchRequest))]
[JsonSerializable(typeof(DouyinSearchResponse))]
[JsonSerializable(typeof(DouyinSearchItem))]
[JsonSerializable(typeof(DouyinAwemeDetailRequest))]
[JsonSerializable(typeof(DouyinAwemeDetailResponse))]
[JsonSerializable(typeof(DouyinAweme))]
[JsonSerializable(typeof(DouyinAwemeStatistics))]
[JsonSerializable(typeof(DouyinAuthor))]
[JsonSerializable(typeof(DouyinVideo))]
[JsonSerializable(typeof(DouyinImageUrl))]
[JsonSerializable(typeof(DouyinPlayAddr))]
[JsonSerializable(typeof(DouyinHomeFeedRequest))]
[JsonSerializable(typeof(DouyinHomeFeedResponse))]
[JsonSerializable(typeof(DouyinCommentRequest))]
[JsonSerializable(typeof(DouyinCommentResponse))]
[JsonSerializable(typeof(DouyinSubCommentRequest))]
[JsonSerializable(typeof(DouyinSubCommentResponse))]
[JsonSerializable(typeof(DouyinComment))]
[JsonSerializable(typeof(DouyinCommentUser))]
[JsonSerializable(typeof(DouyinUserProfileRequest))]
[JsonSerializable(typeof(DouyinUserProfileResponse))]
[JsonSerializable(typeof(DouyinUserPostsRequest))]
[JsonSerializable(typeof(DouyinUserPostsResponse))]
[JsonSerializable(typeof(DouyinCreator))]
[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    WriteIndented = false)]
public partial class DouyinJsonSerializerContext : JsonSerializerContext
{
}
