using UnityBridge.Crawler.Zhihu.Models;

namespace UnityBridge.Crawler.Zhihu.Extensions;

/// <summary>
/// ZhihuClient 内容相关扩展方法。
/// </summary>
public static class ZhihuClientExecuteContentExtensions
{
    extension(ZhihuClient client)
    {
        /// <summary>
        /// <para>异步调用 [GET] /api/v4/search_v3 接口。</para>
        /// <para>关键词搜索内容。</para>
        /// </summary>
        public async Task<ZhihuSearchResponse> ExecuteSearchAsync(
            ZhihuSearchRequest request, CancellationToken ct = default)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));
            if (request is null) throw new ArgumentNullException(nameof(request));

            var parameters = new Dictionary<string, string>
            {
                ["gk_version"] = "gz-gaokao",
                ["t"] = "general",
                ["q"] = request.Keyword,
                ["correction"] = "1",
                ["offset"] = ((request.Page - 1) * request.PageSize).ToString(),
                ["limit"] = request.PageSize.ToString(),
                ["filter_fields"] = "",
                ["lc_idx"] = ((request.Page - 1) * request.PageSize).ToString(),
                ["show_all_topics"] = "0",
                ["search_source"] = "Filter"
            };

            if (!string.IsNullOrEmpty(request.TimeInterval))
                parameters["time_interval"] = request.TimeInterval;

            if (request.Sort != "default")
                parameters["sort"] = request.Sort;

            if (!string.IsNullOrEmpty(request.Vertical))
                parameters["vertical"] = request.Vertical;

            return await client.SendSignedGetAsync<ZhihuSearchResponse>(
                "/api/v4/search_v3", parameters, ct);
        }

        /// <summary>
        /// <para>异步调用 [GET] /question/{questionId}/answer/{answerId} 接口。</para>
        /// <para>获取回答详情（返回 HTML）。</para>
        /// </summary>
        public async Task<string> ExecuteAnswerDetailHtmlAsync(
            ZhihuAnswerDetailRequest request, CancellationToken ct = default)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));
            if (request is null) throw new ArgumentNullException(nameof(request));

            return await client.SendGetHtmlAsync(
                $"/question/{request.QuestionId}/answer/{request.AnswerId}", null, ct);
        }

        /// <summary>
        /// <para>异步调用 [GET] /p/{articleId} 接口。</para>
        /// <para>获取文章详情（返回 HTML）。</para>
        /// </summary>
        public async Task<string> ExecuteArticleDetailHtmlAsync(
            ZhihuArticleDetailRequest request, CancellationToken ct = default)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));
            if (request is null) throw new ArgumentNullException(nameof(request));

            return await client.SendGetHtmlAsync(
                $"/p/{request.ArticleId}", ZhihuEndpoints.ZHUANLAN, ct);
        }

        /// <summary>
        /// <para>异步调用 [GET] /zvideo/{videoId} 接口。</para>
        /// <para>获取视频详情（返回 HTML）。</para>
        /// </summary>
        public async Task<string> ExecuteVideoDetailHtmlAsync(
            ZhihuVideoDetailRequest request, CancellationToken ct = default)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));
            if (request is null) throw new ArgumentNullException(nameof(request));

            return await client.SendGetHtmlAsync(
                $"/zvideo/{request.VideoId}", null, ct);
        }

        /// <summary>
        /// <para>异步调用 [GET] /api/v4/questions/{questionId}/feeds 接口。</para>
        /// <para>获取问题的回答列表。</para>
        /// </summary>
        public async Task<ZhihuQuestionAnswersResponse> ExecuteQuestionAnswersAsync(
            ZhihuQuestionAnswersRequest request, CancellationToken ct = default)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));
            if (request is null) throw new ArgumentNullException(nameof(request));

            var parameters = new Dictionary<string, string>
            {
                ["cursor"] = request.Cursor,
                ["limit"] = request.Limit.ToString(),
                ["offset"] = request.Offset.ToString(),
                ["order"] = request.Order,
                ["platform"] = "desktop"
            };

            return await client.SendSignedGetAsync<ZhihuQuestionAnswersResponse>(
                $"/api/v4/questions/{request.QuestionId}/feeds", parameters, ct);
        }
    }
}
