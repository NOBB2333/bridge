namespace UnityBridge.Crawler.Tieba.Models;

#region 搜索请求

/// <summary>帖子搜索请求。</summary>
public class TiebaSearchRequest : TiebaRequest
{
    public string Keyword { get; set; } = string.Empty;
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public int SortType { get; set; } = 1; // 0=相关性, 1=时间倒序
    public int OnlyThread { get; set; } = 1; // 0=混合, 1=仅主题帖
}

/// <summary>贴吧帖子列表请求。</summary>
public class TiebaForumRequest : TiebaRequest
{
    public string TiebaName { get; set; } = string.Empty;
    public int PageNum { get; set; } = 0;
}

/// <summary>帖子详情请求。</summary>
public class TiebaPostDetailRequest : TiebaRequest
{
    public string PostId { get; set; } = string.Empty;
}

#endregion

#region 帖子实体 (SQLSugar)

/// <summary>百度贴吧帖子（SQLSugar 实体）。</summary>
[SugarTable("tieba_posts")]
public class TiebaPost
{
    [SugarColumn(IsPrimaryKey = true, ColumnName = "post_id")]
    public string PostId { get; set; } = string.Empty;

    public string? Title { get; set; }

    [SugarColumn(ColumnDataType = "text")]
    public string? Description { get; set; }

    [SugarColumn(ColumnName = "post_url")]
    public string? PostUrl { get; set; }

    [SugarColumn(ColumnName = "publish_time")]
    public string? PublishTime { get; set; }

    [SugarColumn(ColumnName = "user_link")]
    public string? UserLink { get; set; }

    [SugarColumn(ColumnName = "user_nickname")]
    public string? UserNickname { get; set; }

    [SugarColumn(ColumnName = "user_avatar")]
    public string? UserAvatar { get; set; }

    [SugarColumn(ColumnName = "tieba_name")]
    public string? TiebaName { get; set; }

    [SugarColumn(ColumnName = "tieba_link")]
    public string? TiebaLink { get; set; }

    [SugarColumn(ColumnName = "reply_count")]
    public int? ReplyCount { get; set; }

    [SugarColumn(ColumnName = "reply_page_count")]
    public int? ReplyPageCount { get; set; }

    [SugarColumn(ColumnName = "ip_location")]
    public string? IpLocation { get; set; }

    // 爬虫扩展字段
    [SugarColumn(ColumnName = "keyword")]
    public string? Keyword { get; set; }

    [SugarColumn(ColumnName = "crawled_at")]
    public DateTimeOffset? CrawledAt { get; set; }
}

#endregion
