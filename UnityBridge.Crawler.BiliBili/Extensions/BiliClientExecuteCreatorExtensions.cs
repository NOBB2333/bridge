using UnityBridge.Crawler.BiliBili.Models;

namespace UnityBridge.Crawler.BiliBili.Extensions;

/// <summary>
/// BiliClient UP主相关扩展方法。
/// </summary>
public static class BiliClientExecuteCreatorExtensions
{
    extension(BiliClient client)
    {
        /// <summary>
        /// <para>异步调用 [GET] /x/space/wbi/acc/info 接口。</para>
        /// <para>获取UP主基本信息。</para>
        /// </summary>
        public async Task<BiliResponse<BiliCreator>> ExecuteUpInfoAsync(
            BiliUpInfoRequest request, CancellationToken ct = default)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));
            if (request is null) throw new ArgumentNullException(nameof(request));

            var parameters = new Dictionary<string, string>
            {
                ["mid"] = request.Mid,
                ["token"] = "",
                ["platform"] = "web",
                ["web_location"] = "1550101",
                ["dm_img_list"] = "[]",
                ["dm_img_str"] = "V2ViR0wgMS4wIChPcGVuR0wgRVMgMi4wIENocm9taXVtKQ",
                ["dm_cover_img_str"] = "QU5HTEUgKEFwcGxlLCBBTkdMRSBNZXRhbCBSZW5kZXJlcjogQXBwbGUgTTEsIFVuc3BlY2lmaWVkIFZlcnNpb24pR29vZ2xlIEluYy4gKEFwcGxlKQ"
            };

            return await client.SendSignedGetAsync<BiliResponse<BiliCreator>>(
                "/x/space/wbi/acc/info", parameters, true, ct);
        }

        /// <summary>
        /// <para>异步调用 [GET] /x/space/wbi/arc/search 接口。</para>
        /// <para>获取UP主的视频列表。</para>
        /// </summary>
        public async Task<BiliUpVideosResponse> ExecuteUpVideosAsync(
            BiliUpVideosRequest request, CancellationToken ct = default)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));
            if (request is null) throw new ArgumentNullException(nameof(request));

            var parameters = new Dictionary<string, string>
            {
                ["mid"] = request.Mid,
                ["pn"] = request.Pn.ToString(),
                ["ps"] = request.Ps.ToString(),
                ["order"] = request.Order
            };

            return await client.SendSignedGetAsync<BiliUpVideosResponse>(
                "/x/space/wbi/arc/search", parameters, true, ct);
        }
    }
}
