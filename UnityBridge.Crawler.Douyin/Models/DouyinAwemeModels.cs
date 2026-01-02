namespace UnityBridge.Crawler.Douyin.Models;

#region 视频搜索

/// <summary>视频搜索请求。</summary>
public class DouyinSearchRequest : DouyinRequest
{
    public string Keyword { get; set; } = string.Empty;
    public int Offset { get; set; } = 0;
    public int Count { get; set; } = 10;
    public string SearchChannel { get; set; } = "aweme_general"; // aweme_general, aweme_video_web
    public int SortType { get; set; } = 0; // 0=综合, 1=最多点赞, 2=最新发布
    public int PublishTime { get; set; } = 0; // 0=不限, 1=一天内, 7=一周内, 180=半年内
    public string SearchId { get; set; } = string.Empty;
}

/// <summary>视频搜索响应。</summary>
public class DouyinSearchResponse : DouyinResponse
{
    [JsonPropertyName("data")]
    public List<DouyinSearchItem>? Data { get; set; }

    [JsonPropertyName("has_more")]
    public int HasMore { get; set; }

    [JsonPropertyName("cursor")]
    public int Cursor { get; set; }
}

public class DouyinSearchItem
{
    [JsonPropertyName("aweme_info")]
    public DouyinAweme? AwemeInfo { get; set; }
}

#endregion

#region 视频详情请求

/// <summary>视频详情请求。</summary>
public class DouyinAwemeDetailRequest : DouyinRequest
{
    public string AwemeId { get; set; } = string.Empty;
}

/// <summary>视频详情响应。</summary>
public class DouyinAwemeDetailResponse : DouyinResponse
{
    [JsonPropertyName("aweme_detail")]
    public DouyinAweme? AwemeDetail { get; set; }
}

#endregion

#region 视频实体 (API + SQLSugar)

/// <summary>抖音视频（API 响应 + SQLSugar 实体）。</summary>
[SugarTable("douyin_awemes")]
public class DouyinAweme
{
    [SugarColumn(IsPrimaryKey = true, ColumnName = "aweme_id")]
    [JsonPropertyName("aweme_id")]
    public string AwemeId { get; set; } = string.Empty;

    [JsonPropertyName("aweme_type")]
    [SugarColumn(ColumnName = "aweme_type")]
    public int? AwemeType { get; set; }

    [JsonPropertyName("desc")]
    [SugarColumn(ColumnDataType = "text")]
    public string? Title { get; set; }

    [JsonPropertyName("create_time")]
    [SugarColumn(ColumnName = "create_time")]
    public long? CreateTime { get; set; }

    [SugarColumn(IsIgnore = true)]
    [JsonPropertyName("statistics")]
    public DouyinAwemeStatistics? Statistics { get; set; }

    [SugarColumn(ColumnName = "digg_count")]
    public long? DiggCount { get; set; }

    [SugarColumn(ColumnName = "comment_count")]
    public long? CommentCount { get; set; }

    [SugarColumn(ColumnName = "share_count")]
    public long? ShareCount { get; set; }

    [SugarColumn(ColumnName = "collect_count")]
    public long? CollectCount { get; set; }

    [SugarColumn(ColumnName = "play_count")]
    public long? PlayCount { get; set; }

    [SugarColumn(IsIgnore = true)]
    [JsonPropertyName("author")]
    public DouyinAuthor? Author { get; set; }

    [SugarColumn(ColumnName = "user_id")]
    public string? UserId { get; set; }

    [SugarColumn(ColumnName = "sec_uid")]
    public string? SecUid { get; set; }

    [SugarColumn(ColumnName = "nickname")]
    public string? Nickname { get; set; }

    [SugarColumn(ColumnName = "avatar")]
    public string? Avatar { get; set; }

    [SugarColumn(IsIgnore = true)]
    [JsonPropertyName("video")]
    public DouyinVideo? Video { get; set; }

    [SugarColumn(ColumnName = "cover_url")]
    public string? CoverUrl { get; set; }

    [SugarColumn(ColumnName = "video_url")]
    public string? VideoUrl { get; set; }

    [SugarColumn(ColumnName = "aweme_url")]
    public string? AwemeUrl { get; set; }

    // 爬虫扩展字段
    [SugarColumn(ColumnName = "keyword")]
    public string? Keyword { get; set; }

    [SugarColumn(ColumnName = "crawled_at")]
    public DateTimeOffset? CrawledAt { get; set; }
}

public class DouyinAwemeStatistics
{
    [JsonPropertyName("digg_count")]
    public long DiggCount { get; set; }

    [JsonPropertyName("comment_count")]
    public long CommentCount { get; set; }

    [JsonPropertyName("share_count")]
    public long ShareCount { get; set; }

    [JsonPropertyName("collect_count")]
    public long CollectCount { get; set; }

    [JsonPropertyName("play_count")]
    public long PlayCount { get; set; }
}

public class DouyinAuthor
{
    [JsonPropertyName("uid")]
    public string? Uid { get; set; }

    [JsonPropertyName("sec_uid")]
    public string? SecUid { get; set; }

    [JsonPropertyName("nickname")]
    public string? Nickname { get; set; }

    [JsonPropertyName("avatar_thumb")]
    public DouyinImageUrl? AvatarThumb { get; set; }
}

public class DouyinVideo
{
    [JsonPropertyName("cover")]
    public DouyinImageUrl? Cover { get; set; }

    [JsonPropertyName("play_addr")]
    public DouyinPlayAddr? PlayAddr { get; set; }

    [JsonPropertyName("duration")]
    public long Duration { get; set; }
}

public class DouyinImageUrl
{
    [JsonPropertyName("url_list")]
    public List<string>? UrlList { get; set; }
}

public class DouyinPlayAddr
{
    [JsonPropertyName("url_list")]
    public List<string>? UrlList { get; set; }
}

#endregion

#region 首页推荐

/// <summary>首页推荐请求。</summary>
public class DouyinHomeFeedRequest : DouyinRequest
{
    public int TagId { get; set; } = 0; // 0=精选
    public int RefreshIndex { get; set; } = 0;
    public int Count { get; set; } = 20;
}

/// <summary>首页推荐响应。</summary>
public class DouyinHomeFeedResponse : DouyinResponse
{
    [JsonPropertyName("aweme_list")]
    public List<DouyinAweme>? AwemeList { get; set; }

    [JsonPropertyName("has_more")]
    public int HasMore { get; set; }

    [JsonPropertyName("max_cursor")]
    public long MaxCursor { get; set; }
}

#endregion
