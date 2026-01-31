namespace UnityBridge.db;

using PrettyPrompt;
using PrettyPrompt.Completion;
using PrettyPrompt.Consoles;
using PrettyPrompt.Documents;
using PrettyPrompt.Highlighting;

/// <summary>
/// 数据库交互式 Shell 主类。
/// 使用 PrettyPrompt 实现 fish 风格的内联补全提示。
/// </summary>
public class DbShell
{
    private SqlSugarClient? _db;
    private readonly List<string> _history = new();
    private DatabaseSchema? _schema;
    private string? _connectionString;
    private DbType _dbType = DbType.Sqlite;
    
    /// <summary>
    /// 运行交互式 Shell。
    /// </summary>
    public async Task RunAsync()
    {
        // 创建 PrettyPrompt 配置
        var promptConfig = new PromptConfiguration(
            prompt: new FormattedString("db> ", new FormatSpan(0, 3, AnsiColor.Green))
        );
        
        // 创建 Prompt 实例
        await using var prompt = new Prompt(
            callbacks: new SqlPromptCallbacks(this),
            configuration: promptConfig
        );
        
        while (true)
        {
            try
            {
                var response = await prompt.ReadLineAsync();
                
                if (!response.IsSuccess)
                {
                    // Ctrl+C
                    Console.WriteLine();
                    continue;
                }
                
                var input = response.Text;
                
                if (string.IsNullOrWhiteSpace(input))
                    continue;
                
                _history.Add(input);
                
                var result = await ProcessCommandAsync(input.Trim());
                if (!result)
                    break;
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]错误: {ex.Message}[/]");
            }
        }
    }

    /// <summary>
    /// 处理命令，返回 false 表示退出。
    /// </summary>
    private async Task<bool> ProcessCommandAsync(string command)
    {
        var parts = command.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 0)
            return true;

        var cmd = parts[0].ToLowerInvariant();

        return cmd switch
        {
            "exit" or "quit" or "q" => false,
            "help" or "?" => ShowHelp(),
            "clear" or "cls" => ClearScreen(),
            "connect" => await ConnectAsync(parts.Skip(1).ToArray()),
            "show" when parts.Length > 1 && parts[1].ToLowerInvariant() == "tables" => ShowTables(),
            "show" when parts.Length > 1 && parts[1].ToLowerInvariant() == "databases" => await ExecuteQueryAsync("show databases"),
            "describe" or "desc" when parts.Length > 1 => DescribeTable(parts[1]),
            "history" => ShowHistory(),
            _ when IsSqlQuery(command) => await ExecuteQueryAsync(command),
            _ => UnknownCommand(cmd)
        };
    }

    private bool ShowHelp()
    {
        AnsiConsole.MarkupLine("\n[bold cyan]═══ 数据库连接帮助 ═══[/]\n");
        
        // 命令列表
        var cmdTable = new Table()
            .AddColumn("[cyan]命令[/]")
            .AddColumn("[cyan]描述[/]");
        
        cmdTable.AddRow("connect <path>", "连接 SQLite 数据库");
        cmdTable.AddRow("connect mysql <connstr>", "连接 MySQL 数据库");
        cmdTable.AddRow("connect pgsql <connstr>", "连接 PostgreSQL 数据库");
        cmdTable.AddRow("connect mssql <connstr>", "连接 SQL Server 数据库");
        cmdTable.AddRow("show tables", "显示所有表");
        cmdTable.AddRow("show databases", "显示所有数据库");
        cmdTable.AddRow("use <database>", "切换数据库");
        cmdTable.AddRow("describe <table>", "显示表结构");
        cmdTable.AddRow("select/insert/...", "执行 SQL 语句");
        cmdTable.AddRow("history", "显示命令历史");
        cmdTable.AddRow("clear", "清屏");
        cmdTable.AddRow("exit", "退出");
        
        AnsiConsole.Write(cmdTable);
        
        // 连接串速查表
        AnsiConsole.MarkupLine("\n[bold yellow]═══ 连接串速查表 ═══[/]\n");
        
        var connTable = new Table()
            .AddColumn("[yellow]数据库[/]")
            .AddColumn("[yellow]服务器[/]")
            .AddColumn("[yellow]用户名[/]")
            .AddColumn("[yellow]端口写法[/]");
        
        connTable.AddRow("MySQL", "[green]server=[/]", "[green]user=[/]", "port=3306");
        connTable.AddRow("PostgreSQL", "[green]host=[/]", "[green]username=[/]", "port=5432");
        connTable.AddRow("SQL Server", "[green]server=[/]", "[green]user id=[/]", "server=host[red],[/]1433");
        connTable.AddRow("SQLite", "直接写路径", "-", "-");
        
        AnsiConsole.Write(connTable);
        
        // 示例
        AnsiConsole.MarkupLine("\n[bold green]═══ 连接示例 ═══[/]\n");
        AnsiConsole.MarkupLine("[grey]# SQLite[/]");
        AnsiConsole.MarkupLine("  connect ./mydata.db");
        AnsiConsole.MarkupLine("");
        AnsiConsole.MarkupLine("[grey]# MySQL[/]");
        AnsiConsole.MarkupLine("  connect mysql [green]server[/]=127.0.0.1;port=3306;database=mydb;[green]user[/]=root;");
        AnsiConsole.MarkupLine("");
        AnsiConsole.MarkupLine("[grey]# PostgreSQL[/]  [yellow]⚠ 注意用 host 和 username[/]");
        AnsiConsole.MarkupLine("  connect pgsql [green]host[/]=127.0.0.1;port=5432;database=mydb;[green]username[/]=postgres;");
        AnsiConsole.MarkupLine("");
        AnsiConsole.MarkupLine("[grey]# SQL Server[/]  [yellow]⚠ 端口用逗号分隔[/]");
        AnsiConsole.MarkupLine("  connect mssql [green]server[/]=127.0.0.1[red],[/]1433;database=mydb;[green]user id[/]=sa;");
        
        AnsiConsole.MarkupLine("\n[grey]💡 密码不需要写在连接串中，系统会安全提示输入[/]");
        AnsiConsole.MarkupLine("[grey]   Tab/→ 接受补全建议, ↑/↓ 历史导航[/]\n");
        
        return true;
    }

    private bool ClearScreen()
    {
        AnsiConsole.Clear();
        return true;
    }

    private async Task<bool> ConnectAsync(string[] args)
    {
        if (args.Length == 0)
        {
            AnsiConsole.MarkupLine("[yellow]用法:[/]");
            AnsiConsole.MarkupLine("  connect <path>              - 连接 SQLite");
            AnsiConsole.MarkupLine("  connect mysql <connstr>     - 连接 MySQL");
            AnsiConsole.MarkupLine("  connect pgsql <connstr>     - 连接 PostgreSQL");
            AnsiConsole.MarkupLine("  connect mssql <connstr>     - 连接 SQL Server");
            AnsiConsole.MarkupLine("[grey]提示: 密码会以隐藏方式输入[/]");
            return true;
        }

        try
        {
            StaticConfig.EnableAot = true;
            
            var dbType = args[0].ToLowerInvariant();
            
            if (dbType == "mysql" || dbType == "pgsql" || dbType == "mssql")
            {
                var connStr = args.Length > 1 ? string.Join(" ", args.Skip(1)) : "";
                
                // 检查连接串中是否已有密码
                if (!connStr.Contains("password=", StringComparison.OrdinalIgnoreCase) &&
                    !connStr.Contains("pwd=", StringComparison.OrdinalIgnoreCase))
                {
                    // 安全方式提示输入密码
                    var password = AnsiConsole.Prompt(
                        new TextPrompt<string>("[grey]请输入密码:[/]")
                            .PromptStyle("grey")
                            .Secret());
                    
                    connStr = connStr.TrimEnd(';') + $";password={password};";
                }
                
                var sqlDbType = dbType switch
                {
                    "mysql" => DbType.MySql,
                    "pgsql" => DbType.PostgreSQL,
                    "mssql" => DbType.SqlServer,
                    _ => DbType.MySql
                };
                
                // 保存连接信息用于切换数据库
                _connectionString = connStr;
                _dbType = sqlDbType;
                
                _db = new SqlSugarClient(new ConnectionConfig
                {
                    ConnectionString = connStr,
                    DbType = sqlDbType,
                    IsAutoCloseConnection = true,
                    InitKeyType = InitKeyType.Attribute
                });
                
                var dbName = dbType switch
                {
                    "mysql" => "MySQL",
                    "pgsql" => "PostgreSQL",
                    "mssql" => "SQL Server",
                    _ => dbType
                };
                AnsiConsole.MarkupLine($"[green]✓ 已连接 {dbName} 数据库[/]");
            }
            else
            {
                // SQLite
                var path = args[0];
                if (!File.Exists(path))
                {
                    AnsiConsole.MarkupLine($"[red]文件不存在: {path}[/]");
                    return true;
                }
                
                _connectionString = $"Data Source={path};";
                _dbType = DbType.Sqlite;
                
                _db = new SqlSugarClient(new ConnectionConfig
                {
                    ConnectionString = _connectionString,
                    DbType = DbType.Sqlite,
                    IsAutoCloseConnection = true,
                    InitKeyType = InitKeyType.Attribute
                });
                AnsiConsole.MarkupLine($"[green]✓ 已连接 SQLite: {path}[/]");
            }

            // 加载 Schema 用于智能提示
            await LoadSchemaAsync();
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]连接失败: {ex.Message}[/]");
        }

        return true;
    }

    /// <summary>
    /// 切换数据库 - 更新连接字符串并重新创建连接。
    /// </summary>
    private async Task SwitchDatabaseAsync(string databaseName)
    {
        if (_db == null || string.IsNullOrEmpty(_connectionString))
        {
            AnsiConsole.MarkupLine("[yellow]请先使用 'connect' 命令连接数据库[/]");
            return;
        }
        
        try
        {
            // 更新连接字符串中的 database 参数
            var newConnStr = UpdateConnectionStringDatabase(_connectionString, databaseName);
            
            // 关闭旧连接
            _db.Dispose();
            
            // 创建新连接
            _db = new SqlSugarClient(new ConnectionConfig
            {
                ConnectionString = newConnStr,
                DbType = _dbType,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute
            });
            
            // 更新保存的连接字符串
            _connectionString = newConnStr;
            
            AnsiConsole.MarkupLine($"[green]✓ 已切换到数据库: {databaseName}[/]");
            
            // 重新加载 Schema
            await LoadSchemaAsync();
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]切换数据库失败: {ex.Message}[/]");
        }
    }
    
    /// <summary>
    /// 更新连接字符串中的 database 参数。
    /// </summary>
    private string UpdateConnectionStringDatabase(string connStr, string database)
    {
        // 移除现有的 database 参数
        var parts = connStr.Split(';', StringSplitOptions.RemoveEmptyEntries)
            .Where(p => !p.Trim().StartsWith("database=", StringComparison.OrdinalIgnoreCase) &&
                       !p.Trim().StartsWith("initial catalog=", StringComparison.OrdinalIgnoreCase))
            .ToList();
        
        // 添加新的 database 参数
        parts.Add($"database={database}");
        
        return string.Join(";", parts) + ";";
    }

    private async Task LoadSchemaAsync()
    {
        if (_db == null) return;
        
        _schema = new DatabaseSchema();
        
        try
        {
            // 尝试获取数据库列表 (MySQL/PostgreSQL/SQL Server)
            try
            {
                var databases = _db.Ado.GetDataTable("show databases");
                foreach (System.Data.DataRow row in databases.Rows)
                {
                    _schema.Databases.Add(row[0]?.ToString() ?? "");
                }
            }
            catch
            {
                // SQLite 不支持 show databases
            }
            
            // 获取表列表
            var tables = _db.DbMaintenance.GetTableInfoList();
            foreach (var table in tables)
            {
                var tableSchema = new TableSchema { Name = table.Name };
                
                try
                {
                    var columns = _db.DbMaintenance.GetColumnInfosByTableName(table.Name);
                    foreach (var col in columns)
                    {
                        tableSchema.Columns.Add(new ColumnSchema
                        {
                            Name = col.DbColumnName,
                            Type = col.DataType,
                            IsPrimaryKey = col.IsPrimarykey,
                            IsNullable = col.IsNullable
                        });
                    }
                }
                catch
                {
                    // 忽略列加载错误
                }
                
                _schema.Tables.Add(tableSchema);
            }
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[grey]Schema 加载警告: {ex.Message}[/]");
        }
        
        var dbCount = _schema.Databases.Count > 0 ? $"，{_schema.Databases.Count} 个数据库" : "";
        AnsiConsole.MarkupLine($"[grey]已加载 {_schema.Tables.Count} 个表的 Schema{dbCount}[/]");
    }

    private bool ShowTables()
    {
        if (_db == null)
        {
            AnsiConsole.MarkupLine("[yellow]请先使用 'connect' 命令连接数据库[/]");
            return true;
        }

        var tables = _db.DbMaintenance.GetTableInfoList();
        
        var table = new Table()
            .AddColumn("[cyan]表名[/]")
            .AddColumn("[cyan]描述[/]");
        
        foreach (var t in tables)
        {
            table.AddRow(t.Name, t.Description ?? "");
        }
        
        AnsiConsole.Write(table);
        return true;
    }

    private bool DescribeTable(string tableName)
    {
        if (_db == null)
        {
            AnsiConsole.MarkupLine("[yellow]请先使用 'connect' 命令连接数据库[/]");
            return true;
        }

        var columns = _db.DbMaintenance.GetColumnInfosByTableName(tableName);
        if (columns.Count == 0)
        {
            AnsiConsole.MarkupLine($"[yellow]表 '{tableName}' 不存在[/]");
            return true;
        }
        
        var table = new Table()
            .AddColumn("[cyan]列名[/]")
            .AddColumn("[cyan]类型[/]")
            .AddColumn("[cyan]主键[/]")
            .AddColumn("[cyan]可空[/]");
        
        foreach (var col in columns)
        {
            table.AddRow(
                col.DbColumnName,
                col.DataType,
                col.IsPrimarykey ? "[green]✓[/]" : "",
                col.IsNullable ? "[grey]✓[/]" : "[red]✗[/]"
            );
        }
        
        AnsiConsole.Write(table);
        return true;
    }

    private bool ShowHistory()
    {
        for (int i = 0; i < _history.Count; i++)
        {
            AnsiConsole.MarkupLine($"[grey]{i + 1}.[/] {_history[i]}");
        }
        return true;
    }

    private bool IsSqlQuery(string command)
    {
        var lower = command.ToLowerInvariant().TrimStart();
        // 支持常见的 SQL 查询语句
        return lower.StartsWith("select") || 
               lower.StartsWith("show") ||
               lower.StartsWith("pragma") ||
               lower.StartsWith("insert") ||
               lower.StartsWith("update") ||
               lower.StartsWith("delete") ||
               lower.StartsWith("create") ||
               lower.StartsWith("drop") ||
               lower.StartsWith("alter") ||
               lower.StartsWith("use");
    }

    private async Task<bool> ExecuteQueryAsync(string sql)
    {
        if (_db == null)
        {
            AnsiConsole.MarkupLine("[yellow]请先使用 'connect' 命令连接数据库[/]");
            return true;
        }

        var lower = sql.ToLowerInvariant().TrimStart();

        try
        {
            // 处理 USE 命令 - 需要重新创建连接
            if (lower.StartsWith("use "))
            {
                var dbName = sql.Substring(4).Trim().TrimEnd(';');
                await SwitchDatabaseAsync(dbName);
                return true;
            }
            
            // 判断是否是非查询语句 (不返回结果集)
            if (lower.StartsWith("insert") || 
                lower.StartsWith("update") || 
                lower.StartsWith("delete") ||
                lower.StartsWith("create") ||
                lower.StartsWith("drop") ||
                lower.StartsWith("alter") ||
                lower.StartsWith("truncate"))
            {
                var affected = _db.Ado.ExecuteCommand(sql);
                AnsiConsole.MarkupLine($"[green]✓ 执行成功，影响 {affected} 行[/]");
                return true;
            }
            
            // 查询语句
            var dt = _db.Ado.GetDataTable(sql);
            
            if (dt.Rows.Count == 0)
            {
                AnsiConsole.MarkupLine("[grey]查询结果为空[/]");
                return true;
            }

            // 如果列数较多，使用交互式查看器；否则简单显示
            if (dt.Columns.Count > 8)
            {
                await ShowInteractiveTable(dt);
            }
            else
            {
                ShowSimpleTable(dt);
            }
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]SQL 执行错误: {ex.Message}[/]");
        }

        return true;
    }

    /// <summary>
    /// 交互式表格查看器 - 支持左右滚动。
    /// </summary>
    private async Task ShowInteractiveTable(System.Data.DataTable dt)
    {
        var terminalWidth = Console.WindowWidth;
        var visibleCols = Math.Min(dt.Columns.Count, Math.Max(5, terminalWidth / 15));
        var colOffset = 0;
        var rowCount = Math.Min(dt.Rows.Count, 50);
        
        while (true)
        {
            Console.Clear();
            AnsiConsole.MarkupLine($"[grey]← → 左右滚动 | q 退出 | 列 {colOffset + 1}-{Math.Min(colOffset + visibleCols, dt.Columns.Count)}/{dt.Columns.Count}[/]\n");
            
            var table = new Table().Border(TableBorder.Rounded);
            
            // 添加可见范围内的列
            for (int i = colOffset; i < Math.Min(colOffset + visibleCols, dt.Columns.Count); i++)
            {
                var colName = dt.Columns[i].ColumnName;
                if (colName.Length > 20)
                    colName = colName.Substring(0, 17) + "...";
                table.AddColumn(new TableColumn($"[cyan]{colName}[/]").NoWrap());
            }
            
            // 添加行
            for (int i = 0; i < rowCount; i++)
            {
                var row = dt.Rows[i];
                var values = new List<string>();
                
                for (int j = colOffset; j < Math.Min(colOffset + visibleCols, dt.Columns.Count); j++)
                {
                    var val = row[j]?.ToString();
                    if (val == null)
                    {
                        values.Add("[grey]NULL[/]");
                    }
                    else
                    {
                        val = Markup.Escape(val); // 转义特殊字符
                        if (val.Length > 25)
                            val = val.Substring(0, 22) + "...";
                        values.Add(val);
                    }
                }
                
                table.AddRow(values.ToArray());
            }
            
            AnsiConsole.Write(table);
            AnsiConsole.MarkupLine($"\n[grey]{dt.Rows.Count} 行，{dt.Columns.Count} 列[/]");
            
            // 读取按键
            var key = Console.ReadKey(true);
            
            if (key.Key == ConsoleKey.Q || key.Key == ConsoleKey.Escape)
                break;
            else if (key.Key == ConsoleKey.LeftArrow && colOffset > 0)
                colOffset--;
            else if (key.Key == ConsoleKey.RightArrow && colOffset + visibleCols < dt.Columns.Count)
                colOffset++;
        }
    }

    /// <summary>
    /// 简单表格显示（列数较少时使用）。
    /// </summary>
    private void ShowSimpleTable(System.Data.DataTable dt)
    {
        var table = new Table().Border(TableBorder.Rounded);
        
        foreach (System.Data.DataColumn col in dt.Columns)
        {
            table.AddColumn($"[cyan]{col.ColumnName}[/]");
        }
        
        var rowCount = Math.Min(dt.Rows.Count, 100);
        for (int i = 0; i < rowCount; i++)
        {
            var row = dt.Rows[i];
            var values = new string[dt.Columns.Count];
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                var val = row[j]?.ToString();
                if (val == null)
                {
                    values[j] = "[grey]NULL[/]";
                }
                else
                {
                    val = Markup.Escape(val); // 转义特殊字符
                    if (val.Length > 50)
                        val = val.Substring(0, 47) + "...";
                    values[j] = val;
                }
            }
            table.AddRow(values);
        }
        
        AnsiConsole.Write(table);
        
        if (dt.Rows.Count > 100)
            AnsiConsole.MarkupLine($"[grey]显示前 100 行，共 {dt.Rows.Count} 行[/]");
        else
            AnsiConsole.MarkupLine($"[grey]{dt.Rows.Count} 行[/]");
    }

    private bool UnknownCommand(string cmd)
    {
        AnsiConsole.MarkupLine($"[yellow]未知命令: '{cmd}', 输入 'help' 查看可用命令[/]");
        return true;
    }

    // 供 AutoCompleter 访问
    public DatabaseSchema? Schema => _schema;
    public List<string> History => _history;
    public bool IsConnected => _db != null;
}

/// <summary>
/// 数据库 Schema 信息。
/// </summary>
public class DatabaseSchema
{
    public List<string> Databases { get; } = new();
    public List<TableSchema> Tables { get; } = new();
}

public class TableSchema
{
    public string Name { get; set; } = "";
    public List<ColumnSchema> Columns { get; } = new();
}

public class ColumnSchema
{
    public string Name { get; set; } = "";
    public string Type { get; set; } = "";
    public bool IsPrimaryKey { get; set; }
    public bool IsNullable { get; set; }
}
