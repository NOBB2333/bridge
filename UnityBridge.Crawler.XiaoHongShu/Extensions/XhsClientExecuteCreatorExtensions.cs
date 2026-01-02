using UnityBridge.Crawler.XiaoHongShu.Models;

namespace UnityBridge.Crawler.XiaoHongShu.Extensions;

/// <summary>
/// XhsClient 创作者相关扩展方法。
/// </summary>
public static class XhsClientExecuteCreatorExtensions
{
    extension(XhsClient client)
    {
        /// <summary>
        /// <para>异步调用 [GET] /api/sns/web/v1/user_posted 接口。</para>
        /// <para>获取创作者的笔记列表。</para>
        /// </summary>
        public async Task<XhsCreatorNotesResponse> ExecuteCreatorNotesAsync(
            XhsCreatorNotesRequest request, CancellationToken ct = default)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));
            if (request is null) throw new ArgumentNullException(nameof(request));

            var queryParams = new Dictionary<string, string>
            {
                ["user_id"] = request.UserId,
                ["cursor"] = request.Cursor,
                ["num"] = request.Num.ToString(),
                ["image_formats"] = "jpg,webp,avif",
                ["xsec_source"] = request.XsecSource
            };

            if (!string.IsNullOrEmpty(request.XsecToken))
            {
                queryParams["xsec_token"] = request.XsecToken;
            }

            var queryString = string.Join("&", queryParams.Select(kv => $"{kv.Key}={Uri.EscapeDataString(kv.Value)}"));
            var uri = $"/api/sns/web/v1/user_posted?{queryString}";

            return await client.SendSignedGetAsync<XhsCreatorNotesResponse>(
                request, ct,
                uri.TrimStart('/').Split('/'));
        }
    }
}
