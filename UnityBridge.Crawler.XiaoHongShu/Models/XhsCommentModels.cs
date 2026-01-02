namespace UnityBridge.Crawler.XiaoHongShu.Models;

#region 评论请求/响应

/// <summary>评论列表请求。</summary>
public class XhsCommentPageRequest : XhsRequest
{
    public string NoteId { get; set; } = string.Empty;
    public string Cursor { get; set; } = string.Empty;
    public string? XsecToken { get; set; }
}

/// <summary>评论列表响应。</summary>
public class XhsCommentPageResponse : XhsResponse
{
    [JsonPropertyName("data")]
    public XhsCommentPageData? Data { get; set; }
}

public class XhsCommentPageData
{
    [JsonPropertyName("comments")]
    public List<XhsComment>? Comments { get; set; }

    [JsonPropertyName("cursor")]
    public string? Cursor { get; set; }

    [JsonPropertyName("has_more")]
    public bool HasMore { get; set; }
}

/// <summary>子评论请求。</summary>
public class XhsSubCommentRequest : XhsRequest
{
    public string NoteId { get; set; } = string.Empty;
    public string RootCommentId { get; set; } = string.Empty;
    public int Num { get; set; } = 10;
    public string Cursor { get; set; } = string.Empty;
    public string? XsecToken { get; set; }
}

/// <summary>子评论响应。</summary>
public class XhsSubCommentResponse : XhsResponse
{
    [JsonPropertyName("data")]
    public XhsSubCommentData? Data { get; set; }
}

public class XhsSubCommentData
{
    [JsonPropertyName("comments")]
    public List<XhsComment>? Comments { get; set; }

    [JsonPropertyName("cursor")]
    public string? Cursor { get; set; }

    [JsonPropertyName("has_more")]
    public bool HasMore { get; set; }
}

#endregion

#region 评论实体 (API + SQLSugar)

/// <summary>评论（API 响应 + SQLSugar 实体）。</summary>
[SugarTable("xhs_comments")]
public class XhsComment
{
    [SugarColumn(IsPrimaryKey = true, ColumnName = "comment_id")]
    [JsonPropertyName("id")]
    public string CommentId { get; set; } = string.Empty;

    [SugarColumn(ColumnName = "note_id")]
    public string? NoteId { get; set; }

    [SugarColumn(ColumnName = "parent_comment_id")]
    public string? ParentCommentId { get; set; }

    [JsonPropertyName("content")]
    public string? Content { get; set; }

    [SugarColumn(IsIgnore = true)]
    [JsonPropertyName("user_info")]
    public XhsCommentUserInfo? UserInfo { get; set; }

    [SugarColumn(ColumnName = "user_id")]
    public string? UserId { get; set; }

    [SugarColumn(ColumnName = "user_nickname")]
    public string? UserNickname { get; set; }

    [JsonPropertyName("create_time")]
    [SugarColumn(ColumnName = "create_time")]
    public long? CreateTime { get; set; }

    [JsonPropertyName("like_count")]
    [SugarColumn(ColumnName = "like_count")]
    public string? LikeCount { get; set; }

    [JsonPropertyName("sub_comment_count")]
    [SugarColumn(ColumnName = "sub_comment_count")]
    public string? SubCommentCount { get; set; }

    [SugarColumn(IsIgnore = true)]
    [JsonPropertyName("sub_comments")]
    public List<XhsComment>? SubComments { get; set; }

    [SugarColumn(ColumnName = "xsec_token")]
    public string? XsecToken { get; set; }

    [SugarColumn(ColumnName = "crawled_at")]
    public DateTimeOffset? CrawledAt { get; set; }
}

public class XhsCommentUserInfo
{
    [JsonPropertyName("user_id")]
    public string? UserId { get; set; }

    [JsonPropertyName("nickname")]
    public string? Nickname { get; set; }

    [JsonPropertyName("image")]
    public string? Avatar { get; set; }
}

#endregion
