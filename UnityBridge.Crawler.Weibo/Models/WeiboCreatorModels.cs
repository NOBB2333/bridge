namespace UnityBridge.Crawler.Weibo.Models;

#region 创作者请求/响应

/// <summary>创作者信息请求。</summary>
public class WeiboCreatorProfileRequest : WeiboRequest
{
    public string CreatorId { get; set; } = string.Empty;
}

/// <summary>创作者微博列表请求。</summary>
public class WeiboCreatorNotesRequest : WeiboRequest
{
    public string CreatorId { get; set; } = string.Empty;
    public string ContainerId { get; set; } = string.Empty;
    public string SinceId { get; set; } = "0";
}

/// <summary>创作者信息响应。</summary>
public class WeiboCreatorProfileResponse : WeiboResponse
{
    [JsonPropertyName("data")]
    public WeiboCreatorProfileData? Data { get; set; }
}

public class WeiboCreatorProfileData
{
    [JsonPropertyName("userInfo")]
    public WeiboCreator? UserInfo { get; set; }

    [JsonPropertyName("tabsInfo")]
    public WeiboTabsInfo? TabsInfo { get; set; }
}

public class WeiboTabsInfo
{
    [JsonPropertyName("tabs")]
    public List<WeiboTab>? Tabs { get; set; }
}

public class WeiboTab
{
    [JsonPropertyName("tabKey")]
    public string? TabKey { get; set; }

    [JsonPropertyName("containerid")]
    public string? ContainerId { get; set; }
}

/// <summary>创作者微博列表响应。</summary>
public class WeiboCreatorNotesResponse : WeiboResponse
{
    [JsonPropertyName("data")]
    public WeiboCreatorNotesData? Data { get; set; }
}

public class WeiboCreatorNotesData
{
    [JsonPropertyName("cards")]
    public List<WeiboCard>? Cards { get; set; }

    [JsonPropertyName("since_id")]
    public string? SinceId { get; set; }
}

#endregion

#region 创作者实体 (API + SQLSugar)

/// <summary>微博创作者（API 响应 + SQLSugar 实体）。</summary>
[SugarTable("weibo_creators")]
public class WeiboCreator
{
    [SugarColumn(IsPrimaryKey = true, ColumnName = "user_id")]
    [JsonPropertyName("id")]
    public long UserId { get; set; }

    [JsonPropertyName("screen_name")]
    public string? Nickname { get; set; }

    [JsonPropertyName("gender")]
    public string? Gender { get; set; }

    [JsonPropertyName("profile_image_url")]
    [SugarColumn(ColumnName = "avatar")]
    public string? Avatar { get; set; }

    [JsonPropertyName("description")]
    [SugarColumn(ColumnDataType = "text")]
    public string? Description { get; set; }

    [JsonPropertyName("follow_count")]
    [SugarColumn(ColumnName = "following_count")]
    public int? FollowingCount { get; set; }

    [JsonPropertyName("followers_count")]
    [SugarColumn(ColumnName = "follower_count")]
    public int? FollowerCount { get; set; }

    [JsonPropertyName("statuses_count")]
    [SugarColumn(ColumnName = "note_count")]
    public int? NoteCount { get; set; }

    [SugarColumn(ColumnName = "crawled_at")]
    public DateTimeOffset? CrawledAt { get; set; }
}

#endregion
