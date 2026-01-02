namespace UnityBridge.Crawler.Tieba.Models;

#region 评论请求

/// <summary>评论列表请求。</summary>
public class TiebaCommentRequest : TiebaRequest
{
    public string PostId { get; set; } = string.Empty;
    public int Page { get; set; } = 1;
}

/// <summary>子评论请求。</summary>
public class TiebaSubCommentRequest : TiebaRequest
{
    public string PostId { get; set; } = string.Empty;
    public string ParentCommentId { get; set; } = string.Empty;
    public string TiebaId { get; set; } = string.Empty;
    public int Page { get; set; } = 1;
}

#endregion

#region 评论实体 (SQLSugar)

/// <summary>百度贴吧评论（SQLSugar 实体）。</summary>
[SugarTable("tieba_comments")]
public class TiebaComment
{
    [SugarColumn(IsPrimaryKey = true, ColumnName = "comment_id")]
    public string CommentId { get; set; } = string.Empty;

    [SugarColumn(ColumnName = "post_id")]
    public string? PostId { get; set; }

    [SugarColumn(ColumnName = "parent_comment_id")]
    public string? ParentCommentId { get; set; }

    [SugarColumn(ColumnDataType = "text")]
    public string? Content { get; set; }

    [SugarColumn(ColumnName = "user_link")]
    public string? UserLink { get; set; }

    [SugarColumn(ColumnName = "user_nickname")]
    public string? UserNickname { get; set; }

    [SugarColumn(ColumnName = "user_avatar")]
    public string? UserAvatar { get; set; }

    [SugarColumn(ColumnName = "publish_time")]
    public string? PublishTime { get; set; }

    [SugarColumn(ColumnName = "ip_location")]
    public string? IpLocation { get; set; }

    [SugarColumn(ColumnName = "sub_comment_count")]
    public int? SubCommentCount { get; set; }

    [SugarColumn(ColumnName = "post_url")]
    public string? PostUrl { get; set; }

    [SugarColumn(ColumnName = "tieba_id")]
    public string? TiebaId { get; set; }

    [SugarColumn(ColumnName = "tieba_name")]
    public string? TiebaName { get; set; }

    [SugarColumn(ColumnName = "tieba_link")]
    public string? TiebaLink { get; set; }

    [SugarColumn(ColumnName = "crawled_at")]
    public DateTimeOffset? CrawledAt { get; set; }
}

#endregion
