namespace Logger;
/// <summary>
/// Отдельный проект с логгером, который будет общим для всех заданий
/// </summary>
public class Logger : ILogger
{
    public void Log(string message)
    {
        Console.WriteLine(message);
    }
}