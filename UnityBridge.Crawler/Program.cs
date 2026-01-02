using Microsoft.Extensions.Configuration;
using SqlSugar;

namespace UnityBridge.Crawler;

/// <summary>
/// 爬虫主程序入口。
/// </summary>
public class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("===========================================");
        Console.WriteLine("   UnityBridge.Crawler - 社交媒体爬虫工具");
        Console.WriteLine("===========================================");
        Console.WriteLine();

        // 加载配置
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("Configuration/appsettings.json", optional: true, reloadOnChange: true)
            .Build();

        var options = new CrawlerOptions();
        configuration.GetSection("Crawler").Bind(options);

        Console.WriteLine($"[配置] 签名服务: {options.SignServerUrl}");
        Console.WriteLine($"[配置] 数据库类型: {options.Database.Type}");
        Console.WriteLine($"[配置] 延迟范围: {options.DefaultDelay.MinMs}-{options.DefaultDelay.MaxMs}ms");
        Console.WriteLine($"[配置] 最大页数: {options.MaxPages}");
        Console.WriteLine();

        // 初始化数据库
        var db = options.Database.Type.ToLowerInvariant() switch
        {
            "mysql" => CrawlerStorageHelper.CreateMySqlDb(options.Database.ConnectionString),
            _ => CrawlerStorageHelper.CreateSqliteDb(options.Database.ConnectionString.Replace("Data Source=", "").Replace(";", ""))
        };

        // 解析命令行参数
        if (args.Length == 0)
        {
            ShowHelp();
            return;
        }

        var command = args[0].ToLowerInvariant();
        var keyword = args.Length > 1 ? args[1] : string.Empty;
        var platform = args.Length > 2 ? args[2].ToLowerInvariant() : "all";

        switch (command)
        {
            case "search":
                if (string.IsNullOrEmpty(keyword))
                {
                    Console.WriteLine("[错误] 请提供搜索关键词！");
                    ShowHelp();
                    return;
                }
                await SearchAsync(db, options, keyword, platform);
                break;

            case "help":
                ShowHelp();
                break;

            default:
                Console.WriteLine($"[错误] 未知命令: {command}");
                ShowHelp();
                break;
        }

        Console.WriteLine();
        Console.WriteLine("[完成] 程序执行结束。");
    }

    private static async Task SearchAsync(SqlSugarClient db, CrawlerOptions options, string keyword, string platform)
    {
        Console.WriteLine($"[搜索] 关键词: {keyword}");
        Console.WriteLine($"[搜索] 平台: {platform}");
        Console.WriteLine();

        var cts = new CancellationTokenSource();
        Console.CancelKeyPress += (_, e) =>
        {
            e.Cancel = true;
            cts.Cancel();
            Console.WriteLine("\n[中断] 正在取消任务...");
        };

        var tasks = new List<Task>();

        // 小红书
        if ((platform == "all" || platform == "xhs") && options.Platforms.XiaoHongShu.Enabled)
        {
            if (!string.IsNullOrEmpty(options.Platforms.XiaoHongShu.Cookies))
            {
                var client = CrawlerFactory.CreateXhsClient(
                    options.Platforms.XiaoHongShu.Cookies,
                    options.SignServerUrl);
                tasks.Add(CrawlerCommand.XhsSearchAsync(client, db, keyword,
                    options.MaxPages, options.DefaultDelay.MinMs, options.DefaultDelay.MaxMs, cts.Token));
            }
            else
            {
                Console.WriteLine("[跳过] 小红书: 未配置 Cookies");
            }
        }

        // B站
        if ((platform == "all" || platform == "bili") && options.Platforms.BiliBili.Enabled)
        {
            if (!string.IsNullOrEmpty(options.Platforms.BiliBili.Cookies))
            {
                var client = CrawlerFactory.CreateBiliClient(
                    options.Platforms.BiliBili.Cookies,
                    options.SignServerUrl);
                tasks.Add(CrawlerCommand.BiliSearchAsync(client, db, keyword,
                    options.MaxPages, options.DefaultDelay.MinMs, options.DefaultDelay.MaxMs, cts.Token));
            }
            else
            {
                Console.WriteLine("[跳过] B站: 未配置 Cookies");
            }
        }

        // 抖音
        if ((platform == "all" || platform == "douyin") && options.Platforms.Douyin.Enabled)
        {
            if (!string.IsNullOrEmpty(options.Platforms.Douyin.Cookies))
            {
                var client = CrawlerFactory.CreateDouyinClient(
                    options.Platforms.Douyin.Cookies,
                    options.SignServerUrl);
                tasks.Add(CrawlerCommand.DouyinSearchAsync(client, db, keyword,
                    options.MaxPages, options.DefaultDelay.MinMs, options.DefaultDelay.MaxMs, cts.Token));
            }
            else
            {
                Console.WriteLine("[跳过] 抖音: 未配置 Cookies");
            }
        }

        // 快手
        if ((platform == "all" || platform == "kuaishou") && options.Platforms.Kuaishou.Enabled)
        {
            if (!string.IsNullOrEmpty(options.Platforms.Kuaishou.Cookies))
            {
                var client = CrawlerFactory.CreateKuaishouClient(options.Platforms.Kuaishou.Cookies);
                tasks.Add(CrawlerCommand.KuaishouSearchAsync(client, db, keyword,
                    options.MaxPages, options.DefaultDelay.MinMs, options.DefaultDelay.MaxMs, cts.Token));
            }
            else
            {
                Console.WriteLine("[跳过] 快手: 未配置 Cookies");
            }
        }

        // 知乎
        if ((platform == "all" || platform == "zhihu") && options.Platforms.Zhihu.Enabled)
        {
            if (!string.IsNullOrEmpty(options.Platforms.Zhihu.Cookies))
            {
                var client = CrawlerFactory.CreateZhihuClient(
                    options.Platforms.Zhihu.Cookies,
                    options.SignServerUrl);
                tasks.Add(CrawlerCommand.ZhihuSearchAsync(client, db, keyword,
                    options.MaxPages, options.DefaultDelay.MinMs, options.DefaultDelay.MaxMs, cts.Token));
            }
            else
            {
                Console.WriteLine("[跳过] 知乎: 未配置 Cookies");
            }
        }

        // 微博
        if ((platform == "all" || platform == "weibo") && options.Platforms.Weibo.Enabled)
        {
            if (!string.IsNullOrEmpty(options.Platforms.Weibo.Cookies))
            {
                var client = CrawlerFactory.CreateWeiboClient(options.Platforms.Weibo.Cookies);
                tasks.Add(CrawlerCommand.WeiboSearchAsync(client, db, keyword,
                    options.MaxPages, options.DefaultDelay.MinMs, options.DefaultDelay.MaxMs, cts.Token));
            }
            else
            {
                Console.WriteLine("[跳过] 微博: 未配置 Cookies");
            }
        }

        if (tasks.Count == 0)
        {
            Console.WriteLine("[警告] 没有可执行的任务，请检查配置文件中的 Cookies。");
            return;
        }

        try
        {
            await Task.WhenAll(tasks);
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("[中断] 所有任务已取消。");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[错误] 任务执行失败: {ex.Message}");
        }
    }

    private static void ShowHelp()
    {
        Console.WriteLine("用法:");
        Console.WriteLine("  UnityBridge.Crawler search <关键词> [平台]");
        Console.WriteLine();
        Console.WriteLine("平台选项:");
        Console.WriteLine("  all      - 所有已配置的平台（默认）");
        Console.WriteLine("  xhs      - 小红书");
        Console.WriteLine("  bili     - B站");
        Console.WriteLine("  douyin   - 抖音");
        Console.WriteLine("  kuaishou - 快手");
        Console.WriteLine("  zhihu    - 知乎");
        Console.WriteLine("  weibo    - 微博");
        Console.WriteLine();
        Console.WriteLine("示例:");
        Console.WriteLine("  UnityBridge.Crawler search \"人工智能\"");
        Console.WriteLine("  UnityBridge.Crawler search \"Python教程\" bili");
        Console.WriteLine("  UnityBridge.Crawler search \"美食\" xhs");
        Console.WriteLine();
        Console.WriteLine("配置文件: Configuration/appsettings.json");
    }
}
