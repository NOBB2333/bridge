namespace UnityBridge.Crawler.Weibo.Models;

#region 评论请求/响应

/// <summary>评论列表请求。</summary>
public class WeiboCommentRequest : WeiboRequest
{
    public string NoteId { get; set; } = string.Empty;
    public long MaxId { get; set; } = 0;
    public int MaxIdType { get; set; } = 0;
}

/// <summary>评论列表响应。</summary>
public class WeiboCommentResponse : WeiboResponse
{
    [JsonPropertyName("data")]
    public WeiboCommentData? Data { get; set; }
}

public class WeiboCommentData
{
    [JsonPropertyName("data")]
    public List<WeiboComment>? Comments { get; set; }

    [JsonPropertyName("max_id")]
    public long MaxId { get; set; }

    [JsonPropertyName("max_id_type")]
    public int MaxIdType { get; set; }
}

#endregion

#region 评论实体 (API + SQLSugar)

/// <summary>微博评论（API 响应 + SQLSugar 实体）。</summary>
[SugarTable("weibo_comments")]
public class WeiboComment
{
    [SugarColumn(IsPrimaryKey = true, ColumnName = "comment_id")]
    [JsonPropertyName("id")]
    public string CommentId { get; set; } = string.Empty;

    [SugarColumn(ColumnName = "note_id")]
    public string? NoteId { get; set; }

    [SugarColumn(ColumnName = "parent_comment_id")]
    public string? ParentCommentId { get; set; }

    [JsonPropertyName("text")]
    [SugarColumn(ColumnDataType = "text")]
    public string? Content { get; set; }

    [JsonPropertyName("created_at")]
    [SugarColumn(ColumnName = "create_time")]
    public string? CreateTime { get; set; }

    [JsonPropertyName("total_number")]
    [SugarColumn(ColumnName = "sub_comment_count")]
    public int? SubCommentCount { get; set; }

    [JsonPropertyName("like_count")]
    [SugarColumn(ColumnName = "like_count")]
    public int? LikeCount { get; set; }

    [JsonPropertyName("source")]
    [SugarColumn(ColumnName = "ip_location")]
    public string? IpLocation { get; set; }

    [SugarColumn(IsIgnore = true)]
    [JsonPropertyName("user")]
    public WeiboUser? User { get; set; }

    [SugarColumn(ColumnName = "user_id")]
    public string? UserId { get; set; }

    [SugarColumn(ColumnName = "nickname")]
    public string? Nickname { get; set; }

    [SugarColumn(ColumnName = "avatar")]
    public string? Avatar { get; set; }

    [SugarColumn(ColumnName = "crawled_at")]
    public DateTimeOffset? CrawledAt { get; set; }
}

#endregion
