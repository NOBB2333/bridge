using UnityBridge.Crawler.Douyin.Models;

namespace UnityBridge.Crawler.Douyin.Extensions;

/// <summary>
/// DouyinClient 用户相关扩展方法。
/// </summary>
public static class DouyinClientExecuteCreatorExtensions
{
    extension(DouyinClient client)
    {
        /// <summary>
        /// <para>异步调用 [GET] /aweme/v1/web/user/profile/other/ 接口。</para>
        /// <para>获取用户信息。</para>
        /// </summary>
        public async Task<DouyinUserProfileResponse> ExecuteUserProfileAsync(
            DouyinUserProfileRequest request, CancellationToken ct = default)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));
            if (request is null) throw new ArgumentNullException(nameof(request));

            var parameters = new Dictionary<string, string>
            {
                ["sec_user_id"] = request.SecUserId,
                ["publish_video_strategy_type"] = "2",
                ["personal_center_strategy"] = "1"
            };

            return await client.SendSignedGetAsync<DouyinUserProfileResponse>(
                "/aweme/v1/web/user/profile/other/", parameters, true, ct);
        }

        /// <summary>
        /// <para>异步调用 [GET] /aweme/v1/web/aweme/post/ 接口。</para>
        /// <para>获取用户的视频列表。</para>
        /// </summary>
        public async Task<DouyinUserPostsResponse> ExecuteUserPostsAsync(
            DouyinUserPostsRequest request, CancellationToken ct = default)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));
            if (request is null) throw new ArgumentNullException(nameof(request));

            var parameters = new Dictionary<string, string>
            {
                ["sec_user_id"] = request.SecUserId,
                ["count"] = request.Count.ToString(),
                ["max_cursor"] = request.MaxCursor,
                ["locate_query"] = "false",
                ["publish_video_strategy_type"] = "2"
            };

            return await client.SendSignedGetAsync<DouyinUserPostsResponse>(
                "/aweme/v1/web/aweme/post/", parameters, true, ct);
        }
    }
}
