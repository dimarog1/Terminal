namespace Additional;

public class Additional
{
    public static string GetConvertedSize(double bytes)
    {
        var kb = bytes / 1024d;
        if (kb > 1)
        {
            var mb = kb / 1024d;
            if (mb > 1)
            {
                var gb = mb / 1024d;
                return gb > 1 ? $"{Math.Round(gb, 1)} ГБ" : $"{Math.Round(mb, 1)} МБ";
            }

            return $"{Math.Round(kb, 1)} КБ";
        }

        return $"{bytes} Б";
    }
    public static void CleanConsole()
    {
        Console.SetCursorPosition(0, 0);
        Console.Clear();
    }
    
    public static void ShowErrorMessage(int code)
    {
        var codes = new Dictionary<int, string>()
        {
            {0, "Такой команды не существует"},
            {1, "Передан неправильный параметр команды <show>, должно быть передано число!"},
            {2, "Не указано название файла"},
            {3, "Такого файла не существует"},
            {4, "Для комманды <cd> должен быть передан параметр - путь к дериктории"},
            {5, "Такой директории не существует"},
            {6, "Вы достигли самого верха дериктории"},
            {7, "Неправильно введена комманда <dir>"}
        };
        PrintColorMessage(codes[code], ConsoleColor.Red);
    }
    
    public static void Help()
    {
        var message =
            "dir - показате абсолютный путь текущей директории" + Environment.NewLine +
            "ls <n> - показать дерево текущей директории, n - глубина показа файлов" + Environment.NewLine +
            "cls - очистить консоль" + Environment.NewLine +
            "cd <path> - переход в ведённую директорию path" + Environment.NewLine +
            "cd up - переход в вышележащую директорию" + Environment.NewLine +
            "cat <filename> - показать содержимое файла, filename - название файла" + Environment.NewLine +
            "help - вывести список комманд" + Environment.NewLine +
            "exit - выход из терминала";
        PrintColorMessage(message, ConsoleColor.Yellow);
    }

    public static void PrintColorMessage(string message, ConsoleColor color)
    {
        var defaultColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ForegroundColor = defaultColor;
    }
}