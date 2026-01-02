using UnityBridge.db;

AnsiConsole.Write(new FigletText("UnityBridge DB").Color(Color.Cyan1));
AnsiConsole.MarkupLine("[grey]交互式数据库 Shell - 输入 'help' 查看命令, 'exit' 退出[/]");
AnsiConsole.WriteLine();

var shell = new DbShell();
await shell.RunAsync();
