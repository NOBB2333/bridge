namespace UnityBridge.Crawler.BiliBili.Models;

#region 评论请求/响应

/// <summary>评论列表请求。</summary>
public class BiliCommentRequest : BiliRequest
{
    public string VideoId { get; set; } = string.Empty; // oid (aid)
    public int Mode { get; set; } = 3; // 3=热门, 2=时间
    public int Next { get; set; } = 0;
}

/// <summary>评论列表响应。</summary>
public class BiliCommentResponse : BiliResponse
{
    [JsonPropertyName("data")]
    public BiliCommentData? Data { get; set; }
}

public class BiliCommentData
{
    [JsonPropertyName("replies")]
    public List<BiliComment>? Replies { get; set; }

    [JsonPropertyName("cursor")]
    public BiliCommentCursor? Cursor { get; set; }
}

public class BiliCommentCursor
{
    [JsonPropertyName("is_end")]
    public bool IsEnd { get; set; }

    [JsonPropertyName("next")]
    public int Next { get; set; }
}

/// <summary>子评论请求。</summary>
public class BiliSubCommentRequest : BiliRequest
{
    public string VideoId { get; set; } = string.Empty;
    public string RootCommentId { get; set; } = string.Empty;
    public int Pn { get; set; } = 1;
    public int Ps { get; set; } = 20;
    public int Mode { get; set; } = 3;
}

/// <summary>子评论响应。</summary>
public class BiliSubCommentResponse : BiliResponse
{
    [JsonPropertyName("data")]
    public BiliSubCommentData? Data { get; set; }
}

public class BiliSubCommentData
{
    [JsonPropertyName("replies")]
    public List<BiliComment>? Replies { get; set; }

    [JsonPropertyName("page")]
    public BiliCommentPage? Page { get; set; }
}

public class BiliCommentPage
{
    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("num")]
    public int Num { get; set; }

    [JsonPropertyName("size")]
    public int Size { get; set; }
}

#endregion

#region 评论实体 (API + SQLSugar)

/// <summary>B站评论（API 响应 + SQLSugar 实体）。</summary>
[SugarTable("bili_comments")]
public class BiliComment
{
    [SugarColumn(IsPrimaryKey = true, ColumnName = "rpid")]
    [JsonPropertyName("rpid")]
    public long Rpid { get; set; }

    [SugarColumn(ColumnName = "video_id")]
    public string? VideoId { get; set; }

    [SugarColumn(ColumnName = "parent_rpid")]
    public long? ParentRpid { get; set; }

    [SugarColumn(IsIgnore = true)]
    [JsonPropertyName("content")]
    public BiliCommentContent? ContentObj { get; set; }

    [SugarColumn(ColumnDataType = "text")]
    public string? Content { get; set; }

    [JsonPropertyName("ctime")]
    [SugarColumn(ColumnName = "create_time")]
    public long? CreateTime { get; set; }

    [JsonPropertyName("like")]
    [SugarColumn(ColumnName = "like_count")]
    public long? LikeCount { get; set; }

    [JsonPropertyName("rcount")]
    [SugarColumn(ColumnName = "reply_count")]
    public int? ReplyCount { get; set; }

    [SugarColumn(IsIgnore = true)]
    [JsonPropertyName("member")]
    public BiliCommentMember? Member { get; set; }

    [SugarColumn(ColumnName = "user_id")]
    public long? UserId { get; set; }

    [SugarColumn(ColumnName = "nickname")]
    public string? Nickname { get; set; }

    [SugarColumn(ColumnName = "avatar")]
    public string? Avatar { get; set; }

    [SugarColumn(IsIgnore = true)]
    [JsonPropertyName("replies")]
    public List<BiliComment>? Replies { get; set; }

    [SugarColumn(ColumnName = "crawled_at")]
    public DateTimeOffset? CrawledAt { get; set; }
}

public class BiliCommentContent
{
    [JsonPropertyName("message")]
    public string? Message { get; set; }
}

public class BiliCommentMember
{
    [JsonPropertyName("mid")]
    public string? Mid { get; set; }

    [JsonPropertyName("uname")]
    public string? Uname { get; set; }

    [JsonPropertyName("avatar")]
    public string? Avatar { get; set; }
}

#endregion
