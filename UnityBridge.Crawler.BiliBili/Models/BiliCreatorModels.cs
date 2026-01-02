namespace UnityBridge.Crawler.BiliBili.Models;

#region UP主请求/响应

/// <summary>UP主信息请求。</summary>
public class BiliUpInfoRequest : BiliRequest
{
    public string Mid { get; set; } = string.Empty;
}

/// <summary>UP主视频列表请求。</summary>
public class BiliUpVideosRequest : BiliRequest
{
    public string Mid { get; set; } = string.Empty;
    public int Pn { get; set; } = 1;
    public int Ps { get; set; } = 30;
    public string Order { get; set; } = "pubdate"; // pubdate, click, stow
}

/// <summary>UP主视频列表响应。</summary>
public class BiliUpVideosResponse : BiliResponse
{
    [JsonPropertyName("data")]
    public BiliUpVideosData? Data { get; set; }
}

public class BiliUpVideosData
{
    [JsonPropertyName("list")]
    public BiliUpVideosList? List { get; set; }

    [JsonPropertyName("page")]
    public BiliUpVideosPage? Page { get; set; }
}

public class BiliUpVideosList
{
    [JsonPropertyName("vlist")]
    public List<BiliUpVideoItem>? Vlist { get; set; }
}

public class BiliUpVideoItem
{
    [JsonPropertyName("aid")]
    public long Aid { get; set; }

    [JsonPropertyName("bvid")]
    public string? Bvid { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("pic")]
    public string? Pic { get; set; }

    [JsonPropertyName("play")]
    public long Play { get; set; }

    [JsonPropertyName("comment")]
    public long Comment { get; set; }

    [JsonPropertyName("created")]
    public long Created { get; set; }

    [JsonPropertyName("length")]
    public string? Length { get; set; }
}

public class BiliUpVideosPage
{
    [JsonPropertyName("pn")]
    public int Pn { get; set; }

    [JsonPropertyName("ps")]
    public int Ps { get; set; }

    [JsonPropertyName("count")]
    public int Count { get; set; }
}

#endregion

#region UP主实体 (API + SQLSugar)

/// <summary>B站UP主（API 响应 + SQLSugar 实体）。</summary>
[SugarTable("bili_creators")]
public class BiliCreator
{
    [SugarColumn(IsPrimaryKey = true, ColumnName = "mid")]
    [JsonPropertyName("mid")]
    public long Mid { get; set; }

    [JsonPropertyName("name")]
    public string? Nickname { get; set; }

    [JsonPropertyName("face")]
    [SugarColumn(ColumnName = "avatar")]
    public string? Avatar { get; set; }

    [JsonPropertyName("sign")]
    public string? Description { get; set; }

    [JsonPropertyName("sex")]
    public string? Sex { get; set; }

    [JsonPropertyName("level")]
    public int? Level { get; set; }

    [SugarColumn(ColumnName = "follower_count")]
    public long? FollowerCount { get; set; }

    [SugarColumn(ColumnName = "following_count")]
    public long? FollowingCount { get; set; }

    [SugarColumn(ColumnName = "video_count")]
    public int? VideoCount { get; set; }

    [SugarColumn(ColumnName = "crawled_at")]
    public DateTimeOffset? CrawledAt { get; set; }
}

#endregion
