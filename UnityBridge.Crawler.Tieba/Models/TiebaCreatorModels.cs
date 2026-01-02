namespace UnityBridge.Crawler.Tieba.Models;

#region 创作者请求

/// <summary>创作者帖子请求。</summary>
public class TiebaCreatorPostsRequest : TiebaRequest
{
    public string UserName { get; set; } = string.Empty;
    public int PageNum { get; set; } = 1;
}

/// <summary>创作者帖子响应。</summary>
public class TiebaCreatorPostsResponse : TiebaResponse
{
    [JsonPropertyName("data")]
    public TiebaCreatorPostsData? Data { get; set; }
}

public class TiebaCreatorPostsData
{
    [JsonPropertyName("thread_list")]
    public List<TiebaCreatorPostItem>? ThreadList { get; set; }

    [JsonPropertyName("has_more")]
    public int HasMore { get; set; }
}

public class TiebaCreatorPostItem
{
    [JsonPropertyName("thread_id")]
    public string? ThreadId { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("fname")]
    public string? ForumName { get; set; }

    [JsonPropertyName("create_time")]
    public string? CreateTime { get; set; }

    [JsonPropertyName("reply_num")]
    public int ReplyNum { get; set; }
}

#endregion

#region 创作者实体 (SQLSugar)

/// <summary>百度贴吧创作者（SQLSugar 实体）。</summary>
[SugarTable("tieba_creators")]
public class TiebaCreator
{
    [SugarColumn(IsPrimaryKey = true, ColumnName = "user_id")]
    public string UserId { get; set; } = string.Empty;

    [SugarColumn(ColumnName = "user_name")]
    public string? UserName { get; set; }

    public string? Nickname { get; set; }

    public string? Gender { get; set; }

    public string? Avatar { get; set; }

    [SugarColumn(ColumnName = "ip_location")]
    public string? IpLocation { get; set; }

    [SugarColumn(ColumnName = "follow_count")]
    public int? FollowCount { get; set; }

    [SugarColumn(ColumnName = "fans_count")]
    public int? FansCount { get; set; }

    [SugarColumn(ColumnName = "registration_duration")]
    public string? RegistrationDuration { get; set; }

    [SugarColumn(ColumnName = "crawled_at")]
    public DateTimeOffset? CrawledAt { get; set; }
}

#endregion
