using UnityBridge.Crawler.Douyin.Models;

namespace UnityBridge.Crawler.Douyin.Extensions;

/// <summary>
/// DouyinClient 评论相关扩展方法。
/// </summary>
public static class DouyinClientExecuteCommentExtensions
{
    extension(DouyinClient client)
    {
        /// <summary>
        /// <para>异步调用 [GET] /aweme/v1/web/comment/list/ 接口。</para>
        /// <para>获取视频一级评论。</para>
        /// </summary>
        public async Task<DouyinCommentResponse> ExecuteCommentPageAsync(
            DouyinCommentRequest request, CancellationToken ct = default)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));
            if (request is null) throw new ArgumentNullException(nameof(request));

            var parameters = new Dictionary<string, string>
            {
                ["aweme_id"] = request.AwemeId,
                ["cursor"] = request.Cursor.ToString(),
                ["count"] = request.Count.ToString(),
                ["item_type"] = "0"
            };

            return await client.SendSignedGetAsync<DouyinCommentResponse>(
                "/aweme/v1/web/comment/list/", parameters, true, ct);
        }

        /// <summary>
        /// <para>异步调用 [GET] /aweme/v1/web/comment/list/reply/ 接口。</para>
        /// <para>获取二级子评论。</para>
        /// </summary>
        public async Task<DouyinSubCommentResponse> ExecuteSubCommentAsync(
            DouyinSubCommentRequest request, CancellationToken ct = default)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));
            if (request is null) throw new ArgumentNullException(nameof(request));

            var parameters = new Dictionary<string, string>
            {
                ["comment_id"] = request.CommentId,
                ["cursor"] = request.Cursor.ToString(),
                ["count"] = request.Count.ToString(),
                ["item_type"] = "0"
            };

            return await client.SendSignedGetAsync<DouyinSubCommentResponse>(
                "/aweme/v1/web/comment/list/reply/", parameters, true, ct);
        }
    }
}
