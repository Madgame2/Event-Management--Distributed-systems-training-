namespace ConsoleClinet.UI;

public static class ConsoleRenderer
{
    public static void DrawHeader()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(@"
 ╔══════════════════════════════════════════════════════════════════════╗
 ║                   EVENT MANAGEMENT SYSTEM CLIENT                     ║
 ╚══════════════════════════════════════════════════════════════════════╝");
        Console.ResetColor();
    }
    
    
    public static void DrawMenu()
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("\n┌── [ Главное меню ] ───────────────────────────────────┐");
        Console.ResetColor();
        
        Console.WriteLine("  │ [1]  Создать новое мероприятие                       │");
        Console.WriteLine("  │ [2]  Зарегистрировать участника                      │");
        Console.WriteLine("  │ [0]  Выйти из приложения                            │");
        
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("└───────────────────────────────────────────────────────┘");
        Console.ResetColor();
    }
    
    public static void PrintSectionHeader(string title)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"\n─── ► {title.ToUpper()} ◄ ───");
        Console.ResetColor();
    }
    
    public static void PrintSuccess(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\n [✔] УСПЕХ: {message}");
        Console.ResetColor();
    }
    
    public static void PrintError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\n [✖] ОШИБКА: {message}");
        Console.ResetColor();
    }
    
    public static void PrintInfo(string message)
    {
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine($" [i] {message}");
        Console.ResetColor();
    }
    
    public static string ReadInput(string prompt)
    {
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.Write($" › {prompt}: ");
        Console.ResetColor();
        return Console.ReadLine()?.Trim() ?? string.Empty;
    }
}