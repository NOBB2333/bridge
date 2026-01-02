namespace UnityBridge.Crawler.Kuaishou.Models;

#region 创作者请求/响应

/// <summary>创作者信息请求。</summary>
public class KuaishouCreatorProfileRequest : KuaishouRequest
{
    public string UserId { get; set; } = string.Empty;
}

/// <summary>创作者视频列表请求。</summary>
public class KuaishouCreatorVideosRequest : KuaishouRequest
{
    public string UserId { get; set; } = string.Empty;
    public string Pcursor { get; set; } = string.Empty;
}

/// <summary>创作者信息响应。</summary>
public class KuaishouCreatorProfileResponse : KuaishouResponse
{
    [JsonPropertyName("data")]
    public KuaishouCreatorProfileData? Data { get; set; }
}

public class KuaishouCreatorProfileData
{
    [JsonPropertyName("visionProfile")]
    public KuaishouVisionProfile? VisionProfile { get; set; }
}

public class KuaishouVisionProfile
{
    [JsonPropertyName("userProfile")]
    public KuaishouCreator? UserProfile { get; set; }
}

/// <summary>创作者视频响应。</summary>
public class KuaishouCreatorVideosResponse : KuaishouResponse
{
    [JsonPropertyName("data")]
    public KuaishouCreatorVideosData? Data { get; set; }
}

public class KuaishouCreatorVideosData
{
    [JsonPropertyName("visionProfilePhotoList")]
    public KuaishouVisionProfilePhotoList? VisionProfilePhotoList { get; set; }
}

public class KuaishouVisionProfilePhotoList
{
    [JsonPropertyName("feeds")]
    public List<KuaishouFeed>? Feeds { get; set; }

    [JsonPropertyName("pcursor")]
    public string? Pcursor { get; set; }
}

#endregion

#region 创作者实体 (API + SQLSugar)

/// <summary>快手创作者（API 响应 + SQLSugar 实体）。</summary>
[SugarTable("kuaishou_creators")]
public class KuaishouCreator
{
    [SugarColumn(IsPrimaryKey = true, ColumnName = "user_id")]
    [JsonPropertyName("id")]
    public string UserId { get; set; } = string.Empty;

    [JsonPropertyName("profile")]
    [SugarColumn(IsIgnore = true)]
    public KuaishouProfile? Profile { get; set; }

    public string? Nickname { get; set; }

    public string? Avatar { get; set; }

    public string? Gender { get; set; }

    public string? Description { get; set; }

    [SugarColumn(ColumnName = "ip_location")]
    public string? IpLocation { get; set; }

    [SugarColumn(ColumnName = "follower_count")]
    public long? FollowerCount { get; set; }

    [SugarColumn(ColumnName = "following_count")]
    public long? FollowingCount { get; set; }

    [SugarColumn(ColumnName = "photo_count")]
    public int? PhotoCount { get; set; }

    [SugarColumn(ColumnName = "crawled_at")]
    public DateTimeOffset? CrawledAt { get; set; }
}

public class KuaishouProfile
{
    [JsonPropertyName("user_name")]
    public string? UserName { get; set; }

    [JsonPropertyName("headurl")]
    public string? HeadUrl { get; set; }

    [JsonPropertyName("user_text")]
    public string? UserText { get; set; }

    [JsonPropertyName("gender")]
    public string? Gender { get; set; }
}

#endregion
