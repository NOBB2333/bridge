using UnityBridge.Crawler.Douyin.Models;

namespace UnityBridge.Crawler.Douyin.Extensions;

/// <summary>
/// DouyinClient 视频相关扩展方法。
/// </summary>
public static class DouyinClientExecuteAwemeExtensions
{
    extension(DouyinClient client)
    {
        /// <summary>
        /// <para>异步调用 [GET] /aweme/v1/web/general/search/single/ 接口。</para>
        /// <para>关键词搜索视频。</para>
        /// </summary>
        public async Task<DouyinSearchResponse> ExecuteSearchAsync(
            DouyinSearchRequest request, CancellationToken ct = default)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));
            if (request is null) throw new ArgumentNullException(nameof(request));

            var parameters = new Dictionary<string, string>
            {
                ["search_channel"] = request.SearchChannel,
                ["enable_history"] = "1",
                ["keyword"] = request.Keyword,
                ["search_source"] = "tab_search",
                ["query_correct_type"] = "1",
                ["is_filter_search"] = "0",
                ["offset"] = request.Offset.ToString(),
                ["count"] = request.Count.ToString(),
                ["need_filter_settings"] = "1",
                ["list_type"] = "multi"
            };

            if (!string.IsNullOrEmpty(request.SearchId))
            {
                parameters["search_id"] = request.SearchId;
            }

            if (request.SortType != 0 || request.PublishTime != 0)
            {
                var filterJson = JsonSerializer.Serialize(new
                {
                    sort_type = request.SortType.ToString(),
                    publish_time = request.PublishTime.ToString()
                });
                parameters["filter_selected"] = filterJson;
                parameters["is_filter_search"] = "1";
            }

            return await client.SendSignedGetAsync<DouyinSearchResponse>(
                "/aweme/v1/web/general/search/single/", parameters, true, ct);
        }

        /// <summary>
        /// <para>异步调用 [GET] /aweme/v1/web/aweme/detail/ 接口。</para>
        /// <para>获取视频详情。</para>
        /// </summary>
        public async Task<DouyinAwemeDetailResponse> ExecuteAwemeDetailAsync(
            DouyinAwemeDetailRequest request, CancellationToken ct = default)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));
            if (request is null) throw new ArgumentNullException(nameof(request));

            var parameters = new Dictionary<string, string>
            {
                ["aweme_id"] = request.AwemeId
            };

            return await client.SendSignedGetAsync<DouyinAwemeDetailResponse>(
                "/aweme/v1/web/aweme/detail/", parameters, true, ct);
        }

        /// <summary>
        /// <para>异步调用 [POST] /aweme/v1/web/module/feed/ 接口。</para>
        /// <para>获取首页推荐流。</para>
        /// </summary>
        public async Task<DouyinHomeFeedResponse> ExecuteHomeFeedAsync(
            DouyinHomeFeedRequest request, CancellationToken ct = default)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));
            if (request is null) throw new ArgumentNullException(nameof(request));

            var parameters = new Dictionary<string, string>
            {
                ["module_id"] = "3003101",
                ["count"] = request.Count.ToString(),
                ["refresh_index"] = request.RefreshIndex.ToString(),
                ["tag_id"] = request.TagId.ToString()
            };

            return await client.SendSignedGetAsync<DouyinHomeFeedResponse>(
                "/aweme/v1/web/module/feed/", parameters, false, ct);
        }
    }
}
