namespace UnityBridge.Crawler.XiaoHongShu.Models;

#region 搜索笔记

/// <summary>搜索笔记请求。</summary>
public class XhsNoteSearchRequest : XhsRequest
{
    [JsonPropertyName("keyword")]
    public string Keyword { get; set; } = string.Empty;

    [JsonPropertyName("page")]
    public int Page { get; set; } = 1;

    [JsonPropertyName("page_size")]
    public int PageSize { get; set; } = 20;

    [JsonPropertyName("sort")]
    public string Sort { get; set; } = "general";

    [JsonPropertyName("note_type")]
    public int NoteType { get; set; } = 0;

    [JsonPropertyName("search_id")]
    public string SearchId { get; set; } = Guid.NewGuid().ToString("N");
}

/// <summary>搜索笔记响应。</summary>
public class XhsNoteSearchResponse : XhsResponse
{
    [JsonPropertyName("data")]
    public XhsNoteSearchData? Data { get; set; }
}

public class XhsNoteSearchData
{
    [JsonPropertyName("items")]
    public List<XhsNoteSearchItem>? Items { get; set; }

    [JsonPropertyName("has_more")]
    public bool HasMore { get; set; }

    [JsonPropertyName("cursor")]
    public string? Cursor { get; set; }
}

public class XhsNoteSearchItem
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("model_type")]
    public string? ModelType { get; set; }

    [JsonPropertyName("note_card")]
    public XhsNoteCard? NoteCard { get; set; }

    [JsonPropertyName("xsec_token")]
    public string? XsecToken { get; set; }
}

#endregion

#region 笔记实体 (API + SQLSugar)

/// <summary>笔记详情（API 响应 + SQLSugar 实体）。</summary>
[SugarTable("xhs_notes")]
public class XhsNoteCard
{
    [SugarColumn(IsPrimaryKey = true, ColumnName = "note_id")]
    [JsonPropertyName("note_id")]
    public string NoteId { get; set; } = string.Empty;

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("desc")]
    public string? Description { get; set; }

    [JsonPropertyName("type")]
    public string? NoteType { get; set; }

    [SugarColumn(IsIgnore = true)]
    [JsonPropertyName("user")]
    public XhsUserInfo? User { get; set; }

    [SugarColumn(ColumnName = "user_id")]
    public string? UserId { get; set; }

    [SugarColumn(ColumnName = "user_nickname")]
    public string? UserNickname { get; set; }

    [SugarColumn(IsIgnore = true)]
    [JsonPropertyName("image_list")]
    public List<XhsImageInfo>? ImageList { get; set; }

    [SugarColumn(ColumnDataType = "text")]
    public string? ImageUrls { get; set; }

    [SugarColumn(IsIgnore = true)]
    [JsonPropertyName("video")]
    public XhsVideoInfo? Video { get; set; }

    [SugarColumn(ColumnName = "video_url")]
    public string? VideoUrl { get; set; }

    [SugarColumn(IsIgnore = true)]
    [JsonPropertyName("interact_info")]
    public XhsInteractInfo? InteractInfo { get; set; }

    [SugarColumn(ColumnName = "liked_count")]
    public long? LikedCount { get; set; }

    [SugarColumn(ColumnName = "collected_count")]
    public long? CollectedCount { get; set; }

    [SugarColumn(ColumnName = "comment_count")]
    public long? CommentCount { get; set; }

    [SugarColumn(ColumnName = "share_count")]
    public long? ShareCount { get; set; }

    [JsonPropertyName("time")]
    [SugarColumn(ColumnName = "publish_time")]
    public long? PublishTime { get; set; }

    [JsonPropertyName("last_update_time")]
    [SugarColumn(ColumnName = "update_time")]
    public long? UpdateTime { get; set; }

    [SugarColumn(ColumnName = "xsec_token")]
    public string? XsecToken { get; set; }

    [SugarColumn(ColumnName = "xsec_source")]
    public string? XsecSource { get; set; }

    [SugarColumn(ColumnName = "note_url")]
    public string? NoteUrl { get; set; }

    // 爬虫扩展字段
    [SugarColumn(ColumnName = "keyword")]
    public string? Keyword { get; set; }

    [SugarColumn(ColumnName = "crawled_at")]
    public DateTimeOffset? CrawledAt { get; set; }
}

public class XhsUserInfo
{
    [JsonPropertyName("user_id")]
    public string? UserId { get; set; }

    [JsonPropertyName("nickname")]
    public string? Nickname { get; set; }

    [JsonPropertyName("avatar")]
    public string? Avatar { get; set; }
}

public class XhsImageInfo
{
    [JsonPropertyName("url_default")]
    public string? Url { get; set; }

    [JsonPropertyName("width")]
    public int? Width { get; set; }

    [JsonPropertyName("height")]
    public int? Height { get; set; }
}

public class XhsVideoInfo
{
    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("duration")]
    public int? Duration { get; set; }
}

public class XhsInteractInfo
{
    [JsonPropertyName("liked_count")]
    public string? LikedCount { get; set; }

    [JsonPropertyName("collected_count")]
    public string? CollectedCount { get; set; }

    [JsonPropertyName("comment_count")]
    public string? CommentCount { get; set; }

    [JsonPropertyName("share_count")]
    public string? ShareCount { get; set; }
}

#endregion
