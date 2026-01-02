namespace UnityBridge.Crawler.XiaoHongShu.Models;

#region 笔记详情

/// <summary>笔记详情请求。</summary>
public class XhsNoteDetailRequest : XhsRequest
{
    [JsonPropertyName("source_note_id")]
    public string NoteId { get; set; } = string.Empty;

    [JsonPropertyName("image_formats")]
    public List<string> ImageFormats { get; set; } = ["jpg", "webp", "avif"];

    [JsonPropertyName("extra")]
    public XhsNoteDetailExtra Extra { get; set; } = new();

    [JsonPropertyName("xsec_token")]
    public string? XsecToken { get; set; }

    [JsonPropertyName("xsec_source")]
    public string? XsecSource { get; set; }
}

public class XhsNoteDetailExtra
{
    [JsonPropertyName("need_body_topic")]
    public int NeedBodyTopic { get; set; } = 1;
}

/// <summary>笔记详情响应。</summary>
public class XhsNoteDetailResponse : XhsResponse
{
    [JsonPropertyName("data")]
    public XhsNoteDetailData? Data { get; set; }
}

public class XhsNoteDetailData
{
    [JsonPropertyName("items")]
    public List<XhsNoteDetailItem>? Items { get; set; }
}

public class XhsNoteDetailItem
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("note_card")]
    public XhsNoteCard? NoteCard { get; set; }
}

#endregion

#region 首页推荐

/// <summary>首页推荐请求。</summary>
public class XhsHomeFeedRequest : XhsRequest
{
    [JsonPropertyName("category")]
    public string Category { get; set; } = "homefeed_recommend";

    [JsonPropertyName("cursor_score")]
    public string CursorScore { get; set; } = string.Empty;

    [JsonPropertyName("num")]
    public int Num { get; set; } = 20;

    [JsonPropertyName("refresh_type")]
    public int RefreshType { get; set; } = 1;

    [JsonPropertyName("note_index")]
    public int NoteIndex { get; set; } = 0;

    [JsonPropertyName("image_formats")]
    public List<string> ImageFormats { get; set; } = ["jpg", "webp", "avif"];
}

/// <summary>首页推荐响应。</summary>
public class XhsHomeFeedResponse : XhsResponse
{
    [JsonPropertyName("data")]
    public XhsHomeFeedData? Data { get; set; }
}

public class XhsHomeFeedData
{
    [JsonPropertyName("items")]
    public List<XhsNoteSearchItem>? Items { get; set; }

    [JsonPropertyName("cursor_score")]
    public string? CursorScore { get; set; }
}

#endregion
