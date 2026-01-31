# UnityBridge.db

交互式数据库 Shell，支持 fish 风格的智能补全提示。
这个项目在peear console正式发布1.0之后要重工。Spectre.Console


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

---

### MySQL

```bash
# 基础连接（会提示输入密码）
db> connect mysql server=localhost;database=mydb;user=root;

# 指定端口
db> connect mysql server=192.168.1.100;port=3306;database=crawler;user=admin;

# 完整连接串（不推荐，密码会显示）
db> connect mysql server=localhost;port=3306;database=mydb;user=root;password=123456;
```

**连接串参数说明:**

| 参数 | 别名 | 说明 | 示例 |
|------|------|------|------|
| `server` | `host`, `data source` | 服务器地址 | `server=127.0.0.1` |
| `port` | - | 端口号，默认 3306 | `port=3306` |
| `database` | `initial catalog` | 数据库名 | `database=mydb` |
| `user` | `uid`, `user id` | 用户名 | `user=root` |
| `password` | `pwd` | 密码（建议省略，会安全提示输入） | `password=123456` |
| `charset` | - | 字符集 | `charset=utf8mb4` |

---

### PostgreSQL

```bash
# 基础连接（会提示输入密码）
db> connect pgsql host=localhost;database=mydb;username=postgres;

# 指定端口
db> connect pgsql host=192.168.1.100;port=5432;database=hemacup;username=postgres;

# 完整连接串
db> connect pgsql host=124.222.232.50;port=30007;database=hemacup;username=postgres;password=123456;
```

**连接串参数说明:**

| 参数 | 别名 | 说明 | 示例 |
|------|------|------|------|
| `host` | `server` | 服务器地址 | `host=127.0.0.1` |
| `port` | - | 端口号，默认 5432 | `port=5432` |
| `database` | - | 数据库名 | `database=mydb` |
| `username` | `user id`, `uid` | 用户名 ⚠️ **不是 `user`** | `username=postgres` |
| `password` | `pwd` | 密码（建议省略，会安全提示输入） | `password=123456` |
| `sslmode` | - | SSL 模式 | `sslmode=prefer` |

> ⚠️ **注意**: PostgreSQL 使用 `host` 和 `username`，与 MySQL 的 `server` 和 `user` 不同！

---

### SQL Server

```bash
# 基础连接（会提示输入密码）
db> connect mssql server=localhost;database=mydb;user id=sa;

# 指定端口（用逗号分隔）
db> connect mssql server=192.168.1.100,1433;database=master;user id=sa;

# 完整连接串
db> connect mssql server=localhost;database=mydb;user id=sa;password=123456;
```

**连接串参数说明:**

| 参数 | 别名 | 说明 | 示例 |
|------|------|------|------|
| `server` | `data source` | 服务器地址（端口用逗号） | `server=127.0.0.1,1433` |
| `database` | `initial catalog` | 数据库名 | `database=master` |
| `user id` | `uid` | 用户名 ⚠️ **不是 `user`** | `user id=sa` |
| `password` | `pwd` | 密码（建议省略，会安全提示输入） | `password=123456` |
| `encrypt` | - | 是否加密 | `encrypt=true` |
| `trustservercertificate` | - | 信任服务器证书 | `trustservercertificate=true` |

> ⚠️ **注意**: SQL Server 端口使用逗号分隔 `server=host,port`，不是 `port=` 参数！

---

## 连接串速查表

| 数据库 | 服务器参数 | 用户名参数 | 端口写法 | 示例 |
|--------|-----------|-----------|---------|------|
| MySQL | `server=` | `user=` | `port=3306` | `server=127.0.0.1;port=3306;database=mydb;user=root;` |
| PostgreSQL | `host=` | `username=` | `port=5432` | `host=127.0.0.1;port=5432;database=mydb;username=postgres;` |
| SQL Server | `server=` | `user id=` | `server=host,1433` | `server=127.0.0.1,1433;database=mydb;user id=sa;` |
| SQLite | 直接写路径 | - | - | `connect ./mydata.db` |

> 💡 **安全提示**: 不要在连接串中写密码，系统会以隐藏方式提示输入。

---

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
