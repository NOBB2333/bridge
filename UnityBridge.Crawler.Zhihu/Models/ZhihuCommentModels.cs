namespace UnityBridge.Crawler.Zhihu.Models;

#region 评论请求/响应

/// <summary>评论列表请求。</summary>
public class ZhihuCommentRequest : ZhihuRequest
{
    public string ContentId { get; set; } = string.Empty;
    public string ContentType { get; set; } = "answer"; // answer, article, zvideo
    public string Offset { get; set; } = string.Empty;
    public int Limit { get; set; } = 10;
    public string OrderBy { get; set; } = "score";
}

/// <summary>评论列表响应。</summary>
public class ZhihuCommentResponse : ZhihuResponse
{
    [JsonPropertyName("data")]
    public List<ZhihuComment>? Data { get; set; }

    [JsonPropertyName("paging")]
    public ZhihuPaging? Paging { get; set; }
}

/// <summary>子评论请求。</summary>
public class ZhihuSubCommentRequest : ZhihuRequest
{
    public string RootCommentId { get; set; } = string.Empty;
    public string Offset { get; set; } = string.Empty;
    public int Limit { get; set; } = 10;
    public string OrderBy { get; set; } = "sort";
}

/// <summary>子评论响应。</summary>
public class ZhihuSubCommentResponse : ZhihuResponse
{
    [JsonPropertyName("data")]
    public List<ZhihuComment>? Data { get; set; }

    [JsonPropertyName("paging")]
    public ZhihuPaging? Paging { get; set; }
}

#endregion

#region 评论实体 (API + SQLSugar)

/// <summary>知乎评论（API 响应 + SQLSugar 实体）。</summary>
[SugarTable("zhihu_comments")]
public class ZhihuComment
{
    [SugarColumn(IsPrimaryKey = true, ColumnName = "comment_id")]
    [JsonPropertyName("id")]
    public string CommentId { get; set; } = string.Empty;

    [SugarColumn(ColumnName = "content_id")]
    public string? ContentId { get; set; }

    [SugarColumn(ColumnName = "content_type")]
    public string? ContentType { get; set; }

    [SugarColumn(ColumnName = "parent_comment_id")]
    public string? ParentCommentId { get; set; }

    [JsonPropertyName("content")]
    [SugarColumn(ColumnDataType = "text")]
    public string? Content { get; set; }

    [JsonPropertyName("created_time")]
    [SugarColumn(ColumnName = "publish_time")]
    public long? PublishTime { get; set; }

    [JsonPropertyName("ip_info")]
    [SugarColumn(ColumnName = "ip_location")]
    public string? IpLocation { get; set; }

    [JsonPropertyName("child_comment_count")]
    [SugarColumn(ColumnName = "sub_comment_count")]
    public int? SubCommentCount { get; set; }

    [JsonPropertyName("like_count")]
    [SugarColumn(ColumnName = "like_count")]
    public int? LikeCount { get; set; }

    [JsonPropertyName("dislike_count")]
    [SugarColumn(ColumnName = "dislike_count")]
    public int? DislikeCount { get; set; }

    [SugarColumn(IsIgnore = true)]
    [JsonPropertyName("author")]
    public ZhihuCommentAuthor? Author { get; set; }

    [SugarColumn(ColumnName = "user_id")]
    public string? UserId { get; set; }

    [SugarColumn(ColumnName = "user_nickname")]
    public string? UserNickname { get; set; }

    [SugarColumn(ColumnName = "user_avatar")]
    public string? UserAvatar { get; set; }

    [SugarColumn(ColumnName = "crawled_at")]
    public DateTimeOffset? CrawledAt { get; set; }
}

public class ZhihuCommentAuthor
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("avatar_url")]
    public string? AvatarUrl { get; set; }
}

#endregion
