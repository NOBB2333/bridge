using UnityBridge.Crawler.XiaoHongShu.Models;

namespace UnityBridge.Crawler.XiaoHongShu.Extensions;

/// <summary>
/// XhsClient 评论相关扩展方法。
/// </summary>
public static class XhsClientExecuteCommentExtensions
{
    extension(XhsClient client)
    {
        /// <summary>
        /// <para>异步调用 [GET] /api/sns/web/v2/comment/page 接口。</para>
        /// <para>获取笔记一级评论。</para>
        /// </summary>
        public async Task<XhsCommentPageResponse> ExecuteCommentPageAsync(
            XhsCommentPageRequest request, CancellationToken ct = default)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));
            if (request is null) throw new ArgumentNullException(nameof(request));

            // 构建查询参数
            var queryParams = new Dictionary<string, string>
            {
                ["note_id"] = request.NoteId,
                ["cursor"] = request.Cursor,
                ["top_comment_id"] = "",
                ["image_formats"] = "jpg,webp,avif"
            };

            if (!string.IsNullOrEmpty(request.XsecToken))
            {
                queryParams["xsec_token"] = request.XsecToken;
            }

            var queryString = string.Join("&", queryParams.Select(kv => $"{kv.Key}={Uri.EscapeDataString(kv.Value)}"));
            var uri = $"/api/sns/web/v2/comment/page?{queryString}";

            return await client.SendSignedGetAsync<XhsCommentPageResponse>(
                request, ct,
                uri.TrimStart('/').Split('/'));
        }

        /// <summary>
        /// <para>异步调用 [GET] /api/sns/web/v2/comment/sub/page 接口。</para>
        /// <para>获取二级子评论。</para>
        /// </summary>
        public async Task<XhsSubCommentResponse> ExecuteSubCommentAsync(
            XhsSubCommentRequest request, CancellationToken ct = default)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));
            if (request is null) throw new ArgumentNullException(nameof(request));

            var queryParams = new Dictionary<string, string>
            {
                ["note_id"] = request.NoteId,
                ["root_comment_id"] = request.RootCommentId,
                ["num"] = request.Num.ToString(),
                ["cursor"] = request.Cursor
            };

            if (!string.IsNullOrEmpty(request.XsecToken))
            {
                queryParams["xsec_token"] = request.XsecToken;
            }

            var queryString = string.Join("&", queryParams.Select(kv => $"{kv.Key}={Uri.EscapeDataString(kv.Value)}"));
            var uri = $"/api/sns/web/v2/comment/sub/page?{queryString}";

            return await client.SendSignedGetAsync<XhsSubCommentResponse>(
                request, ct,
                uri.TrimStart('/').Split('/'));
        }
    }
}
