using UnityBridge.Crawler.Tieba.Models;

namespace UnityBridge.Crawler.Tieba.Extensions;

/// <summary>
/// TiebaClient 创作者相关扩展方法。
/// </summary>
public static class TiebaClientExecuteCreatorExtensions
{
    extension(TiebaClient client)
    {
        /// <summary>
        /// <para>异步调用 [GET] /home/get/getthread 接口。</para>
        /// <para>获取创作者的帖子列表。</para>
        /// </summary>
        public async Task<TiebaCreatorPostsResponse> ExecuteCreatorPostsAsync(
            TiebaCreatorPostsRequest request, CancellationToken ct = default)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));
            if (request is null) throw new ArgumentNullException(nameof(request));

            var timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            var parameters = new Dictionary<string, string>
            {
                ["un"] = request.UserName,
                ["pn"] = request.PageNum.ToString(),
                ["ie"] = "utf-8",
                ["_"] = timestamp.ToString()
            };

            return await client.SendGetJsonAsync<TiebaCreatorPostsResponse>(
                "/home/get/getthread", parameters, ct);
        }
    }
}
