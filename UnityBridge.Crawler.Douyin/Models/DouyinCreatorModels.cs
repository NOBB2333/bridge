namespace UnityBridge.Crawler.Douyin.Models;

#region 用户请求/响应

/// <summary>用户信息请求。</summary>
public class DouyinUserProfileRequest : DouyinRequest
{
    public string SecUserId { get; set; } = string.Empty;
}

/// <summary>用户信息响应。</summary>
public class DouyinUserProfileResponse : DouyinResponse
{
    [JsonPropertyName("user")]
    public DouyinCreator? User { get; set; }
}

/// <summary>用户视频列表请求。</summary>
public class DouyinUserPostsRequest : DouyinRequest
{
    public string SecUserId { get; set; } = string.Empty;
    public string MaxCursor { get; set; } = "0";
    public int Count { get; set; } = 18;
}

/// <summary>用户视频列表响应。</summary>
public class DouyinUserPostsResponse : DouyinResponse
{
    [JsonPropertyName("aweme_list")]
    public List<DouyinAweme>? AwemeList { get; set; }

    [JsonPropertyName("has_more")]
    public int HasMore { get; set; }

    [JsonPropertyName("max_cursor")]
    public long MaxCursor { get; set; }
}

#endregion

#region 创作者实体 (API + SQLSugar)

/// <summary>抖音创作者（API 响应 + SQLSugar 实体）。</summary>
[SugarTable("douyin_creators")]
public class DouyinCreator
{
    [SugarColumn(IsPrimaryKey = true, ColumnName = "uid")]
    [JsonPropertyName("uid")]
    public string Uid { get; set; } = string.Empty;

    [JsonPropertyName("sec_uid")]
    [SugarColumn(ColumnName = "sec_uid")]
    public string? SecUid { get; set; }

    [JsonPropertyName("unique_id")]
    [SugarColumn(ColumnName = "unique_id")]
    public string? UniqueId { get; set; }

    [JsonPropertyName("short_id")]
    [SugarColumn(ColumnName = "short_id")]
    public string? ShortId { get; set; }

    [JsonPropertyName("nickname")]
    public string? Nickname { get; set; }

    [SugarColumn(IsIgnore = true)]
    [JsonPropertyName("avatar_thumb")]
    public DouyinImageUrl? AvatarThumb { get; set; }

    [SugarColumn(ColumnName = "avatar")]
    public string? Avatar { get; set; }

    [JsonPropertyName("signature")]
    public string? Description { get; set; }

    [JsonPropertyName("gender")]
    public int? Gender { get; set; }

    [JsonPropertyName("ip_location")]
    [SugarColumn(ColumnName = "ip_location")]
    public string? IpLocation { get; set; }

    [JsonPropertyName("follower_count")]
    [SugarColumn(ColumnName = "follower_count")]
    public long? FollowerCount { get; set; }

    [JsonPropertyName("following_count")]
    [SugarColumn(ColumnName = "following_count")]
    public long? FollowingCount { get; set; }

    [JsonPropertyName("total_favorited")]
    [SugarColumn(ColumnName = "total_favorited")]
    public long? TotalFavorited { get; set; }

    [JsonPropertyName("aweme_count")]
    [SugarColumn(ColumnName = "aweme_count")]
    public int? AwemeCount { get; set; }

    [SugarColumn(ColumnName = "crawled_at")]
    public DateTimeOffset? CrawledAt { get; set; }
}

#endregion
