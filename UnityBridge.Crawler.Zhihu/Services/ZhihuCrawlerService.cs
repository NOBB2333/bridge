using Microsoft.Extensions.Logging;
using UnityBridge.Crawler.Zhihu.Models;

namespace UnityBridge.Crawler.Zhihu.Services;

public class ZhihuCrawlerService
{
    private readonly ZhihuClient _client;
    private readonly ILogger<ZhihuCrawlerService> _logger;

    public ZhihuCrawlerService(ZhihuClient client, ILogger<ZhihuCrawlerService> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task CrawlQuestionAsync(string questionId)
    {
         _logger.LogInformation("Start crawling Zhihu question: {QuestionId}", questionId);
         await Task.CompletedTask;
    }
}
