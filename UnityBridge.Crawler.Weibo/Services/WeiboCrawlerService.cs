using Microsoft.Extensions.Logging;
using UnityBridge.Crawler.Weibo.Models;

namespace UnityBridge.Crawler.Weibo.Services;

public class WeiboCrawlerService
{
    private readonly WeiboClient _client;
    private readonly ILogger<WeiboCrawlerService> _logger;

    public WeiboCrawlerService(WeiboClient client, ILogger<WeiboCrawlerService> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task CrawlSearchAsync(string keyword)
    {
         _logger.LogInformation("Start crawling weibo search: {Keyword}", keyword);
         await Task.CompletedTask;
    }
}
