# UnityBridge.db

交互式数据库 Shell，支持 fish 风格的智能补全提示。

## 快速开始

```bash
cd UnityBridge.db
dotnet run
```

## 连接数据库

### SQLite

```bash
db> connect crawler.db
db> connect /path/to/database.db
```

### MySQL

```bash
# 基础连接（会提示输入密码）
db> connect mysql server=localhost;database=mydb;user=root;

# 完整连接串
db> connect mysql server=192.168.1.100;port=3306;database=crawler;user=admin;
```

> **安全提示**：密码不需要在命令中输入，系统会以隐藏方式提示输入。

### PostgreSQL

```bash
db> connect pgsql host=localhost;database=mydb;username=postgres;
```

### SQL Server

```bash
db> connect mssql server=localhost;database=mydb;user id=sa;
```

## 命令列表

| 命令 | 描述 |
|------|------|
| `connect <path>` | 连接 SQLite |
| `connect mysql <connstr>` | 连接 MySQL |
| `connect pgsql <connstr>` | 连接 PostgreSQL |
| `connect mssql <connstr>` | 连接 SQL Server |
| `show tables` | 显示所有表 |
| `show databases` | 显示所有数据库 (MySQL) |
| `describe <table>` | 显示表结构 |
| `select ...` | 执行查询 |
| `insert/update/delete ...` | 执行 SQL 语句 |
| `use <database>` | 切换数据库 |
| `history` | 命令历史 |
| `clear` | 清屏 |
| `exit` | 退出 |

## 智能提示

- 输入时**自动显示灰色补全建议**
- 按 **→ (右箭头)** 或 **Tab** 接受建议
- 按 **↑/↓** 浏览历史命令

## 依赖

- .NET 10.0+
- PrettyPrompt (fish 风格补全)
- Spectre.Console (格式化输出)
- SqlSugar (数据库访问)
