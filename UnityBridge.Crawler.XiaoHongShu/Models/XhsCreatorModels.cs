namespace UnityBridge.Crawler.XiaoHongShu.Models;

#region 创作者请求/响应

/// <summary>创作者笔记列表请求。</summary>
public class XhsCreatorNotesRequest : XhsRequest
{
    public string UserId { get; set; } = string.Empty;
    public string Cursor { get; set; } = string.Empty;
    public int Num { get; set; } = 30;
    public string? XsecToken { get; set; }
    public string XsecSource { get; set; } = "pc_feed";
}

/// <summary>创作者笔记列表响应。</summary>
public class XhsCreatorNotesResponse : XhsResponse
{
    [JsonPropertyName("data")]
    public XhsCreatorNotesData? Data { get; set; }
}

public class XhsCreatorNotesData
{
    [JsonPropertyName("notes")]
    public List<XhsNoteCard>? Notes { get; set; }

    [JsonPropertyName("cursor")]
    public string? Cursor { get; set; }

    [JsonPropertyName("has_more")]
    public bool HasMore { get; set; }
}

#endregion

#region 创作者实体 (API + SQLSugar)

/// <summary>创作者信息（API 响应 + SQLSugar 实体）。</summary>
[SugarTable("xhs_creators")]
public class XhsCreator
{
    [SugarColumn(IsPrimaryKey = true, ColumnName = "user_id")]
    [JsonPropertyName("user_id")]
    public string UserId { get; set; } = string.Empty;

    [JsonPropertyName("nickname")]
    public string? Nickname { get; set; }

    [JsonPropertyName("avatar")]
    [SugarColumn(ColumnName = "avatar_url")]
    public string? Avatar { get; set; }

    [JsonPropertyName("desc")]
    public string? Description { get; set; }

    [JsonPropertyName("gender")]
    public int? Gender { get; set; }

    [JsonPropertyName("ip_location")]
    [SugarColumn(ColumnName = "ip_location")]
    public string? IpLocation { get; set; }

    [JsonPropertyName("red_id")]
    [SugarColumn(ColumnName = "red_id")]
    public string? RedId { get; set; }

    [SugarColumn(IsIgnore = true)]
    [JsonPropertyName("interactions")]
    public List<XhsInteractionItem>? Interactions { get; set; }

    [SugarColumn(ColumnName = "fans_count")]
    public long? FansCount { get; set; }

    [SugarColumn(ColumnName = "follows_count")]
    public long? FollowsCount { get; set; }

    [SugarColumn(ColumnName = "notes_count")]
    public long? NotesCount { get; set; }

    [SugarColumn(ColumnName = "liked_count")]
    public long? LikedCount { get; set; }

    [SugarColumn(ColumnName = "collected_count")]
    public long? CollectedCount { get; set; }

    [SugarColumn(ColumnName = "level")]
    public int? Level { get; set; }

    [SugarColumn(ColumnName = "xsec_token")]
    public string? XsecToken { get; set; }

    [SugarColumn(ColumnName = "crawled_at")]
    public DateTimeOffset? CrawledAt { get; set; }
}

public class XhsInteractionItem
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("count")]
    public string? Count { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

#endregion
