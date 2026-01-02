namespace UnityBridge.Crawler.Weibo.Models;

#region 搜索请求/响应

/// <summary>微博搜索请求。</summary>
public class WeiboSearchRequest : WeiboRequest
{
    public string Keyword { get; set; } = string.Empty;
    public int Page { get; set; } = 1;
    public int SearchType { get; set; } = 1; // 1=综合, 61=实时, 62=热门, 63=视频
}

/// <summary>微博搜索响应。</summary>
public class WeiboSearchResponse : WeiboResponse
{
    [JsonPropertyName("data")]
    public WeiboSearchData? Data { get; set; }
}

public class WeiboSearchData
{
    [JsonPropertyName("cards")]
    public List<WeiboCard>? Cards { get; set; }
}

public class WeiboCard
{
    [JsonPropertyName("card_type")]
    public int CardType { get; set; }

    [JsonPropertyName("mblog")]
    public WeiboNote? Mblog { get; set; }

    [JsonPropertyName("card_group")]
    public List<WeiboCard>? CardGroup { get; set; }
}

#endregion

#region 笔记详情请求

/// <summary>微博详情请求。</summary>
public class WeiboNoteDetailRequest : WeiboRequest
{
    public string NoteId { get; set; } = string.Empty;
}

#endregion

#region 笔记实体 (API + SQLSugar)

/// <summary>微博笔记（API 响应 + SQLSugar 实体）。</summary>
[SugarTable("weibo_notes")]
public class WeiboNote
{
    [SugarColumn(IsPrimaryKey = true, ColumnName = "note_id")]
    [JsonPropertyName("id")]
    public string NoteId { get; set; } = string.Empty;

    [JsonPropertyName("text")]
    [SugarColumn(ColumnDataType = "text")]
    public string? Content { get; set; }

    [JsonPropertyName("created_at")]
    [SugarColumn(ColumnName = "create_time")]
    public string? CreateTime { get; set; }

    [JsonPropertyName("attitudes_count")]
    [SugarColumn(ColumnName = "like_count")]
    public int? LikeCount { get; set; }

    [JsonPropertyName("comments_count")]
    [SugarColumn(ColumnName = "comment_count")]
    public int? CommentCount { get; set; }

    [JsonPropertyName("reposts_count")]
    [SugarColumn(ColumnName = "repost_count")]
    public int? RepostCount { get; set; }

    [SugarColumn(ColumnName = "note_url")]
    public string? NoteUrl { get; set; }

    [JsonPropertyName("region_name")]
    [SugarColumn(ColumnName = "ip_location")]
    public string? IpLocation { get; set; }

    [SugarColumn(IsIgnore = true)]
    [JsonPropertyName("pics")]
    public List<WeiboPic>? Pics { get; set; }

    [SugarColumn(ColumnDataType = "text", ColumnName = "image_list")]
    public string? ImageList { get; set; }

    [SugarColumn(ColumnName = "video_url")]
    public string? VideoUrl { get; set; }

    [SugarColumn(IsIgnore = true)]
    [JsonPropertyName("user")]
    public WeiboUser? User { get; set; }

    [SugarColumn(ColumnName = "user_id")]
    public string? UserId { get; set; }

    [SugarColumn(ColumnName = "nickname")]
    public string? Nickname { get; set; }

    [SugarColumn(ColumnName = "avatar")]
    public string? Avatar { get; set; }

    // 爬虫扩展字段
    [SugarColumn(ColumnName = "keyword")]
    public string? Keyword { get; set; }

    [SugarColumn(ColumnName = "crawled_at")]
    public DateTimeOffset? CrawledAt { get; set; }
}

public class WeiboPic
{
    [JsonPropertyName("url")]
    public string? Url { get; set; }
}

public class WeiboUser
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("screen_name")]
    public string? ScreenName { get; set; }

    [JsonPropertyName("profile_image_url")]
    public string? ProfileImageUrl { get; set; }

    [JsonPropertyName("gender")]
    public string? Gender { get; set; }
}

#endregion
