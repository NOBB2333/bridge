# UnityBridge

UnityBridge 是一个综合性的 .NET 解决方案，旨在连接企业级 AI 应用平台（如 Dify、Sino）与主流社交/内容平台（如 Bilibili、抖音、小红书等）。

它不仅提供了针对 Dify/Sino 的 API 客户端 SDK，还内置了一套强大的、统一架构的爬虫框架，用于数据采集和内容分发。参考DotNetCore.SKIT.FlurlHttpClient.Wechat 风格

## ✨ 功能特性

### 🔌 API 集成
*   **Dify API**: 完整的 Dify 平台管理与交互能力（导入导出应用、Key 管理、工作流发布）。
*   **Sino API**: 企业级 AI 知识库与 Copilot 服务对接。

### 🕷️ 多平台爬虫 SDK
内置统一架构的爬虫客户端，支持以下平台的数据采集与操作：
*   📺 **Bilibili** (B站)
*   🎵 **Douyin** (抖音)
*   🎬 **Kuaishou** (快手)
*   📕 **XiaoHongShu** (小红书)
*   🧣 **Weibo** (微博)
*   🧵 **Tieba** (百度贴吧)
*   🧠 **Zhihu** (知乎)

**爬虫核心能力：**
*   **统一接口**: 所有爬虫继承自 `CrawlerClientBase`，使用一致的调用方式。
*   **自动签名**: 内置 JS 逆向签名服务 (`SignService`)，自动处理各平台的 API 签名验证。
*   **代理池支持**: 内置 `ProxyPoolManager`，支持动态切换代理。
*   **Cookie 管理**: 自动化的 Cookie 提取、持久化与通过。

### 🛠️ 运维与工具
*   **CLI 工具**: 命令行管理工具，用于批量的应用迁移、备份和环境检测。
*   **Web API**: 提供 HTTP 接口服务。
*   **签名服务 (SignServer)**: 可独立部署的 API 签名计算服务（通过 HTTP 暴露），方便非 .NET 语言调用。

---

## 📁 项目结构

```mermaid
graph TD
    Core[UnityBridge.Core]
    CrawlerCore[UnityBridge.Crawler.Core] --> Core
    Db[UnityBridge.Db] --> Core

    %% 爬虫实现
    Bili[Crawler.BiliBili] --> CrawlerCore
    Douyin[Crawler.Douyin] --> CrawlerCore
    Kuaishou[Crawler.Kuaishou] --> CrawlerCore
    Tieba[Crawler.Tieba] --> CrawlerCore
    Weibo[Crawler.Weibo] --> CrawlerCore
    Xhs[Crawler.XiaoHongShu] --> CrawlerCore
    Zhihu[Crawler.Zhihu] --> CrawlerCore
    
    %% API 实现
    Dify[Api.Dify] --> Core
    Sino[Api.Sino] --> Core

    %% 聚合 SDK
    Sdk[UnityBridge.Api.Sdk] --> Core & Dify & Sino & CrawlerCore
    
    %% 应用层
    Main[UnityBridge (CLI)] --> Sdk & Tools
    SignServer[Crawler.SignServer] --> CrawlerCore
```

| 项目 | 说明 |
|------|------|
| **UnityBridge.Core** | **核心底座**。定义了 `ClientOptions`, `CommonClientBase`, `HttpInterceptor` 等基础架构。 |
| **UnityBridge.Crawler.Core** | **爬虫核心**。继承自 Core，增加了 `ProxyPool`, `CookieManager`, `SignService` (JS签名算法) 等爬虫专用功能。 |
| **UnityBridge.Api.Sdk** | **全功能 SDK**。聚合了所有 API 和爬虫能力，推荐第三方开发引用此包。 |
| **UnityBridge.Crawler.*** | 各平台的具体爬虫实现（如 `.BiliBili`, `.Douyin`）。 |
| **UnityBridge.Api.*** | 各 AI 平台的 API 客户端实现（如 `.Dify`, `.Sino`）。 |
| **UnityBridge** | 命令行主程序 (CLI)。 |
| **UnityBridge.Crawler.SignServer** | 独立的签名计算 Web 服务（不含业务逻辑，仅暴露签名接口）。 |
| **UnityBridge.Tools** | 通用工具集。 |
| **UnityBridge.Db** | 数据库访问层。 |

---

## 🚀 快速开始

### 1. 使用 CLI 工具管理 Dify
```bash
# 运行主程序
dotnet run --project UnityBridge/UnityBridge.csproj
```
启动后可选择：应用导入导出、Key 管理、工作流发布等功能。

### 2. 在代码中使用 SDK
引用 `UnityBridge.Api.Sdk` 项目或 DLL。

**初始化爬虫客户端：**
```csharp
var options = new BiliClientOptions 
{ 
    Cookies = "your_cookies_here",
    EnableProxyPool = true
};
var client = new BiliClient(options);
// var info = await client.GetVideoInfoAsync("BV1xx...");
```

**使用 Dify API：**
```csharp
var difyClient = new DifyApiClient(new DifyApiClientOptions { ... });
// await difyClient.ExportAppAsync("app_id");
```

---

## ⚙️ 配置说明

配置文件位于 `UnityBridge/Configuration/` 目录下：

*   `DifyMigration.json`: Dify 平台的连接信息。
*   `Endpoint.json`: 各 API 的端点地址。

---

## 🔒 授权与协议
本项目包含内置的试用期与离线授权机制。
MIT License
