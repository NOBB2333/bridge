namespace UnityBridge.Crawler.Kuaishou.Models;

#region 视频搜索

/// <summary>视频搜索请求。</summary>
public class KuaishouSearchRequest : KuaishouRequest
{
    public string Keyword { get; set; } = string.Empty;
    public string Pcursor { get; set; } = string.Empty;
    public string SearchSessionId { get; set; } = string.Empty;
}

/// <summary>视频搜索响应。</summary>
public class KuaishouSearchResponse : KuaishouResponse
{
    [JsonPropertyName("data")]
    public KuaishouSearchData? Data { get; set; }
}

public class KuaishouSearchData
{
    [JsonPropertyName("visionSearchPhoto")]
    public KuaishouVisionSearchPhoto? VisionSearchPhoto { get; set; }
}

public class KuaishouVisionSearchPhoto
{
    [JsonPropertyName("feeds")]
    public List<KuaishouFeed>? Feeds { get; set; }

    [JsonPropertyName("pcursor")]
    public string? Pcursor { get; set; }

    [JsonPropertyName("searchSessionId")]
    public string? SearchSessionId { get; set; }
}

public class KuaishouFeed
{
    [JsonPropertyName("photo")]
    public KuaishouVideo? Photo { get; set; }
}

#endregion

#region 视频详情请求

/// <summary>视频详情请求。</summary>
public class KuaishouVideoDetailRequest : KuaishouRequest
{
    public string PhotoId { get; set; } = string.Empty;
}

/// <summary>视频详情响应。</summary>
public class KuaishouVideoDetailResponse : KuaishouResponse
{
    [JsonPropertyName("data")]
    public KuaishouVideoDetailData? Data { get; set; }
}

public class KuaishouVideoDetailData
{
    [JsonPropertyName("visionVideoDetail")]
    public KuaishouVideo? VisionVideoDetail { get; set; }
}

#endregion

#region 视频实体 (API + SQLSugar)

/// <summary>快手视频（API 响应 + SQLSugar 实体）。</summary>
[SugarTable("kuaishou_videos")]
public class KuaishouVideo
{
    [SugarColumn(IsPrimaryKey = true, ColumnName = "photo_id")]
    [JsonPropertyName("id")]
    public string PhotoId { get; set; } = string.Empty;

    [JsonPropertyName("caption")]
    public string? Title { get; set; }

    [SugarColumn(ColumnDataType = "text")]
    public string? Description { get; set; }

    [JsonPropertyName("timestamp")]
    [SugarColumn(ColumnName = "create_time")]
    public long? CreateTime { get; set; }

    [JsonPropertyName("likeCount")]
    [SugarColumn(ColumnName = "like_count")]
    public long? LikeCount { get; set; }

    [JsonPropertyName("viewCount")]
    [SugarColumn(ColumnName = "view_count")]
    public long? ViewCount { get; set; }

    [JsonPropertyName("commentCount")]
    [SugarColumn(ColumnName = "comment_count")]
    public long? CommentCount { get; set; }

    [SugarColumn(IsIgnore = true)]
    [JsonPropertyName("coverUrl")]
    public string? CoverUrlRaw { get; set; }

    [SugarColumn(ColumnName = "cover_url")]
    public string? CoverUrl { get; set; }

    [SugarColumn(IsIgnore = true)]
    [JsonPropertyName("author")]
    public KuaishouAuthor? Author { get; set; }

    [SugarColumn(ColumnName = "user_id")]
    public string? UserId { get; set; }

    [SugarColumn(ColumnName = "nickname")]
    public string? Nickname { get; set; }

    [SugarColumn(ColumnName = "avatar")]
    public string? Avatar { get; set; }

    [SugarColumn(ColumnName = "video_url")]
    public string? VideoUrl { get; set; }

    // 爬虫扩展字段
    [SugarColumn(ColumnName = "keyword")]
    public string? Keyword { get; set; }

    [SugarColumn(ColumnName = "crawled_at")]
    public DateTimeOffset? CrawledAt { get; set; }
}

public class KuaishouAuthor
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("headerUrl")]
    public string? HeaderUrl { get; set; }
}

#endregion

#region 首页推荐

/// <summary>首页推荐请求。</summary>
public class KuaishouHomeFeedRequest : KuaishouRequest
{
    public string Pcursor { get; set; } = string.Empty;
    public string HotChannelId { get; set; } = "00";
}

/// <summary>首页推荐响应。</summary>
public class KuaishouHomeFeedResponse : KuaishouResponse
{
    [JsonPropertyName("data")]
    public KuaishouHomeFeedData? Data { get; set; }
}

public class KuaishouHomeFeedData
{
    [JsonPropertyName("brilliantTypeData")]
    public KuaishouBrilliantTypeData? BrilliantTypeData { get; set; }
}

public class KuaishouBrilliantTypeData
{
    [JsonPropertyName("feeds")]
    public List<KuaishouFeed>? Feeds { get; set; }

    [JsonPropertyName("pcursor")]
    public string? Pcursor { get; set; }
}

#endregion
