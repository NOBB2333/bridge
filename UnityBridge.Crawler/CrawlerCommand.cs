using SqlSugar;
using UnityBridge.Crawler.XiaoHongShu;
using UnityBridge.Crawler.XiaoHongShu.Extensions;
using UnityBridge.Crawler.XiaoHongShu.Models;
using UnityBridge.Crawler.BiliBili;
using UnityBridge.Crawler.BiliBili.Extensions;
using UnityBridge.Crawler.BiliBili.Models;
using UnityBridge.Crawler.Douyin;
using UnityBridge.Crawler.Douyin.Extensions;
using UnityBridge.Crawler.Douyin.Models;
using UnityBridge.Crawler.Kuaishou;
using UnityBridge.Crawler.Kuaishou.Extensions;
using UnityBridge.Crawler.Kuaishou.Models;
using UnityBridge.Crawler.Zhihu;
using UnityBridge.Crawler.Zhihu.Extensions;
using UnityBridge.Crawler.Zhihu.Models;
using UnityBridge.Crawler.Weibo;
using UnityBridge.Crawler.Weibo.Extensions;
using UnityBridge.Crawler.Weibo.Models;
using UnityBridge.Crawler.Core.SignService;
using UnityBridge.Crawler.Core.AccountPool;

namespace UnityBridge.Crawler;

/// <summary>
/// 爬虫命令入口。
/// </summary>
public static class CrawlerCommand
{
    #region 小红书 (XiaoHongShu)

    /// <summary>
    /// 小红书关键词搜索并存储。
    /// </summary>
    public static async Task XhsSearchAsync(
        XhsClient client,
        SqlSugarClient db,
        string keyword,
        int maxPages = 10,
        int delayMinMs = 1000,
        int delayMaxMs = 3000,
        CancellationToken ct = default)
    {
        Console.WriteLine($"[XHS] 开始搜索关键词：{keyword}");

        for (int page = 1; page <= maxPages && !ct.IsCancellationRequested; page++)
        {
            try
            {
                var request = new XhsNoteSearchRequest
                {
                    Keyword = keyword,
                    Page = page,
                    PageSize = 20
                };

                var response = await client.ExecuteNoteSearchAsync(request, ct);

                if (!response.IsSuccessful())
                {
                    Console.WriteLine($"[XHS] 搜索失败：{response.Code} {response.Message}");

                    if (await client.SwitchToNextAccountAsync(ct))
                    {
                        Console.WriteLine("[XHS] 已切换到新账号，重试...");
                        page--;
                        continue;
                    }
                    break;
                }

                if (response.Data?.Items is not { Count: > 0 } items)
                {
                    Console.WriteLine("[XHS] 没有更多结果。");
                    break;
                }

                var notes = items
                    .Where(i => i.NoteCard is not null)
                    .Select(i =>
                    {
                        var note = i.NoteCard!;
                        note.XsecToken = i.XsecToken;
                        note.Keyword = keyword;
                        note.CrawledAt = DateTimeOffset.Now;

                        if (note.User is not null)
                        {
                            note.UserId = note.User.UserId;
                            note.UserNickname = note.User.Nickname;
                        }

                        if (note.ImageList is { Count: > 0 })
                        {
                            note.ImageUrls = string.Join(",", note.ImageList.Select(img => img.Url));
                        }

                        if (note.Video is not null)
                        {
                            note.VideoUrl = note.Video.Url;
                        }

                        if (note.InteractInfo is not null)
                        {
                            long.TryParse(note.InteractInfo.LikedCount, out var liked);
                            long.TryParse(note.InteractInfo.CollectedCount, out var collected);
                            long.TryParse(note.InteractInfo.CommentCount, out var comment);
                            long.TryParse(note.InteractInfo.ShareCount, out var share);
                            note.LikedCount = liked;
                            note.CollectedCount = collected;
                            note.CommentCount = comment;
                            note.ShareCount = share;
                        }

                        return note;
                    })
                    .ToList();

                var count = await db.Storageable(notes).ExecuteCommandAsync(ct);
                Console.WriteLine($"[XHS] 第 {page} 页：获取 {items.Count} 条，存储 {count} 条笔记");

                if (!response.Data.HasMore)
                {
                    Console.WriteLine("[XHS] 已到达最后一页。");
                    break;
                }

                await Task.Delay(Random.Shared.Next(delayMinMs, delayMaxMs), ct);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("[XHS] 搜索已取消。");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[XHS] 搜索异常：{ex.Message}");
                await client.MarkCurrentAccountInvalidAsync(ct);
            }
        }

        Console.WriteLine($"[XHS] 搜索完成：{keyword}");
    }

    /// <summary>
    /// 小红书获取笔记详情。
    /// </summary>
    public static async Task<XhsNoteCard?> XhsGetNoteDetailAsync(
        XhsClient client,
        SqlSugarClient db,
        string noteId,
        string? xsecToken = null,
        CancellationToken ct = default)
    {
        var request = new XhsNoteDetailRequest
        {
            NoteId = noteId,
            XsecToken = xsecToken
        };

        var response = await client.ExecuteNoteDetailAsync(request, ct);

        if (!response.IsSuccessful() || response.Data?.Items is not { Count: > 0 })
        {
            Console.WriteLine($"[XHS] 获取笔记详情失败：{noteId}");
            return null;
        }

        var note = response.Data.Items[0].NoteCard;
        if (note is not null)
        {
            note.XsecToken = xsecToken;
            note.CrawledAt = DateTimeOffset.Now;
            note.NoteUrl = $"https://www.xiaohongshu.com/explore/{noteId}";

            await db.Storageable(note).ExecuteCommandAsync(ct);
        }

        return note;
    }

    /// <summary>
    /// 小红书获取笔记评论（含二级）。
    /// </summary>
    public static async Task XhsGetCommentsAsync(
        XhsClient client,
        SqlSugarClient db,
        string noteId,
        string? xsecToken = null,
        bool includeSubComments = true,
        CancellationToken ct = default)
    {
        Console.WriteLine($"[XHS] 开始获取评论：{noteId}");

        string cursor = string.Empty;
        int totalComments = 0;

        while (!ct.IsCancellationRequested)
        {
            var request = new XhsCommentPageRequest
            {
                NoteId = noteId,
                Cursor = cursor,
                XsecToken = xsecToken
            };

            var response = await client.ExecuteCommentPageAsync(request, ct);

            if (!response.IsSuccessful() || response.Data?.Comments is not { Count: > 0 } comments)
            {
                break;
            }

            foreach (var comment in comments)
            {
                comment.NoteId = noteId;
                comment.XsecToken = xsecToken;
                comment.CrawledAt = DateTimeOffset.Now;

                if (comment.UserInfo is not null)
                {
                    comment.UserId = comment.UserInfo.UserId;
                    comment.UserNickname = comment.UserInfo.Nickname;
                }
            }

            var count = await db.Storageable(comments).ExecuteCommandAsync(ct);
            totalComments += count;

            if (includeSubComments)
            {
                foreach (var comment in comments)
                {
                    if (int.TryParse(comment.SubCommentCount, out var subCount) && subCount > 0)
                    {
                        await XhsGetSubCommentsAsync(client, db, noteId, comment.CommentId, xsecToken, ct);
                    }
                }
            }

            if (!response.Data.HasMore || string.IsNullOrEmpty(response.Data.Cursor))
            {
                break;
            }

            cursor = response.Data.Cursor;
            await Task.Delay(Random.Shared.Next(500, 1500), ct);
        }

        Console.WriteLine($"[XHS] 评论获取完成：共 {totalComments} 条");
    }

    private static async Task XhsGetSubCommentsAsync(
        XhsClient client,
        SqlSugarClient db,
        string noteId,
        string rootCommentId,
        string? xsecToken,
        CancellationToken ct)
    {
        string cursor = string.Empty;

        while (!ct.IsCancellationRequested)
        {
            var request = new XhsSubCommentRequest
            {
                NoteId = noteId,
                RootCommentId = rootCommentId,
                Cursor = cursor,
                XsecToken = xsecToken
            };

            var response = await client.ExecuteSubCommentAsync(request, ct);

            if (!response.IsSuccessful() || response.Data?.Comments is not { Count: > 0 } subComments)
            {
                break;
            }

            foreach (var sub in subComments)
            {
                sub.NoteId = noteId;
                sub.ParentCommentId = rootCommentId;
                sub.XsecToken = xsecToken;
                sub.CrawledAt = DateTimeOffset.Now;

                if (sub.UserInfo is not null)
                {
                    sub.UserId = sub.UserInfo.UserId;
                    sub.UserNickname = sub.UserInfo.Nickname;
                }
            }

            await db.Storageable(subComments).ExecuteCommandAsync(ct);

            if (!response.Data.HasMore || string.IsNullOrEmpty(response.Data.Cursor))
            {
                break;
            }

            cursor = response.Data.Cursor;
            await Task.Delay(Random.Shared.Next(300, 800), ct);
        }
    }

    #endregion

    #region B站 (BiliBili)

    /// <summary>
    /// B站关键词搜索并存储。
    /// </summary>
    public static async Task BiliSearchAsync(
        BiliClient client,
        SqlSugarClient db,
        string keyword,
        int maxPages = 10,
        int delayMinMs = 1000,
        int delayMaxMs = 3000,
        CancellationToken ct = default)
    {
        Console.WriteLine($"[Bili] 开始搜索关键词：{keyword}");

        for (int page = 1; page <= maxPages && !ct.IsCancellationRequested; page++)
        {
            try
            {
                var request = new BiliVideoSearchRequest
                {
                    Keyword = keyword,
                    Page = page,
                    PageSize = 20
                };

                var response = await client.ExecuteVideoSearchAsync(request, ct);

                if (!response.IsSuccessful() || response.Data?.Result is not { Count: > 0 } results)
                {
                    Console.WriteLine("[Bili] 没有更多结果。");
                    break;
                }

                var videos = results.Select(v => new BiliVideo
                {
                    Aid = v.Aid,
                    Bvid = v.Bvid,
                    Title = v.Title,
                    Description = v.Description,
                    CoverUrl = v.Pic,
                    Nickname = v.Author,
                    UserId = v.Mid,
                    ViewCount = v.Play,
                    DanmakuCount = v.Danmaku,
                    PubDate = v.Pubdate,
                    Keyword = keyword,
                    CrawledAt = DateTimeOffset.Now
                }).ToList();

                var count = await db.Storageable(videos).ExecuteCommandAsync(ct);
                Console.WriteLine($"[Bili] 第 {page} 页：获取 {results.Count} 条，存储 {count} 条视频");

                await Task.Delay(Random.Shared.Next(delayMinMs, delayMaxMs), ct);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("[Bili] 搜索已取消。");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Bili] 搜索异常：{ex.Message}");
            }
        }

        Console.WriteLine($"[Bili] 搜索完成：{keyword}");
    }

    #endregion

    #region 抖音 (Douyin)

    /// <summary>
    /// 抖音关键词搜索并存储。
    /// </summary>
    public static async Task DouyinSearchAsync(
        DouyinClient client,
        SqlSugarClient db,
        string keyword,
        int maxPages = 10,
        int delayMinMs = 1000,
        int delayMaxMs = 3000,
        CancellationToken ct = default)
    {
        Console.WriteLine($"[Douyin] 开始搜索关键词：{keyword}");
        int offset = 0;

        for (int page = 1; page <= maxPages && !ct.IsCancellationRequested; page++)
        {
            try
            {
                var request = new DouyinSearchRequest
                {
                    Keyword = keyword,
                    Offset = offset
                };

                var response = await client.ExecuteSearchAsync(request, ct);

                if (!response.IsSuccessful() || response.Data is not { Count: > 0 } results)
                {
                    Console.WriteLine("[Douyin] 没有更多结果。");
                    break;
                }

                var awemes = results
                    .Where(r => r.AwemeInfo is not null)
                    .Select(r =>
                    {
                        var a = r.AwemeInfo!;
                        a.Keyword = keyword;
                        a.CrawledAt = DateTimeOffset.Now;

                        if (a.Author is not null)
                        {
                            a.UserId = a.Author.Uid;
                            a.Nickname = a.Author.Nickname;
                        }

                        return a;
                    }).ToList();

                var count = await db.Storageable(awemes).ExecuteCommandAsync(ct);
                Console.WriteLine($"[Douyin] 第 {page} 页：获取 {results.Count} 条，存储 {count} 条视频");

                if (response.HasMore == 0)
                {
                    Console.WriteLine("[Douyin] 已到达最后一页。");
                    break;
                }

                offset = response.Cursor;
                await Task.Delay(Random.Shared.Next(delayMinMs, delayMaxMs), ct);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("[Douyin] 搜索已取消。");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Douyin] 搜索异常：{ex.Message}");
            }
        }

        Console.WriteLine($"[Douyin] 搜索完成：{keyword}");
    }

    #endregion

    #region 快手 (Kuaishou)

    /// <summary>
    /// 快手关键词搜索并存储。
    /// </summary>
    public static async Task KuaishouSearchAsync(
        KuaishouClient client,
        SqlSugarClient db,
        string keyword,
        int maxPages = 10,
        int delayMinMs = 1000,
        int delayMaxMs = 3000,
        CancellationToken ct = default)
    {
        Console.WriteLine($"[Kuaishou] 开始搜索关键词：{keyword}");
        string pcursor = string.Empty;
        string searchSessionId = string.Empty;

        for (int page = 1; page <= maxPages && !ct.IsCancellationRequested; page++)
        {
            try
            {
                var request = new KuaishouSearchRequest
                {
                    Keyword = keyword,
                    Pcursor = pcursor,
                    SearchSessionId = searchSessionId
                };

                var response = await client.ExecuteSearchAsync(request, ct);

                if (!response.IsSuccessful() || response.Data?.VisionSearchPhoto?.Feeds is not { Count: > 0 } feeds)
                {
                    Console.WriteLine("[Kuaishou] 没有更多结果。");
                    break;
                }

                var videos = feeds
                    .Where(f => f.Photo is not null)
                    .Select(f =>
                    {
                        var v = f.Photo!;
                        v.Keyword = keyword;
                        v.CrawledAt = DateTimeOffset.Now;

                        if (v.Author is not null)
                        {
                            v.UserId = v.Author.Id;
                            v.Nickname = v.Author.Name;
                            v.Avatar = v.Author.HeaderUrl;
                        }

                        return v;
                    }).ToList();

                var count = await db.Storageable(videos).ExecuteCommandAsync(ct);
                Console.WriteLine($"[Kuaishou] 第 {page} 页：获取 {feeds.Count} 条，存储 {count} 条视频");

                pcursor = response.Data.VisionSearchPhoto.Pcursor ?? string.Empty;
                searchSessionId = response.Data.VisionSearchPhoto.SearchSessionId ?? string.Empty;

                if (string.IsNullOrEmpty(pcursor))
                {
                    Console.WriteLine("[Kuaishou] 已到达最后一页。");
                    break;
                }

                await Task.Delay(Random.Shared.Next(delayMinMs, delayMaxMs), ct);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("[Kuaishou] 搜索已取消。");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Kuaishou] 搜索异常：{ex.Message}");
            }
        }

        Console.WriteLine($"[Kuaishou] 搜索完成：{keyword}");
    }

    #endregion

    #region 知乎 (Zhihu)

    /// <summary>
    /// 知乎关键词搜索并存储。
    /// </summary>
    public static async Task ZhihuSearchAsync(
        ZhihuClient client,
        SqlSugarClient db,
        string keyword,
        int maxPages = 10,
        int delayMinMs = 1000,
        int delayMaxMs = 3000,
        CancellationToken ct = default)
    {
        Console.WriteLine($"[Zhihu] 开始搜索关键词：{keyword}");

        for (int page = 1; page <= maxPages && !ct.IsCancellationRequested; page++)
        {
            try
            {
                var request = new ZhihuSearchRequest
                {
                    Keyword = keyword,
                    Page = page,
                    PageSize = 20
                };

                var response = await client.ExecuteSearchAsync(request, ct);

                if (!response.IsSuccessful() || response.Data is not { Count: > 0 } results)
                {
                    Console.WriteLine("[Zhihu] 没有更多结果。");
                    break;
                }

                var contents = results
                    .Where(r => r.Object is not null)
                    .Select(r =>
                    {
                        var c = r.Object!;
                        c.Keyword = keyword;
                        c.CrawledAt = DateTimeOffset.Now;

                        if (c.Author is not null)
                        {
                            c.UserId = c.Author.Id;
                            c.UserNickname = c.Author.Name;
                            c.UserUrlToken = c.Author.UrlToken;
                            c.UserAvatar = c.Author.AvatarUrl;
                        }

                        return c;
                    }).ToList();

                var count = await db.Storageable(contents).ExecuteCommandAsync(ct);
                Console.WriteLine($"[Zhihu] 第 {page} 页：获取 {results.Count} 条，存储 {count} 条内容");

                if (response.Paging?.IsEnd == true)
                {
                    Console.WriteLine("[Zhihu] 已到达最后一页。");
                    break;
                }

                await Task.Delay(Random.Shared.Next(delayMinMs, delayMaxMs), ct);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("[Zhihu] 搜索已取消。");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Zhihu] 搜索异常：{ex.Message}");
            }
        }

        Console.WriteLine($"[Zhihu] 搜索完成：{keyword}");
    }

    #endregion

    #region 微博 (Weibo)

    /// <summary>
    /// 微博关键词搜索并存储。
    /// </summary>
    public static async Task WeiboSearchAsync(
        WeiboClient client,
        SqlSugarClient db,
        string keyword,
        int maxPages = 10,
        int delayMinMs = 1000,
        int delayMaxMs = 3000,
        CancellationToken ct = default)
    {
        Console.WriteLine($"[Weibo] 开始搜索关键词：{keyword}");

        for (int page = 1; page <= maxPages && !ct.IsCancellationRequested; page++)
        {
            try
            {
                var request = new WeiboSearchRequest
                {
                    Keyword = keyword,
                    Page = page
                };

                var response = await client.ExecuteSearchAsync(request, ct);

                if (!response.IsSuccessful() || response.Data?.Cards is not { Count: > 0 } cards)
                {
                    Console.WriteLine("[Weibo] 没有更多结果。");
                    break;
                }

                var notes = new List<WeiboNote>();
                foreach (var card in cards)
                {
                    if (card.CardType == 9 && card.Mblog is not null)
                    {
                        var note = card.Mblog;
                        note.Keyword = keyword;
                        note.CrawledAt = DateTimeOffset.Now;

                        if (note.User is not null)
                        {
                            note.UserId = note.User.Id.ToString();
                            note.Nickname = note.User.ScreenName;
                            note.Avatar = note.User.ProfileImageUrl;
                        }

                        notes.Add(note);
                    }

                    if (card.CardGroup is { Count: > 0 })
                    {
                        foreach (var subCard in card.CardGroup)
                        {
                            if (subCard.CardType == 9 && subCard.Mblog is not null)
                            {
                                var note = subCard.Mblog;
                                note.Keyword = keyword;
                                note.CrawledAt = DateTimeOffset.Now;

                                if (note.User is not null)
                                {
                                    note.UserId = note.User.Id.ToString();
                                    note.Nickname = note.User.ScreenName;
                                    note.Avatar = note.User.ProfileImageUrl;
                                }

                                notes.Add(note);
                            }
                        }
                    }
                }

                if (notes.Count == 0)
                {
                    Console.WriteLine("[Weibo] 没有更多结果。");
                    break;
                }

                var count = await db.Storageable(notes).ExecuteCommandAsync(ct);
                Console.WriteLine($"[Weibo] 第 {page} 页：获取 {notes.Count} 条，存储 {count} 条微博");

                await Task.Delay(Random.Shared.Next(delayMinMs, delayMaxMs), ct);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("[Weibo] 搜索已取消。");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Weibo] 搜索异常：{ex.Message}");
            }
        }

        Console.WriteLine($"[Weibo] 搜索完成：{keyword}");
    }

    #endregion
}

