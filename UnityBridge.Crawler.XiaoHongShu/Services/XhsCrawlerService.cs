using Microsoft.Extensions.Logging;
using UnityBridge.Crawler.XiaoHongShu.Models;

namespace UnityBridge.Crawler.XiaoHongShu.Services;

public class XhsCrawlerService
{
    private readonly XhsClient _client;
    private readonly ILogger<XhsCrawlerService> _logger;

    public XhsCrawlerService(XhsClient client, ILogger<XhsCrawlerService> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task CrawlNoteAsync(string noteId)
    {
         _logger.LogInformation("Start crawling XHS note: {NoteId}", noteId);
         await Task.CompletedTask;
    }
}
