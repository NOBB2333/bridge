using UnityBridge.Crawler.Tieba.Models;

namespace UnityBridge.Crawler.Tieba.Extensions;

/// <summary>
/// TiebaClient 评论相关扩展方法。
/// </summary>
public static class TiebaClientExecuteCommentExtensions
{
    extension(TiebaClient client)
    {
        /// <summary>
        /// <para>异步调用 [GET] /p/{post_id}?pn={page} 接口。</para>
        /// <para>获取帖子评论（返回 HTML）。</para>
        /// </summary>
        public async Task<string> ExecuteCommentHtmlAsync(
            TiebaCommentRequest request, CancellationToken ct = default)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));
            if (request is null) throw new ArgumentNullException(nameof(request));

            var parameters = new Dictionary<string, string>
            {
                ["pn"] = request.Page.ToString()
            };

            return await client.SendGetHtmlAsync($"/p/{request.PostId}", parameters, ct);
        }

        /// <summary>
        /// <para>异步调用 [GET] /p/comment 接口。</para>
        /// <para>获取子评论（返回 HTML）。</para>
        /// </summary>
        public async Task<string> ExecuteSubCommentHtmlAsync(
            TiebaSubCommentRequest request, CancellationToken ct = default)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));
            if (request is null) throw new ArgumentNullException(nameof(request));

            var parameters = new Dictionary<string, string>
            {
                ["tid"] = request.PostId,
                ["pid"] = request.ParentCommentId,
                ["fid"] = request.TiebaId,
                ["pn"] = request.Page.ToString()
            };

            return await client.SendGetHtmlAsync("/p/comment", parameters, ct);
        }
    }
}
