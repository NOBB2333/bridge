using UnityBridge.Crawler.Weibo.Models;

namespace UnityBridge.Crawler.Weibo.Extensions;

/// <summary>
/// WeiboClient 评论相关扩展方法。
/// </summary>
public static class WeiboClientExecuteCommentExtensions
{
    extension(WeiboClient client)
    {
        /// <summary>
        /// <para>异步调用 [GET] /comments/hotflow 接口。</para>
        /// <para>获取微博评论。</para>
        /// </summary>
        public async Task<WeiboCommentResponse> ExecuteCommentPageAsync(
            WeiboCommentRequest request, CancellationToken ct = default)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));
            if (request is null) throw new ArgumentNullException(nameof(request));

            var parameters = new Dictionary<string, string>
            {
                ["id"] = request.NoteId,
                ["mid"] = request.NoteId,
                ["max_id_type"] = request.MaxIdType.ToString()
            };

            if (request.MaxId > 0)
            {
                parameters["max_id"] = request.MaxId.ToString();
            }

            var extraHeaders = new Dictionary<string, string>
            {
                ["Referer"] = $"https://m.weibo.cn/detail/{request.NoteId}"
            };

            return await client.SendGetJsonAsync<WeiboCommentResponse>(
                "/comments/hotflow", parameters, extraHeaders, ct);
        }
    }
}
