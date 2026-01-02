using UnityBridge.Crawler.Weibo.Models;

namespace UnityBridge.Crawler.Weibo.Extensions;

/// <summary>
/// WeiboClient 创作者相关扩展方法。
/// </summary>
public static class WeiboClientExecuteCreatorExtensions
{
    extension(WeiboClient client)
    {
        /// <summary>
        /// <para>异步调用 [GET] /api/container/getIndex 接口。</para>
        /// <para>获取创作者信息。</para>
        /// </summary>
        public async Task<WeiboCreatorProfileResponse> ExecuteCreatorProfileAsync(
            WeiboCreatorProfileRequest request, CancellationToken ct = default)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));
            if (request is null) throw new ArgumentNullException(nameof(request));

            var parameters = new Dictionary<string, string>
            {
                ["jumpfrom"] = "weibocom",
                ["type"] = "uid",
                ["value"] = request.CreatorId,
                ["containerid"] = $"100505{request.CreatorId}"
            };

            return await client.SendGetJsonAsync<WeiboCreatorProfileResponse>(
                "/api/container/getIndex", parameters, null, ct);
        }

        /// <summary>
        /// <para>异步调用 [GET] /api/container/getIndex 接口。</para>
        /// <para>获取创作者的微博列表。</para>
        /// </summary>
        public async Task<WeiboCreatorNotesResponse> ExecuteCreatorNotesAsync(
            WeiboCreatorNotesRequest request, CancellationToken ct = default)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));
            if (request is null) throw new ArgumentNullException(nameof(request));

            var parameters = new Dictionary<string, string>
            {
                ["jumpfrom"] = "weibocom",
                ["type"] = "uid",
                ["value"] = request.CreatorId,
                ["containerid"] = request.ContainerId,
                ["since_id"] = request.SinceId
            };

            return await client.SendGetJsonAsync<WeiboCreatorNotesResponse>(
                "/api/container/getIndex", parameters, null, ct);
        }
    }
}
