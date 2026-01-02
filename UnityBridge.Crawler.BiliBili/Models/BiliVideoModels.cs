namespace UnityBridge.Crawler.BiliBili.Models;

#region 视频搜索

/// <summary>视频搜索请求。</summary>
public class BiliVideoSearchRequest : BiliRequest
{
    public string Keyword { get; set; } = string.Empty;
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string Order { get; set; } = ""; // 综合排序为空，pubdate=最新发布
}

/// <summary>视频搜索响应。</summary>
public class BiliVideoSearchResponse : BiliResponse
{
    [JsonPropertyName("data")]
    public BiliVideoSearchData? Data { get; set; }
}

public class BiliVideoSearchData
{
    [JsonPropertyName("result")]
    public List<BiliVideoSearchItem>? Result { get; set; }

    [JsonPropertyName("numPages")]
    public int NumPages { get; set; }

    [JsonPropertyName("numResults")]
    public int NumResults { get; set; }

    [JsonPropertyName("page")]
    public int Page { get; set; }
}

public class BiliVideoSearchItem
{
    [JsonPropertyName("aid")]
    public long Aid { get; set; }

    [JsonPropertyName("bvid")]
    public string? Bvid { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("author")]
    public string? Author { get; set; }

    [JsonPropertyName("mid")]
    public long Mid { get; set; }

    [JsonPropertyName("pic")]
    public string? Pic { get; set; }

    [JsonPropertyName("play")]
    public long Play { get; set; }

    [JsonPropertyName("danmaku")]
    public long Danmaku { get; set; }

    [JsonPropertyName("pubdate")]
    public long Pubdate { get; set; }

    [JsonPropertyName("duration")]
    public string? Duration { get; set; }
}

#endregion

#region 视频详情请求

/// <summary>视频详情请求。</summary>
public class BiliVideoDetailRequest : BiliRequest
{
    public string? Aid { get; set; }
    public string? Bvid { get; set; }
}

/// <summary>视频详情响应。</summary>
public class BiliVideoDetailResponse : BiliResponse
{
    [JsonPropertyName("data")]
    public BiliVideoDetailData? Data { get; set; }
}

public class BiliVideoDetailData
{
    [JsonPropertyName("View")]
    public BiliVideo? View { get; set; }
}

#endregion

#region 视频实体 (API + SQLSugar)

/// <summary>B站视频（API 响应 + SQLSugar 实体）。</summary>
[SugarTable("bili_videos")]
public class BiliVideo
{
    [SugarColumn(IsPrimaryKey = true, ColumnName = "aid")]
    [JsonPropertyName("aid")]
    public long Aid { get; set; }

    [JsonPropertyName("bvid")]
    public string? Bvid { get; set; }

    [JsonPropertyName("tname")]
    [SugarColumn(ColumnName = "video_type")]
    public string? VideoType { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("desc")]
    [SugarColumn(ColumnDataType = "text")]
    public string? Description { get; set; }

    [JsonPropertyName("ctime")]
    [SugarColumn(ColumnName = "create_time")]
    public long? CreateTime { get; set; }

    [JsonPropertyName("pubdate")]
    [SugarColumn(ColumnName = "pub_date")]
    public long? PubDate { get; set; }

    [JsonPropertyName("duration")]
    public int? Duration { get; set; }

    [SugarColumn(IsIgnore = true)]
    [JsonPropertyName("stat")]
    public BiliVideoStat? Stat { get; set; }

    [SugarColumn(ColumnName = "view_count")]
    public long? ViewCount { get; set; }

    [SugarColumn(ColumnName = "danmaku_count")]
    public long? DanmakuCount { get; set; }

    [SugarColumn(ColumnName = "like_count")]
    public long? LikeCount { get; set; }

    [SugarColumn(ColumnName = "coin_count")]
    public long? CoinCount { get; set; }

    [SugarColumn(ColumnName = "favorite_count")]
    public long? FavoriteCount { get; set; }

    [SugarColumn(ColumnName = "share_count")]
    public long? ShareCount { get; set; }

    [SugarColumn(ColumnName = "reply_count")]
    public long? ReplyCount { get; set; }

    [JsonPropertyName("pic")]
    [SugarColumn(ColumnName = "cover_url")]
    public string? CoverUrl { get; set; }

    [SugarColumn(IsIgnore = true)]
    [JsonPropertyName("owner")]
    public BiliVideoOwner? Owner { get; set; }

    [SugarColumn(ColumnName = "user_id")]
    public long? UserId { get; set; }

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

public class BiliVideoStat
{
    [JsonPropertyName("view")]
    public long View { get; set; }

    [JsonPropertyName("danmaku")]
    public long Danmaku { get; set; }

    [JsonPropertyName("reply")]
    public long Reply { get; set; }

    [JsonPropertyName("favorite")]
    public long Favorite { get; set; }

    [JsonPropertyName("coin")]
    public long Coin { get; set; }

    [JsonPropertyName("share")]
    public long Share { get; set; }

    [JsonPropertyName("like")]
    public long Like { get; set; }
}

public class BiliVideoOwner
{
    [JsonPropertyName("mid")]
    public long Mid { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("face")]
    public string? Face { get; set; }
}

#endregion

#region 首页推荐

/// <summary>首页推荐请求。</summary>
public class BiliHomeFeedRequest : BiliRequest
{
    public int FreshType { get; set; } = 4;
    public int PageCount { get; set; } = 12;
    public int FreshIdx { get; set; } = 1;
}

/// <summary>首页推荐响应。</summary>
public class BiliHomeFeedResponse : BiliResponse
{
    [JsonPropertyName("data")]
    public BiliHomeFeedData? Data { get; set; }
}

public class BiliHomeFeedData
{
    [JsonPropertyName("item")]
    public List<BiliHomeFeedItem>? Items { get; set; }
}

public class BiliHomeFeedItem
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("bvid")]
    public string? Bvid { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("pic")]
    public string? Pic { get; set; }

    [JsonPropertyName("duration")]
    public int Duration { get; set; }

    [JsonPropertyName("owner")]
    public BiliVideoOwner? Owner { get; set; }

    [JsonPropertyName("stat")]
    public BiliVideoStat? Stat { get; set; }
}

#endregion
