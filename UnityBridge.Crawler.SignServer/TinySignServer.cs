// using System.Net;
// using System.Text.Json;
// using System.Text;
// using UnityBridge.Crawler.Core.SignService;
// using UnityBridge.Crawler.Core.SignService.Bilibili;

// namespace UnityBridge.Crawler.SignServer;

// /// <summary>
// /// 这是一个轻量级的签名服务器实现，使用 System.Net.HttpListener，不依赖 ASP.NET Core。
// /// 这个类仅用于对比和演示 "如何不依赖 Web SDK 实现 HTTP 服务" (Lite 版本)。
// /// </summary>
// public class TinySignServer : IDisposable
// {
//     private readonly HttpListener _listener;
//     private readonly LocalSignClient _signClient;
//     private readonly CancellationTokenSource _cts;

//     public TinySignServer(int port = 8888)
//     {
//         _listener = new HttpListener();
//         _listener.Prefixes.Add($"http://*:{port}/");
//         _signClient = new LocalSignClient();
//         _cts = new CancellationTokenSource();
//     }

//     public void Start()
//     {
//         if (_listener.IsListening) return;
        
//         _listener.Start();
//         Console.WriteLine($"[Tiny] SignServer running on {_listener.Prefixes.First()}");
        
//         // 在后台线程循环处理请求
//         Task.Run(ListenLoop);
//     }

//     public void Stop()
//     {
//         _cts.Cancel();
//         _listener.Stop();
//     }

//     private async Task ListenLoop()
//     {
//         try
//         {
//             while (_listener.IsListening && !_cts.IsCancellationRequested)
//             {
//                 var context = await _listener.GetContextAsync();
//                 _ = ProcessRequestAsync(context); // 不等待，并发处理
//             }
//         }
//         catch (HttpListenerException) when (_cts.IsCancellationRequested) { } // 忽略停止时的异常
//         catch (ObjectDisposedException) { }
//         catch (Exception ex)
//         {
//             Console.WriteLine($"[Tiny] Listener error: {ex.Message}");
//         }
//     }

//     private async Task ProcessRequestAsync(HttpListenerContext context)
//     {
//         try
//         {
//             var req = context.Request;
//             var resp = context.Response;
//             var path = req.Url?.AbsolutePath.ToLowerInvariant() ?? "";
//             var method = req.HttpMethod.ToUpperInvariant();

//             object? responseData = null;

//             // 路由处理
//             if (method == "GET")
//             {
//                 if (path == "/") responseData = new PongResponse("Healthy");
//                 else if (path == "/signsrv/pong") responseData = new PongResponse("pong");
//                 else
//                 {
//                     resp.StatusCode = 404;
//                 }
//             }
//             else if (method == "POST")
//             {
//                 if (path == "/signsrv/v1/xhs/sign")
//                 {
//                     var body = await ReadJsonBodyAsync<XhsSignApiRequest>(req);
//                     if (body != null)
//                     {
//                         var result = await _signClient.GetXhsSignAsync(new XhsSignRequest
//                         {
//                             Uri = body.Uri ?? "",
//                             Data = body.Data,
//                             Cookies = body.Cookies ?? ""
//                         });
//                         responseData = new ApiResponse<XhsSignApiResponse>(0, "success", new XhsSignApiResponse
//                         {
//                             XS = result.XS,
//                             XT = result.XT,
//                             XSCommon = result.XSCommon,
//                             XB3TraceId = result.XB3TraceId
//                         });
//                     }
//                 }
//                 else if (path == "/signsrv/v1/bilibili/sign")
//                 {
//                     var body = await ReadJsonBodyAsync<BiliSignApiRequest>(req);
//                     if (body != null)
//                     {
//                         var result = await _signClient.GetBilibiliSignAsync(new BilibiliSignRequest
//                         {
//                             ReqData = body.ReqData ?? new Dictionary<string, string>(),
//                             Cookies = body.Cookies ?? ""
//                         });
//                         responseData = new ApiResponse<BiliSignApiResponse>(0, "success", new BiliSignApiResponse
//                         {
//                             Wts = result.Wts,
//                             WRid = result.WRid
//                         });
//                     }
//                 }
//                 else if (path == "/signsrv/v1/douyin/sign")
//                 {
//                     var body = await ReadJsonBodyAsync<DouyinSignApiRequest>(req);
//                     if (body != null)
//                     {
//                         var result = await _signClient.GetDouyinSignAsync(new DouyinSignRequest
//                         {
//                             Uri = body.Uri ?? "",
//                             Cookies = body.Cookies ?? ""
//                         });
//                         responseData = new ApiResponse<DouyinSignApiResponse>(0, "success", new DouyinSignApiResponse
//                         {
//                             ABogus = result.ABogus
//                         });
//                     }
//                 }
//                 else if (path == "/signsrv/v1/zhihu/sign")
//                 {
//                     var body = await ReadJsonBodyAsync<ZhihuSignApiRequest>(req);
//                     if (body != null)
//                     {
//                         var result = await _signClient.GetZhihuSignAsync(new ZhihuSignRequest
//                         {
//                             Uri = body.Uri ?? "",
//                             Cookies = body.Cookies ?? ""
//                         });
//                         responseData = new ApiResponse<ZhihuSignApiResponse>(0, "success", new ZhihuSignApiResponse
//                         {
//                             XZse96 = result.XZse96,
//                             XZst81 = result.XZst81
//                         });
//                     }
//                 }
//                 else
//                 {
//                     resp.StatusCode = 404;
//                 }
//             }
//             else
//             {
//                 resp.StatusCode = 405; // Method Not Allowed
//             }

//             // 写入响应
//             if (responseData != null)
//             {
//                 resp.ContentType = "application/json; charset=utf-8";
//                 resp.StatusCode = 200;
//                 // 复用 Program.cs 中定义的 JsonContext
//                 var jsonBytes = JsonSerializer.SerializeToUtf8Bytes(responseData, responseData.GetType(), SignServerJsonContext.Default);
//                 await resp.OutputStream.WriteAsync(jsonBytes);
//             }
            
//             resp.Close();
//         }
//         catch (Exception ex)
//         {
//             Console.WriteLine($"[Tiny] Request error: {ex.Message}");
//             try { context.Response.StatusCode = 500; context.Response.Close(); } catch { }
//         }
//     }

//     private async Task<T?> ReadJsonBodyAsync<T>(HttpListenerRequest request)
//     {
//         if (!request.HasEntityBody) return default;
//         try
//         {
//             // 复用 Program.cs 中定义的 JsonContext
//              return (T?)await JsonSerializer.DeserializeAsync(request.InputStream, typeof(T), SignServerJsonContext.Default);
//         }
//         catch
//         {
//             return default;
//         }
//     }

//     public void Dispose()
//     {
//         Stop();
//         _signClient.Dispose();
//         _cts.Dispose();
//     }
// }
