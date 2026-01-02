namespace UnityBridge.db;

using PrettyPrompt;
using PrettyPrompt.Completion;
using PrettyPrompt.Documents;
using PrettyPrompt.Highlighting;

/// <summary>
/// PrettyPrompt 回调实现。
/// 提供 fish 风格的自动补全建议（灰色 ghost text）。
/// </summary>
public class SqlPromptCallbacks : PromptCallbacks
{
    private readonly DbShell _shell;
    
    // SQL 关键字 (包含组合关键字)
    private static readonly string[] SqlKeywords = 
    {
        // 基础关键字
        "select", "from", "where", "and", "or", "not", "in", "like", "between",
        "limit", "offset", "having", "on", "as", "distinct",
        // 组合关键字 - JOIN
        "join", "left join", "right join", "inner join", "outer join", "cross join",
        // 组合关键字 - ORDER/GROUP
        "order by", "group by", "order by asc", "order by desc",
        // INSERT/UPDATE/DELETE
        "insert into", "values", "update", "set", "delete from",
        // DDL
        "create table", "drop table", "alter table", "add column",
        "primary key", "foreign key", "references", "index", "unique",
        // 其他
        "null", "default", "auto_increment", "constraint",
        "count", "sum", "avg", "max", "min",
        "case", "when", "then", "else", "end",
        "union", "union all", "exists",
        // 数据库命令
        "show databases", "show tables", "use", "pragma"
    };

    // Shell 命令
    private static readonly string[] ShellCommands = 
    {
        "connect", "describe", "history", "clear", "help", "exit",
        "mysql", "pgsql", "mssql"  // 数据库类型
    };

    public SqlPromptCallbacks(DbShell shell)
    {
        _shell = shell;
    }

    /// <summary>
    /// 提供自动补全建议。
    /// PrettyPrompt 会在用户输入时自动调用此方法，并以灰色 ghost text 显示建议。
    /// </summary>
    protected override Task<IReadOnlyList<CompletionItem>> GetCompletionItemsAsync(
        string text, 
        int caret, 
        TextSpan spanToBeReplaced, 
        CancellationToken cancellationToken)
    {
        var textBeforeCaret = text.Substring(0, caret);
        
        // 找到当前正在输入的单词
        var lastSpaceIndex = textBeforeCaret.LastIndexOf(' ');
        var currentWord = lastSpaceIndex >= 0 
            ? textBeforeCaret.Substring(lastSpaceIndex + 1) 
            : textBeforeCaret;
        
        var suggestions = new List<CompletionItem>();
        
        // 如果当前单词为空，不提供建议
        if (string.IsNullOrEmpty(currentWord))
        {
            return Task.FromResult<IReadOnlyList<CompletionItem>>(suggestions);
        }
        
        // 1. 检查是否需要表名补全
        if (NeedsTableCompletion(textBeforeCaret) && _shell.Schema != null)
        {
            foreach (var table in _shell.Schema.Tables)
            {
                if (table.Name.StartsWith(currentWord, StringComparison.OrdinalIgnoreCase))
                {
                    suggestions.Add(new CompletionItem(
                        replacementText: table.Name,
                        getExtendedDescription: _ => Task.FromResult(new FormattedString($"表: {table.Name} ({table.Columns.Count} 列)"))
                    ));
                }
            }
            return Task.FromResult<IReadOnlyList<CompletionItem>>(suggestions);
        }
        
        // 1.5. 检查是否需要数据库名补全 (USE 后面)
        if (NeedsDatabaseCompletion(textBeforeCaret) && _shell.Schema != null)
        {
            foreach (var db in _shell.Schema.Databases)
            {
                if (db.StartsWith(currentWord, StringComparison.OrdinalIgnoreCase))
                {
                    suggestions.Add(new CompletionItem(
                        replacementText: db,
                        getExtendedDescription: _ => Task.FromResult(new FormattedString($"数据库: {db}"))
                    ));
                }
            }
            return Task.FromResult<IReadOnlyList<CompletionItem>>(suggestions);
        }
        
        // 2. 检查是否需要列名补全
        if (NeedsColumnCompletion(textBeforeCaret) && _shell.Schema != null)
        {
            var tableName = ExtractTableName(textBeforeCaret);
            var columns = GetColumns(tableName);
            
            foreach (var col in columns.Where(c => c.Name.StartsWith(currentWord, StringComparison.OrdinalIgnoreCase)))
            {
                suggestions.Add(new CompletionItem(
                    replacementText: col.Name,
                    getExtendedDescription: _ => Task.FromResult(new FormattedString($"{col.Name} ({col.Type})"))
                ));
            }
            return Task.FromResult<IReadOnlyList<CompletionItem>>(suggestions);
        }
        
        // 3. 命令开头 - 提示 Shell 命令和 SQL 关键字
        if (!textBeforeCaret.Contains(' '))
        {
            // Shell 命令
            foreach (var cmd in ShellCommands)
            {
                if (cmd.StartsWith(currentWord, StringComparison.OrdinalIgnoreCase))
                {
                    suggestions.Add(new CompletionItem(replacementText: cmd));
                }
            }
            
            // SQL 关键字
            foreach (var kw in SqlKeywords)
            {
                if (kw.StartsWith(currentWord, StringComparison.OrdinalIgnoreCase))
                {
                    suggestions.Add(new CompletionItem(replacementText: kw));
                }
            }
            
            // 历史记录
            foreach (var h in _shell.History.TakeLast(10))
            {
                if (h.StartsWith(currentWord, StringComparison.OrdinalIgnoreCase) && h != currentWord)
                {
                    suggestions.Add(new CompletionItem(replacementText: h));
                }
            }
        }
        else
        {
            // 4. 语句中间 - SQL 关键字
            foreach (var kw in SqlKeywords)
            {
                if (kw.StartsWith(currentWord, StringComparison.OrdinalIgnoreCase))
                {
                    suggestions.Add(new CompletionItem(replacementText: kw));
                }
            }
            
            // 也添加表名
            if (_shell.Schema != null)
            {
                foreach (var table in _shell.Schema.Tables)
                {
                    if (table.Name.StartsWith(currentWord, StringComparison.OrdinalIgnoreCase))
                    {
                        suggestions.Add(new CompletionItem(replacementText: table.Name));
                    }
                }
            }
        }
        
        return Task.FromResult<IReadOnlyList<CompletionItem>>(suggestions);
    }

    /// <summary>
    /// 检测是否需要表名补全。
    /// </summary>
    private bool NeedsTableCompletion(string input)
    {
        var lower = input.ToLowerInvariant();
        string[] keywords = { "from ", "join ", "into ", "update ", "table ", "describe ", "desc " };
        
        foreach (var kw in keywords)
        {
            if (lower.Contains(kw))
            {
                var idx = lower.LastIndexOf(kw);
                var afterKw = lower.Substring(idx + kw.Length);
                // 如果关键字后只有一个词（正在输入），需要表名补全
                if (!afterKw.Contains(' '))
                    return true;
            }
        }
        
        return false;
    }

    /// <summary>
    /// 检测是否需要数据库名补全 (USE 后面)。
    /// </summary>
    private bool NeedsDatabaseCompletion(string input)
    {
        var lower = input.ToLowerInvariant();
        
        // use database_name
        if (lower.Contains("use "))
        {
            var idx = lower.LastIndexOf("use ");
            var afterUse = lower.Substring(idx + 4);
            // 如果 use 后只有一个词（正在输入），需要数据库名补全
            if (!afterUse.Contains(' '))
                return true;
        }
        
        return false;
    }

    /// <summary>
    /// 检测是否需要列名补全。
    /// </summary>
    private bool NeedsColumnCompletion(string input)
    {
        var lower = input.ToLowerInvariant();
        
        // SELECT 后面，FROM 之前
        if (lower.Contains("select ") && !lower.Contains(" from"))
            return true;
        
        return false;
    }

    /// <summary>
    /// 从 SQL 语句中提取表名。
    /// </summary>
    private string? ExtractTableName(string input)
    {
        var lower = input.ToLowerInvariant();
        
        var fromIndex = lower.LastIndexOf(" from ");
        if (fromIndex >= 0)
        {
            var afterFrom = input.Substring(fromIndex + 6).Trim();
            var parts = afterFrom.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length > 0)
                return parts[0];
        }
        
        return null;
    }

    /// <summary>
    /// 获取列信息。
    /// </summary>
    private IEnumerable<ColumnSchema> GetColumns(string? tableName)
    {
        if (_shell.Schema == null)
            return Enumerable.Empty<ColumnSchema>();
        
        if (!string.IsNullOrEmpty(tableName))
        {
            var table = _shell.Schema.Tables
                .FirstOrDefault(t => t.Name.Equals(tableName, StringComparison.OrdinalIgnoreCase));
            
            if (table != null)
                return table.Columns;
        }
        
        // 返回所有列
        return _shell.Schema.Tables.SelectMany(t => t.Columns).DistinctBy(c => c.Name);
    }
}
