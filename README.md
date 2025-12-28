# UnityBridge

UnityBridge 是一个针对 Dify / 企业内部 AI 应用平台的 **命令行桥接工具**，用于在本地便捷地完成应用的导入导出、API Key 管理、工作流发布、编码检测等常见运维 / 开发工作，并内置试用期与离线授权机制。

## ✨ 功能特性

| 功能 | 说明 |
|------|------|
| 📥 下载（导出）应用 | 从 Dify 控制台导出指定应用配置到本地文件 |
| 📤 上传（导入）应用 | 将本地的应用 YAML 文件上传到平台 |
| 🔑 管理 API Key | 生成、查看、清理应用的 API Key |
| 📊 获取应用详情 | 批量获取应用详情并导出综合表 (CSV) |
| 🚀 工作流发布管理 | 批量发布 Workflow/Chatflow 并创建工具 |
| 📈 分析应用关系 | 分析 Chat 应用与工作流的依赖关系图 |
| 📦 打包应用 | 打包 Chat 应用及其所需工作流 |
| 🔍 检测编码 | 批量检测文件或目录的文本编码 |
| 🔒 离线授权 | 内置 30 天试用期，支持机器码离线激活 |

---

## 🖥️ 环境要求

- **.NET 10 SDK**（或兼容的 Runtime）
- 可访问目标 Dify 平台的网络环境

---

## 🚀 快速开始

### 本地运行

```bash
# 克隆仓库
git clone https://github.com/NOBB2333/bridge.git
cd bridge

# 运行程序
dotnet run --project UnityBridge/UnityBridge.csproj
```

### Docker 运行

```bash
docker compose build
docker compose run --rm unitybridge
```

---

## 📋 菜单说明

启动后会看到以下菜单：

```
请选择操作:
1) 下载 (导出) 应用
2) 上传 (导入) 应用
3) 管理 API Key
4) 获取App应用详情综合表
5) 工作流发布管理
6) 一次性测试 WoWeb 项目流程
7) 分析 Chat 应用与工作流关系图
8) 打包 Chat 应用及其所需工作流
9) 检测文件/文件夹编码
10) 查看试用期信息
11) 测试本机授权 (显示机器码并生成测试激活 key)
```

---

## ⚙️ 配置说明

连接地址、请求头（含 Token / Cookie 等）统一保存在 `UnityBridge/Configuration/*.json` 中：

- `DifyMigration.json` - Dify 平台连接配置
- `Endpoint.json` - API 端点配置

使用前请根据自己的环境修改这些 JSON 文件。

---

## 📁 项目结构

```
UnityBridge/
├── UnityBridge/                    # 主程序
│   ├── Configuration/              # 配置文件
│   ├── Platforms.Dify/             # Dify 平台命令
│   ├── Helpers/                    # 辅助类
│   └── Program.cs                  # 入口点
├── UnityBridge.Api.Dify/           # Dify API 客户端库
├── UnityBridge.Core/               # 核心通用库
└── UnityBridge.Shared/             # 共享工具类
```

---

## 🔒 试用期与授权

- **首次运行**：自动初始化 30 天试用期
- **每次启动**：显示剩余试用时间
- **离线激活**：通过菜单查看机器码，由发行方生成激活 key

> 机器码由硬件标识组合生成，可通过菜单 11 查看。

---

## 📄 License

MIT License
