using Microsoft.Extensions.Logging;
using UnityBridge.Crawler.Kuaishou.Models;

namespace UnityBridge.Crawler.Kuaishou.Services;

public class KuaishouCrawlerService
{
    private readonly KuaishouClient _client;
    private readonly ILogger<KuaishouCrawlerService> _logger;

    public KuaishouCrawlerService(KuaishouClient client, ILogger<KuaishouCrawlerService> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task CrawlSearchAsync(string keyword)
    {
         _logger.LogInformation("Start crawling kuaishou search: {Keyword}", keyword);
         await Task.CompletedTask;
    }
}
