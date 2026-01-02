using SqlSugar;
using UnityBridge.Crawler.XiaoHongShu.Models;
using UnityBridge.Crawler.BiliBili.Models;
using UnityBridge.Crawler.Douyin.Models;
using UnityBridge.Crawler.Tieba.Models;
using UnityBridge.Crawler.Kuaishou.Models;
using UnityBridge.Crawler.Zhihu.Models;
using UnityBridge.Crawler.Weibo.Models;

namespace UnityBridge.Crawler;

/// <summary>
/// 爬虫存储辅助类。
/// </summary>
public static class CrawlerStorageHelper
{
    /// <summary>
    /// 创建 SQLite 数据库客户端。
    /// </summary>
    public static SqlSugarClient CreateSqliteDb(string? dbPath = null)
    {
        // 启用 AOT 模式
        StaticConfig.EnableAot = true;

        var connectionString = $"Data Source={dbPath ?? "crawler.db"};";

        var db = new SqlSugarClient(new ConnectionConfig
        {
            ConnectionString = connectionString,
            DbType = DbType.Sqlite,
            IsAutoCloseConnection = true,
            InitKeyType = InitKeyType.Attribute
        });

        InitAllTables(db);

        Console.WriteLine($"[Storage] SQLite 数据库已初始化：{dbPath ?? "crawler.db"}");

        return db;
    }

    /// <summary>
    /// 创建 MySQL 数据库客户端。
    /// </summary>
    public static SqlSugarClient CreateMySqlDb(string connectionString)
    {
        // 启用 AOT 模式
        StaticConfig.EnableAot = true;

        var db = new SqlSugarClient(new ConnectionConfig
        {
            ConnectionString = connectionString,
            DbType = DbType.MySql,
            IsAutoCloseConnection = true,
            InitKeyType = InitKeyType.Attribute
        });

        InitAllTables(db);

        Console.WriteLine("[Storage] MySQL 数据库已初始化");

        return db;
    }

    /// <summary>
    /// 初始化所有平台的表。
    /// </summary>
    private static void InitAllTables(SqlSugarClient db)
    {
        // 小红书
        db.CodeFirst.InitTables<XhsNoteCard>();
        db.CodeFirst.InitTables<XhsComment>();
        db.CodeFirst.InitTables<XhsCreator>();

        // B站
        db.CodeFirst.InitTables<BiliVideo>();
        db.CodeFirst.InitTables<BiliComment>();
        db.CodeFirst.InitTables<BiliCreator>();

        // 抖音
        db.CodeFirst.InitTables<DouyinAweme>();
        db.CodeFirst.InitTables<DouyinComment>();
        db.CodeFirst.InitTables<DouyinCreator>();

        // 百度贴吧
        db.CodeFirst.InitTables<TiebaPost>();
        db.CodeFirst.InitTables<TiebaComment>();
        db.CodeFirst.InitTables<TiebaCreator>();

        // 快手
        db.CodeFirst.InitTables<KuaishouVideo>();
        db.CodeFirst.InitTables<KuaishouComment>();
        db.CodeFirst.InitTables<KuaishouCreator>();

        // 知乎
        db.CodeFirst.InitTables<ZhihuContent>();
        db.CodeFirst.InitTables<ZhihuComment>();
        db.CodeFirst.InitTables<ZhihuCreator>();

        // 微博
        db.CodeFirst.InitTables<WeiboNote>();
        db.CodeFirst.InitTables<WeiboComment>();
        db.CodeFirst.InitTables<WeiboCreator>();
    }
}

