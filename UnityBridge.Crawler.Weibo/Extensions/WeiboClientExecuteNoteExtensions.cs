using UnityBridge.Crawler.Weibo.Models;

namespace UnityBridge.Crawler.Weibo.Extensions;

/// <summary>
/// WeiboClient 笔记相关扩展方法。
/// </summary>
public static class WeiboClientExecuteNoteExtensions
{
    extension(WeiboClient client)
    {
        /// <summary>
        /// <para>异步调用 [GET] /api/container/getIndex 接口。</para>
        /// <para>关键词搜索微博。</para>
        /// </summary>
        public async Task<WeiboSearchResponse> ExecuteSearchAsync(
            WeiboSearchRequest request, CancellationToken ct = default)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));
            if (request is null) throw new ArgumentNullException(nameof(request));

            var containerId = $"100103type={request.SearchType}&q={request.Keyword}";
            var parameters = new Dictionary<string, string>
            {
                ["containerid"] = containerId,
                ["page_type"] = "searchall",
                ["page"] = request.Page.ToString()
            };

            return await client.SendGetJsonAsync<WeiboSearchResponse>(
                "/api/container/getIndex", parameters, null, ct);
        }

        /// <summary>
        /// <para>异步调用 [GET] /detail/{noteId} 接口。</para>
        /// <para>获取微博详情（返回 HTML）。</para>
        /// </summary>
        public async Task<string> ExecuteNoteDetailHtmlAsync(
            WeiboNoteDetailRequest request, CancellationToken ct = default)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));
            if (request is null) throw new ArgumentNullException(nameof(request));

            return await client.SendGetHtmlAsync($"/detail/{request.NoteId}", ct);
        }
    }
}
