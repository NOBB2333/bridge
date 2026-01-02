namespace UnityBridge.Crawler.Zhihu.Models;

#region 创作者请求/响应

/// <summary>创作者内容列表请求。</summary>
public class ZhihuCreatorContentRequest : ZhihuRequest
{
    public string UrlToken { get; set; } = string.Empty;
    public int Offset { get; set; } = 0;
    public int Limit { get; set; } = 20;
}

/// <summary>创作者内容列表响应。</summary>
public class ZhihuCreatorContentResponse : ZhihuPagedResponse<ZhihuContent>
{
}

#endregion

#region 创作者实体 (API + SQLSugar)

/// <summary>知乎创作者（API 响应 + SQLSugar 实体）。</summary>
[SugarTable("zhihu_creators")]
public class ZhihuCreator
{
    [SugarColumn(IsPrimaryKey = true, ColumnName = "user_id")]
    [JsonPropertyName("id")]
    public string UserId { get; set; } = string.Empty;

    [JsonPropertyName("url_token")]
    [SugarColumn(ColumnName = "url_token")]
    public string? UrlToken { get; set; }

    [JsonPropertyName("name")]
    public string? Nickname { get; set; }

    [SugarColumn(ColumnName = "user_link")]
    public string? UserLink { get; set; }

    [JsonPropertyName("avatar_url")]
    [SugarColumn(ColumnName = "avatar")]
    public string? Avatar { get; set; }

    [JsonPropertyName("gender")]
    public int? Gender { get; set; }

    [JsonPropertyName("ip_info")]
    [SugarColumn(ColumnName = "ip_location")]
    public string? IpLocation { get; set; }

    [JsonPropertyName("following_count")]
    [SugarColumn(ColumnName = "following_count")]
    public int? FollowingCount { get; set; }

    [JsonPropertyName("follower_count")]
    [SugarColumn(ColumnName = "follower_count")]
    public int? FollowerCount { get; set; }

    [JsonPropertyName("answer_count")]
    [SugarColumn(ColumnName = "answer_count")]
    public int? AnswerCount { get; set; }

    [JsonPropertyName("articles_count")]
    [SugarColumn(ColumnName = "article_count")]
    public int? ArticleCount { get; set; }

    [JsonPropertyName("zvideo_count")]
    [SugarColumn(ColumnName = "video_count")]
    public int? VideoCount { get; set; }

    [JsonPropertyName("question_count")]
    [SugarColumn(ColumnName = "question_count")]
    public int? QuestionCount { get; set; }

    [JsonPropertyName("voteup_count")]
    [SugarColumn(ColumnName = "voteup_count")]
    public int? VoteupCount { get; set; }

    [SugarColumn(ColumnName = "crawled_at")]
    public DateTimeOffset? CrawledAt { get; set; }
}

#endregion
