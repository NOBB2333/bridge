namespace UnityBridge.Crawler.Zhihu.Models;

#region 搜索请求/响应

/// <summary>内容搜索请求。</summary>
public class ZhihuSearchRequest : ZhihuRequest
{
    public string Keyword { get; set; } = string.Empty;
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string Sort { get; set; } = "default"; // default, upvoted_count
    public string Vertical { get; set; } = ""; // answer, article, zvideo
    public string TimeInterval { get; set; } = ""; // a_day, a_week, a_month, a_year
}

/// <summary>内容搜索响应。</summary>
public class ZhihuSearchResponse : ZhihuResponse
{
    [JsonPropertyName("data")]
    public List<ZhihuSearchItem>? Data { get; set; }

    [JsonPropertyName("paging")]
    public ZhihuPaging? Paging { get; set; }
}

public class ZhihuSearchItem
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("object")]
    public ZhihuContent? Object { get; set; }
}

#endregion

#region 内容详情请求

/// <summary>回答详情请求。</summary>
public class ZhihuAnswerDetailRequest : ZhihuRequest
{
    public string QuestionId { get; set; } = string.Empty;
    public string AnswerId { get; set; } = string.Empty;
}

/// <summary>文章详情请求。</summary>
public class ZhihuArticleDetailRequest : ZhihuRequest
{
    public string ArticleId { get; set; } = string.Empty;
}

/// <summary>视频详情请求。</summary>
public class ZhihuVideoDetailRequest : ZhihuRequest
{
    public string VideoId { get; set; } = string.Empty;
}

#endregion

#region 内容实体 (API + SQLSugar)

/// <summary>知乎内容（回答/文章/视频）（API 响应 + SQLSugar 实体）。</summary>
[SugarTable("zhihu_contents")]
public class ZhihuContent
{
    [SugarColumn(IsPrimaryKey = true, ColumnName = "content_id")]
    [JsonPropertyName("id")]
    public string ContentId { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    [SugarColumn(ColumnName = "content_type")]
    public string? ContentType { get; set; } // answer, article, zvideo

    [SugarColumn(ColumnName = "question_id")]
    public string? QuestionId { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("excerpt")]
    [SugarColumn(ColumnDataType = "text")]
    public string? Description { get; set; }

    [SugarColumn(IsIgnore = true)]
    [JsonPropertyName("content")]
    public string? ContentHtml { get; set; }

    [SugarColumn(ColumnDataType = "longtext", ColumnName = "content_text")]
    public string? ContentText { get; set; }

    [SugarColumn(ColumnName = "content_url")]
    public string? ContentUrl { get; set; }

    [JsonPropertyName("created_time")]
    [SugarColumn(ColumnName = "created_time")]
    public long? CreatedTime { get; set; }

    [JsonPropertyName("updated_time")]
    [SugarColumn(ColumnName = "updated_time")]
    public long? UpdatedTime { get; set; }

    [JsonPropertyName("voteup_count")]
    [SugarColumn(ColumnName = "voteup_count")]
    public int? VoteupCount { get; set; }

    [JsonPropertyName("comment_count")]
    [SugarColumn(ColumnName = "comment_count")]
    public int? CommentCount { get; set; }

    [SugarColumn(IsIgnore = true)]
    [JsonPropertyName("author")]
    public ZhihuAuthor? Author { get; set; }

    [SugarColumn(ColumnName = "user_id")]
    public string? UserId { get; set; }

    [SugarColumn(ColumnName = "user_nickname")]
    public string? UserNickname { get; set; }

    [SugarColumn(ColumnName = "user_avatar")]
    public string? UserAvatar { get; set; }

    [SugarColumn(ColumnName = "user_url_token")]
    public string? UserUrlToken { get; set; }

    // 爬虫扩展字段
    [SugarColumn(ColumnName = "keyword")]
    public string? Keyword { get; set; }

    [SugarColumn(ColumnName = "crawled_at")]
    public DateTimeOffset? CrawledAt { get; set; }
}

public class ZhihuAuthor
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("url_token")]
    public string? UrlToken { get; set; }

    [JsonPropertyName("avatar_url")]
    public string? AvatarUrl { get; set; }
}

#endregion

#region 问题回答请求

/// <summary>问题回答列表请求。</summary>
public class ZhihuQuestionAnswersRequest : ZhihuRequest
{
    public string QuestionId { get; set; } = string.Empty;
    public string Cursor { get; set; } = string.Empty;
    public int Limit { get; set; } = 5;
    public int Offset { get; set; } = 0;
    public string Order { get; set; } = "default"; // default, updated
}

/// <summary>问题回答列表响应。</summary>
public class ZhihuQuestionAnswersResponse : ZhihuPagedResponse<ZhihuAnswerItem>
{
}

public class ZhihuAnswerItem
{
    [JsonPropertyName("target")]
    public ZhihuContent? Target { get; set; }
}

#endregion
