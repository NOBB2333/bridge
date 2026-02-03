using Microsoft.Extensions.Logging;
using UnityBridge.Crawler.BiliBili.Models;

namespace UnityBridge.Crawler.BiliBili.Services;

/// <summary>
/// B站爬虫业务服务。
/// </summary>
public class BiliCrawlerService
{
    private readonly BiliClient _client;
    private readonly ILogger<BiliCrawlerService> _logger;

    public BiliCrawlerService(BiliClient client, ILogger<BiliCrawlerService> logger)
    {
        _client = client;
        _logger = logger;
    }

    /// <summary>
    /// 爬取指定 UP 主的所有视频（自动翻页）。
    /// </summary>
    public async Task CrawlCreatorVideosAsync(string mid)
    {
        _logger.LogInformation("Start crawling videos for creator: {Mid}", mid);
        
        // TODO: Port logic from Python's get_all_videos_by_creator
        // 1. Get total count
        // 2. Loop pages using search_video_by_keyword or specific API
        // 3. Save to Db
        
        await Task.CompletedTask;
    }
}
