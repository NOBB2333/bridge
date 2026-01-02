using UnityBridge.Crawler.Kuaishou.Models;

namespace UnityBridge.Crawler.Kuaishou.Extensions;

/// <summary>
/// KuaishouClient 创作者相关扩展方法。
/// </summary>
public static class KuaishouClientExecuteCreatorExtensions
{
    extension(KuaishouClient client)
    {
        /// <summary>
        /// <para>异步调用 visionProfile GraphQL 接口。</para>
        /// <para>获取创作者信息。</para>
        /// </summary>
        public async Task<KuaishouCreatorProfileResponse> ExecuteCreatorProfileAsync(
            KuaishouCreatorProfileRequest request, CancellationToken ct = default)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));
            if (request is null) throw new ArgumentNullException(nameof(request));

            var variables = new { userId = request.UserId };

            return await client.SendGraphQLAsync<KuaishouCreatorProfileResponse>(
                "visionProfile", variables, KuaishouGraphQL.CreatorProfile, ct);
        }

        /// <summary>
        /// <para>异步调用 visionProfilePhotoList GraphQL 接口。</para>
        /// <para>获取创作者的视频列表。</para>
        /// </summary>
        public async Task<KuaishouCreatorVideosResponse> ExecuteCreatorVideosAsync(
            KuaishouCreatorVideosRequest request, CancellationToken ct = default)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));
            if (request is null) throw new ArgumentNullException(nameof(request));

            var variables = new
            {
                page = "profile",
                pcursor = request.Pcursor,
                userId = request.UserId
            };

            return await client.SendGraphQLAsync<KuaishouCreatorVideosResponse>(
                "visionProfilePhotoList", variables, KuaishouGraphQL.CreatorVideos, ct);
        }
    }
}
