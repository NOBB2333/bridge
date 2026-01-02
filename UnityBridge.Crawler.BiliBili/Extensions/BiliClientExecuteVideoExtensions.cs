using UnityBridge.Crawler.BiliBili.Models;

namespace UnityBridge.Crawler.BiliBili.Extensions;

/// <summary>
/// BiliClient 视频相关扩展方法。
/// </summary>
public static class BiliClientExecuteVideoExtensions
{
    extension(BiliClient client)
    {
        /// <summary>
        /// <para>异步调用 [GET] /x/web-interface/wbi/search/type 接口。</para>
        /// <para>关键词搜索视频。</para>
        /// </summary>
        public async Task<BiliVideoSearchResponse> ExecuteVideoSearchAsync(
            BiliVideoSearchRequest request, CancellationToken ct = default)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));
            if (request is null) throw new ArgumentNullException(nameof(request));

            var parameters = new Dictionary<string, string>
            {
                ["search_type"] = "video",
                ["keyword"] = request.Keyword,
                ["page"] = request.Page.ToString(),
                ["page_size"] = request.PageSize.ToString()
            };

            if (!string.IsNullOrEmpty(request.Order))
            {
                parameters["order"] = request.Order;
            }

            return await client.SendSignedGetAsync<BiliVideoSearchResponse>(
                "/x/web-interface/wbi/search/type", parameters, true, ct);
        }

        /// <summary>
        /// <para>异步调用 [GET] /x/web-interface/view/detail 接口。</para>
        /// <para>获取视频详情。</para>
        /// </summary>
        public async Task<BiliVideoDetailResponse> ExecuteVideoDetailAsync(
            BiliVideoDetailRequest request, CancellationToken ct = default)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));
            if (request is null) throw new ArgumentNullException(nameof(request));

            var parameters = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(request.Aid))
            {
                parameters["aid"] = request.Aid;
            }
            else if (!string.IsNullOrEmpty(request.Bvid))
            {
                parameters["bvid"] = request.Bvid;
            }
            else
            {
                throw new ArgumentException("请提供 Aid 或 Bvid 中的至少一个参数");
            }

            return await client.SendSignedGetAsync<BiliVideoDetailResponse>(
                "/x/web-interface/view/detail", parameters, true, ct);
        }

        /// <summary>
        /// <para>异步调用 [GET] /x/web-interface/wbi/index/top/feed/rcmd 接口。</para>
        /// <para>获取首页推荐流。</para>
        /// </summary>
        public async Task<BiliHomeFeedResponse> ExecuteHomeFeedAsync(
            BiliHomeFeedRequest request, CancellationToken ct = default)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));
            if (request is null) throw new ArgumentNullException(nameof(request));

            var parameters = new Dictionary<string, string>
            {
                ["web_location"] = "1430650",
                ["fresh_type"] = request.FreshType.ToString(),
                ["ps"] = request.PageCount.ToString(),
                ["fresh_idx"] = request.FreshIdx.ToString(),
                ["fresh_idx_1h"] = request.FreshIdx.ToString(),
                ["brush"] = request.FreshIdx.ToString(),
                ["feed_version"] = "v8"
            };

            return await client.SendSignedGetAsync<BiliHomeFeedResponse>(
                "/x/web-interface/wbi/index/top/feed/rcmd", parameters, true, ct);
        }
    }
}
