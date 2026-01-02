using UnityBridge.Crawler.BiliBili.Models;

namespace UnityBridge.Crawler.BiliBili.Extensions;

/// <summary>
/// BiliClient 评论相关扩展方法。
/// </summary>
public static class BiliClientExecuteCommentExtensions
{
    extension(BiliClient client)
    {
        /// <summary>
        /// <para>异步调用 [GET] /x/v2/reply/wbi/main 接口。</para>
        /// <para>获取视频一级评论。</para>
        /// </summary>
        public async Task<BiliCommentResponse> ExecuteCommentPageAsync(
            BiliCommentRequest request, CancellationToken ct = default)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));
            if (request is null) throw new ArgumentNullException(nameof(request));

            var parameters = new Dictionary<string, string>
            {
                ["oid"] = request.VideoId,
                ["mode"] = request.Mode.ToString(),
                ["type"] = "1",
                ["ps"] = "20",
                ["next"] = request.Next.ToString()
            };

            return await client.SendSignedGetAsync<BiliCommentResponse>(
                "/x/v2/reply/wbi/main", parameters, true, ct);
        }

        /// <summary>
        /// <para>异步调用 [GET] /x/v2/reply/reply 接口。</para>
        /// <para>获取二级子评论。</para>
        /// </summary>
        public async Task<BiliSubCommentResponse> ExecuteSubCommentAsync(
            BiliSubCommentRequest request, CancellationToken ct = default)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));
            if (request is null) throw new ArgumentNullException(nameof(request));

            var parameters = new Dictionary<string, string>
            {
                ["oid"] = request.VideoId,
                ["mode"] = request.Mode.ToString(),
                ["type"] = "1",
                ["ps"] = request.Ps.ToString(),
                ["pn"] = request.Pn.ToString(),
                ["root"] = request.RootCommentId
            };

            return await client.SendSignedGetAsync<BiliSubCommentResponse>(
                "/x/v2/reply/reply", parameters, true, ct);
        }
    }
}
