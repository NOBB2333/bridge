using UnityBridge.Crawler.XiaoHongShu.Models;

namespace UnityBridge.Crawler.XiaoHongShu.Extensions;

/// <summary>
/// XhsClient 笔记相关扩展方法。
/// </summary>
public static class XhsClientExecuteNoteExtensions
{
    extension(XhsClient client)
    {
        /// <summary>
        /// <para>异步调用 [POST] /api/sns/web/v1/search/notes 接口。</para>
        /// <para>关键词搜索笔记。</para>
        /// </summary>
        public async Task<XhsNoteSearchResponse> ExecuteNoteSearchAsync(
            XhsNoteSearchRequest request, CancellationToken ct = default)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));
            if (request is null) throw new ArgumentNullException(nameof(request));

            return await client.SendSignedPostAsync<XhsNoteSearchResponse>(
                request, request, ct,
                "api", "sns", "web", "v1", "search", "notes");
        }

        /// <summary>
        /// <para>异步调用 [POST] /api/sns/web/v1/feed 接口。</para>
        /// <para>获取笔记详情。</para>
        /// </summary>
        public async Task<XhsNoteDetailResponse> ExecuteNoteDetailAsync(
            XhsNoteDetailRequest request, CancellationToken ct = default)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));
            if (request is null) throw new ArgumentNullException(nameof(request));

            return await client.SendSignedPostAsync<XhsNoteDetailResponse>(
                request, request, ct,
                "api", "sns", "web", "v1", "feed");
        }

        /// <summary>
        /// <para>异步调用 [POST] /api/sns/web/v1/homefeed 接口。</para>
        /// <para>获取首页推荐流。</para>
        /// </summary>
        public async Task<XhsHomeFeedResponse> ExecuteHomeFeedAsync(
            XhsHomeFeedRequest request, CancellationToken ct = default)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));
            if (request is null) throw new ArgumentNullException(nameof(request));

            return await client.SendSignedPostAsync<XhsHomeFeedResponse>(
                request, request, ct,
                "api", "sns", "web", "v1", "homefeed");
        }
    }
}
