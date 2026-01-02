using UnityBridge.Crawler.Tieba.Models;

namespace UnityBridge.Crawler.Tieba.Extensions;

/// <summary>
/// TiebaClient 帖子相关扩展方法。
/// </summary>
public static class TiebaClientExecutePostExtensions
{
    extension(TiebaClient client)
    {
        /// <summary>
        /// <para>异步调用 [GET] /f/search/res 接口。</para>
        /// <para>关键词搜索帖子（返回 HTML）。</para>
        /// </summary>
        public async Task<string> ExecuteSearchHtmlAsync(
            TiebaSearchRequest request, CancellationToken ct = default)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));
            if (request is null) throw new ArgumentNullException(nameof(request));

            var parameters = new Dictionary<string, string>
            {
                ["isnew"] = "1",
                ["qw"] = request.Keyword,
                ["rn"] = request.PageSize.ToString(),
                ["pn"] = request.Page.ToString(),
                ["sm"] = request.SortType.ToString(),
                ["only_thread"] = request.OnlyThread.ToString()
            };

            return await client.SendGetHtmlAsync("/f/search/res", parameters, ct);
        }

        /// <summary>
        /// <para>异步调用 [GET] /p/{post_id} 接口。</para>
        /// <para>获取帖子详情（返回 HTML）。</para>
        /// </summary>
        public async Task<string> ExecutePostDetailHtmlAsync(
            TiebaPostDetailRequest request, CancellationToken ct = default)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));
            if (request is null) throw new ArgumentNullException(nameof(request));

            return await client.SendGetHtmlAsync($"/p/{request.PostId}", null, ct);
        }

        /// <summary>
        /// <para>异步调用 [GET] /f 接口。</para>
        /// <para>获取贴吧帖子列表（返回 HTML）。</para>
        /// </summary>
        public async Task<string> ExecuteForumHtmlAsync(
            TiebaForumRequest request, CancellationToken ct = default)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));
            if (request is null) throw new ArgumentNullException(nameof(request));

            var parameters = new Dictionary<string, string>
            {
                ["kw"] = request.TiebaName,
                ["pn"] = request.PageNum.ToString()
            };

            return await client.SendGetHtmlAsync("/f", parameters, ct);
        }
    }
}
