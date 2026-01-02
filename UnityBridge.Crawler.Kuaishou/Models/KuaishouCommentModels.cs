namespace UnityBridge.Crawler.Kuaishou.Models;

#region 评论请求/响应

/// <summary>评论列表请求。</summary>
public class KuaishouCommentRequest : KuaishouRequest
{
    public string PhotoId { get; set; } = string.Empty;
    public string Pcursor { get; set; } = string.Empty;
}

/// <summary>评论列表响应。</summary>
public class KuaishouCommentResponse : KuaishouResponse
{
    [JsonPropertyName("data")]
    public KuaishouCommentData? Data { get; set; }
}

public class KuaishouCommentData
{
    [JsonPropertyName("visionCommentList")]
    public KuaishouVisionCommentList? VisionCommentList { get; set; }
}

public class KuaishouVisionCommentList
{
    [JsonPropertyName("rootComments")]
    public List<KuaishouComment>? RootComments { get; set; }

    [JsonPropertyName("pcursor")]
    public string? Pcursor { get; set; }
}

/// <summary>子评论请求。</summary>
public class KuaishouSubCommentRequest : KuaishouRequest
{
    public string PhotoId { get; set; } = string.Empty;
    public string RootCommentId { get; set; } = string.Empty;
    public string Pcursor { get; set; } = string.Empty;
}

/// <summary>子评论响应。</summary>
public class KuaishouSubCommentResponse : KuaishouResponse
{
    [JsonPropertyName("data")]
    public KuaishouSubCommentData? Data { get; set; }
}

public class KuaishouSubCommentData
{
    [JsonPropertyName("visionSubCommentList")]
    public KuaishouVisionSubCommentList? VisionSubCommentList { get; set; }
}

public class KuaishouVisionSubCommentList
{
    [JsonPropertyName("subComments")]
    public List<KuaishouComment>? SubComments { get; set; }

    [JsonPropertyName("pcursor")]
    public string? Pcursor { get; set; }
}

#endregion

#region 评论实体 (API + SQLSugar)

/// <summary>快手评论（API 响应 + SQLSugar 实体）。</summary>
[SugarTable("kuaishou_comments")]
public class KuaishouComment
{
    [SugarColumn(IsPrimaryKey = true, ColumnName = "comment_id")]
    [JsonPropertyName("commentId")]
    public string CommentId { get; set; } = string.Empty;

    [SugarColumn(ColumnName = "photo_id")]
    public string? PhotoId { get; set; }

    [SugarColumn(ColumnName = "parent_comment_id")]
    public string? ParentCommentId { get; set; }

    [JsonPropertyName("content")]
    [SugarColumn(ColumnDataType = "text")]
    public string? Content { get; set; }

    [JsonPropertyName("timestamp")]
    [SugarColumn(ColumnName = "create_time")]
    public long? CreateTime { get; set; }

    [JsonPropertyName("likedCount")]
    [SugarColumn(ColumnName = "like_count")]
    public long? LikeCount { get; set; }

    [JsonPropertyName("subCommentCount")]
    [SugarColumn(ColumnName = "sub_comment_count")]
    public int? SubCommentCount { get; set; }

    [SugarColumn(IsIgnore = true)]
    [JsonPropertyName("author")]
    public KuaishouAuthor? Author { get; set; }

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
