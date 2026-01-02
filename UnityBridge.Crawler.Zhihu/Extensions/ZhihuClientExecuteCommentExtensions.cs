using UnityBridge.Crawler.Zhihu.Models;

namespace UnityBridge.Crawler.Zhihu.Extensions;

/// <summary>
/// ZhihuClient 评论相关扩展方法。
/// </summary>
public static class ZhihuClientExecuteCommentExtensions
{
    extension(ZhihuClient client)
    {
        /// <summary>
        /// <para>异步调用 [GET] /api/v4/comment_v5/{contentType}s/{contentId}/root_comment 接口。</para>
        /// <para>获取一级评论。</para>
        /// </summary>
        public async Task<ZhihuCommentResponse> ExecuteCommentPageAsync(
            ZhihuCommentRequest request, CancellationToken ct = default)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));
            if (request is null) throw new ArgumentNullException(nameof(request));

            var parameters = new Dictionary<string, string>
            {
                ["order"] = request.OrderBy,
                ["offset"] = request.Offset,
                ["limit"] = request.Limit.ToString()
            };

            return await client.SendSignedGetAsync<ZhihuCommentResponse>(
                $"/api/v4/comment_v5/{request.ContentType}s/{request.ContentId}/root_comment", parameters, ct);
        }

        /// <summary>
        /// <para>异步调用 [GET] /api/v4/comment_v5/comment/{rootCommentId}/child_comment 接口。</para>
        /// <para>获取子评论。</para>
        /// </summary>
        public async Task<ZhihuSubCommentResponse> ExecuteSubCommentAsync(
            ZhihuSubCommentRequest request, CancellationToken ct = default)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));
            if (request is null) throw new ArgumentNullException(nameof(request));

            var parameters = new Dictionary<string, string>
            {
                ["order"] = request.OrderBy,
                ["offset"] = request.Offset,
                ["limit"] = request.Limit.ToString()
            };

            return await client.SendSignedGetAsync<ZhihuSubCommentResponse>(
                $"/api/v4/comment_v5/comment/{request.RootCommentId}/child_comment", parameters, ct);
        }
    }
}
