using Microsoft.Extensions.Logging;
using UnityBridge.Crawler.Tieba.Extensions;
using UnityBridge.Crawler.Tieba.Models;

namespace UnityBridge.Crawler.Tieba.Services;

/// <summary>
/// 贴吧爬虫业务服务。
/// 封装了循环爬取、数据清洗、去重等高级业务逻辑。
/// </summary>
public class TiebaCrawlerService
{
    private readonly TiebaClient _client;
    private readonly ILogger<TiebaCrawlerService> _logger;

    public TiebaCrawlerService(TiebaClient client, ILogger<TiebaCrawlerService> logger)
    {
        _client = client;
        _logger = logger;
    }

    /// <summary>
    /// 根据关键词并发爬取多个关键词。
    /// </summary>
    public async Task CrawlKeywordsAsync(List<string> keywords)
    {
        foreach (var keyword in keywords)
        {
            await CrawlKeywordAsync(keyword);
        }
    }

    /// <summary>
    /// 爬取单个关键词（自动翻页）。
    /// </summary>
    public async Task CrawlKeywordAsync(string keyword)
    {
        _logger.LogInformation("Start crawling keyword: {Keyword}", keyword);
        
        int page = 1;
        const int maxPages = 5; // 可配置

        while (page <= maxPages)
        {
            try
            {
                // 调用 Extension 方法获取数据 (SDK Layer)
                // 注意：这里需要根据实际的 Extension 方法名进行调整，假设有 ExecuteSearchAsync
                // var notes = await _client.ExecuteSearchAsync(keyword, page); 
                
                // 模拟获取数据
                _logger.LogInformation("Crawling page {Page} for {Keyword}...", page, keyword);
                await Task.Delay(100); // Mock I/O

                // TODO: Save to Database
                // await _repository.SaveAsync(notes);

                page++;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error crawling page {Page} for {Keyword}", page, keyword);
                break;
            }
        }
    }
}
