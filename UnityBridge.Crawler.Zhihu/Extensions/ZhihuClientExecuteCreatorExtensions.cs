using UnityBridge.Crawler.Zhihu.Models;

namespace UnityBridge.Crawler.Zhihu.Extensions;

/// <summary>
/// ZhihuClient 创作者相关扩展方法。
/// </summary>
public static class ZhihuClientExecuteCreatorExtensions
{
    extension(ZhihuClient client)
    {
        /// <summary>
        /// <para>异步调用 [GET] /people/{urlToken} 接口。</para>
        /// <para>获取创作者信息（返回 HTML）。</para>
        /// </summary>
        public async Task<string> ExecuteCreatorProfileHtmlAsync(
            string urlToken, CancellationToken ct = default)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));
            if (string.IsNullOrEmpty(urlToken)) throw new ArgumentNullException(nameof(urlToken));

            return await client.SendGetHtmlAsync($"/people/{urlToken}", null, ct);
        }

        /// <summary>
        /// <para>异步调用 [GET] /api/v4/members/{urlToken}/answers 接口。</para>
        /// <para>获取创作者的回答列表。</para>
        /// </summary>
        public async Task<ZhihuCreatorContentResponse> ExecuteCreatorAnswersAsync(
            ZhihuCreatorContentRequest request, CancellationToken ct = default)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));
            if (request is null) throw new ArgumentNullException(nameof(request));

            var parameters = new Dictionary<string, string>
            {
                ["offset"] = request.Offset.ToString(),
                ["limit"] = request.Limit.ToString(),
                ["order_by"] = "created"
            };

            return await client.SendSignedGetAsync<ZhihuCreatorContentResponse>(
                $"/api/v4/members/{request.UrlToken}/answers", parameters, ct);
        }

        /// <summary>
        /// <para>异步调用 [GET] /api/v4/members/{urlToken}/articles 接口。</para>
        /// <para>获取创作者的文章列表。</para>
        /// </summary>
        public async Task<ZhihuCreatorContentResponse> ExecuteCreatorArticlesAsync(
            ZhihuCreatorContentRequest request, CancellationToken ct = default)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));
            if (request is null) throw new ArgumentNullException(nameof(request));

            var parameters = new Dictionary<string, string>
            {
                ["offset"] = request.Offset.ToString(),
                ["limit"] = request.Limit.ToString(),
                ["order_by"] = "created"
            };

            return await client.SendSignedGetAsync<ZhihuCreatorContentResponse>(
                $"/api/v4/members/{request.UrlToken}/articles", parameters, ct);
        }

        /// <summary>
        /// <para>异步调用 [GET] /api/v4/members/{urlToken}/zvideos 接口。</para>
        /// <para>获取创作者的视频列表。</para>
        /// </summary>
        public async Task<ZhihuCreatorContentResponse> ExecuteCreatorVideosAsync(
            ZhihuCreatorContentRequest request, CancellationToken ct = default)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));
            if (request is null) throw new ArgumentNullException(nameof(request));

            var parameters = new Dictionary<string, string>
            {
                ["offset"] = request.Offset.ToString(),
                ["limit"] = request.Limit.ToString()
            };

            return await client.SendSignedGetAsync<ZhihuCreatorContentResponse>(
                $"/api/v4/members/{request.UrlToken}/zvideos", parameters, ct);
        }
    }
}
