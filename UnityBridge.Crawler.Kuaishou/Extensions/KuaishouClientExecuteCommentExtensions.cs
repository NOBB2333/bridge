using UnityBridge.Crawler.Kuaishou.Models;

namespace UnityBridge.Crawler.Kuaishou.Extensions;

/// <summary>
/// KuaishouClient 评论相关扩展方法。
/// </summary>
public static class KuaishouClientExecuteCommentExtensions
{
    extension(KuaishouClient client)
    {
        /// <summary>
        /// <para>异步调用 commentListQuery GraphQL 接口。</para>
        /// <para>获取视频一级评论。</para>
        /// </summary>
        public async Task<KuaishouCommentResponse> ExecuteCommentPageAsync(
            KuaishouCommentRequest request, CancellationToken ct = default)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));
            if (request is null) throw new ArgumentNullException(nameof(request));

            var variables = new
            {
                photoId = request.PhotoId,
                pcursor = request.Pcursor
            };

            return await client.SendGraphQLAsync<KuaishouCommentResponse>(
                "commentListQuery", variables, KuaishouGraphQL.CommentList, ct);
        }

        /// <summary>
        /// <para>异步调用 visionSubCommentList GraphQL 接口。</para>
        /// <para>获取二级子评论。</para>
        /// </summary>
        public async Task<KuaishouSubCommentResponse> ExecuteSubCommentAsync(
            KuaishouSubCommentRequest request, CancellationToken ct = default)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));
            if (request is null) throw new ArgumentNullException(nameof(request));

            var variables = new
            {
                photoId = request.PhotoId,
                pcursor = request.Pcursor,
                rootCommentId = request.RootCommentId
            };

            return await client.SendGraphQLAsync<KuaishouSubCommentResponse>(
                "visionSubCommentList", variables, KuaishouGraphQL.SubCommentList, ct);
        }
    }
}
