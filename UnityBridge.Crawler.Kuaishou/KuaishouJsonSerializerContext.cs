using System.Text.Json.Serialization;
using UnityBridge.Crawler.Kuaishou.Models;

namespace UnityBridge.Crawler.Kuaishou;

/// <summary>
/// 快手 JSON 序列化上下文（AOT 源生成器）。
/// </summary>
[JsonSerializable(typeof(KuaishouSearchRequest))]
[JsonSerializable(typeof(KuaishouSearchResponse))]
[JsonSerializable(typeof(KuaishouSearchData))]
[JsonSerializable(typeof(KuaishouVisionSearchPhoto))]
[JsonSerializable(typeof(KuaishouFeed))]
[JsonSerializable(typeof(KuaishouVideoDetailRequest))]
[JsonSerializable(typeof(KuaishouVideoDetailResponse))]
[JsonSerializable(typeof(KuaishouVideoDetailData))]
[JsonSerializable(typeof(KuaishouVideo))]
[JsonSerializable(typeof(KuaishouAuthor))]
[JsonSerializable(typeof(KuaishouHomeFeedRequest))]
[JsonSerializable(typeof(KuaishouHomeFeedResponse))]
[JsonSerializable(typeof(KuaishouHomeFeedData))]
[JsonSerializable(typeof(KuaishouBrilliantTypeData))]
[JsonSerializable(typeof(KuaishouCommentRequest))]
[JsonSerializable(typeof(KuaishouCommentResponse))]
[JsonSerializable(typeof(KuaishouCommentData))]
[JsonSerializable(typeof(KuaishouVisionCommentList))]
[JsonSerializable(typeof(KuaishouSubCommentRequest))]
[JsonSerializable(typeof(KuaishouSubCommentResponse))]
[JsonSerializable(typeof(KuaishouSubCommentData))]
[JsonSerializable(typeof(KuaishouVisionSubCommentList))]
[JsonSerializable(typeof(KuaishouComment))]
[JsonSerializable(typeof(KuaishouCreatorProfileRequest))]
[JsonSerializable(typeof(KuaishouCreatorVideosRequest))]
[JsonSerializable(typeof(KuaishouCreatorProfileResponse))]
[JsonSerializable(typeof(KuaishouCreatorProfileData))]
[JsonSerializable(typeof(KuaishouVisionProfile))]
[JsonSerializable(typeof(KuaishouCreatorVideosResponse))]
[JsonSerializable(typeof(KuaishouCreatorVideosData))]
[JsonSerializable(typeof(KuaishouVisionProfilePhotoList))]
[JsonSerializable(typeof(KuaishouCreator))]
[JsonSerializable(typeof(KuaishouProfile))]
[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    WriteIndented = false)]
public partial class KuaishouJsonSerializerContext : JsonSerializerContext
{
}
