using UnityBridge.Crawler.Kuaishou.Models;

namespace UnityBridge.Crawler.Kuaishou.Extensions;

/// <summary>
/// KuaishouClient 视频相关扩展方法。
/// </summary>
public static class KuaishouClientExecuteVideoExtensions
{
    extension(KuaishouClient client)
    {
        /// <summary>
        /// <para>异步调用 visionSearchPhoto GraphQL 接口。</para>
        /// <para>关键词搜索视频。</para>
        /// </summary>
        public async Task<KuaishouSearchResponse> ExecuteSearchAsync(
            KuaishouSearchRequest request, CancellationToken ct = default)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));
            if (request is null) throw new ArgumentNullException(nameof(request));

            var variables = new
            {
                keyword = request.Keyword,
                pcursor = request.Pcursor,
                page = "search",
                searchSessionId = request.SearchSessionId
            };

            return await client.SendGraphQLAsync<KuaishouSearchResponse>(
                "visionSearchPhoto", variables, KuaishouGraphQL.SearchQuery, ct);
        }

        /// <summary>
        /// <para>异步调用 visionVideoDetail GraphQL 接口。</para>
        /// <para>获取视频详情。</para>
        /// </summary>
        public async Task<KuaishouVideoDetailResponse> ExecuteVideoDetailAsync(
            KuaishouVideoDetailRequest request, CancellationToken ct = default)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));
            if (request is null) throw new ArgumentNullException(nameof(request));

            var variables = new
            {
                photoId = request.PhotoId,
                page = "search"
            };

            return await client.SendGraphQLAsync<KuaishouVideoDetailResponse>(
                "visionVideoDetail", variables, KuaishouGraphQL.VideoDetail, ct);
        }

        /// <summary>
        /// <para>异步调用 brilliantTypeDataQuery GraphQL 接口。</para>
        /// <para>获取首页推荐流。</para>
        /// </summary>
        public async Task<KuaishouHomeFeedResponse> ExecuteHomeFeedAsync(
            KuaishouHomeFeedRequest request, CancellationToken ct = default)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));
            if (request is null) throw new ArgumentNullException(nameof(request));

            var variables = new
            {
                pcursor = request.Pcursor,
                hotChannelId = request.HotChannelId,
                page = "brilliant"
            };

            return await client.SendGraphQLAsync<KuaishouHomeFeedResponse>(
                "brilliantTypeDataQuery", variables, KuaishouGraphQL.HomeFeed, ct);
        }
    }
}
