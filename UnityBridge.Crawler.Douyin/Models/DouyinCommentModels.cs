namespace UnityBridge.Crawler.Douyin.Models;

#region 评论请求/响应

/// <summary>评论列表请求。</summary>
public class DouyinCommentRequest : DouyinRequest
{
    public string AwemeId { get; set; } = string.Empty;
    public int Cursor { get; set; } = 0;
    public int Count { get; set; } = 20;
}

/// <summary>评论列表响应。</summary>
public class DouyinCommentResponse : DouyinResponse
{
    [JsonPropertyName("comments")]
    public List<DouyinComment>? Comments { get; set; }

    [JsonPropertyName("has_more")]
    public int HasMore { get; set; }

    [JsonPropertyName("cursor")]
    public long Cursor { get; set; }

    [JsonPropertyName("total")]
    public long Total { get; set; }
}

/// <summary>子评论请求。</summary>
public class DouyinSubCommentRequest : DouyinRequest
{
    public string CommentId { get; set; } = string.Empty;
    public string AwemeId { get; set; } = string.Empty;
    public int Cursor { get; set; } = 0;
    public int Count { get; set; } = 20;
}

/// <summary>子评论响应。</summary>
public class DouyinSubCommentResponse : DouyinResponse
{
    [JsonPropertyName("comments")]
    public List<DouyinComment>? Comments { get; set; }

    [JsonPropertyName("has_more")]
    public int HasMore { get; set; }

    [JsonPropertyName("cursor")]
    public long Cursor { get; set; }
}

#endregion

#region 评论实体 (API + SQLSugar)

/// <summary>抖音评论（API 响应 + SQLSugar 实体）。</summary>
[SugarTable("douyin_comments")]
public class DouyinComment
{
    [SugarColumn(IsPrimaryKey = true, ColumnName = "cid")]
    [JsonPropertyName("cid")]
    public string Cid { get; set; } = string.Empty;

    [SugarColumn(ColumnName = "aweme_id")]
    public string? AwemeId { get; set; }

    [SugarColumn(ColumnName = "reply_id")]
    [JsonPropertyName("reply_id")]
    public string? ReplyId { get; set; }

    [JsonPropertyName("text")]
    [SugarColumn(ColumnDataType = "text")]
    public string? Content { get; set; }

    [JsonPropertyName("create_time")]
    [SugarColumn(ColumnName = "create_time")]
    public long? CreateTime { get; set; }

    [JsonPropertyName("digg_count")]
    [SugarColumn(ColumnName = "digg_count")]
    public long? DiggCount { get; set; }

    [JsonPropertyName("reply_comment_total")]
    [SugarColumn(ColumnName = "reply_count")]
    public int? ReplyCount { get; set; }

    [SugarColumn(IsIgnore = true)]
    [JsonPropertyName("user")]
    public DouyinCommentUser? User { get; set; }

    [SugarColumn(ColumnName = "user_id")]
    public string? UserId { get; set; }

    [SugarColumn(ColumnName = "sec_uid")]
    public string? SecUid { get; set; }

    [SugarColumn(ColumnName = "nickname")]
    public string? Nickname { get; set; }

    [SugarColumn(ColumnName = "avatar")]
    public string? Avatar { get; set; }

    [JsonPropertyName("ip_label")]
    [SugarColumn(ColumnName = "ip_location")]
    public string? IpLocation { get; set; }

    [SugarColumn(ColumnName = "crawled_at")]
    public DateTimeOffset? CrawledAt { get; set; }
}

public class DouyinCommentUser
{
    [JsonPropertyName("uid")]
    public string? Uid { get; set; }

    [JsonPropertyName("sec_uid")]
    public string? SecUid { get; set; }

    [JsonPropertyName("nickname")]
    public string? Nickname { get; set; }

    [JsonPropertyName("avatar_thumb")]
    public DouyinImageUrl? AvatarThumb { get; set; }
}

#endregion
