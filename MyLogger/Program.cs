
using System;
using System.IO;

public class MyLogger
{
    private ILogRepository _logRepository;

    public MyLogger(ILogRepository logRepository)
    {
        _logRepository = logRepository;
    }

    public void Log(string message)
    {
        _logRepository.SaveLog(message);
    }
}

public interface ILogRepository
{
    void SaveLog(string message);
}

public class TextLogRepository : ILogRepository
{
    private readonly string _filePath;

    public TextLogRepository(string filePath)
    {
        _filePath = filePath;
    }

    public void SaveLog(string message)
    {
        using (StreamWriter writer = new StreamWriter(_filePath, true))
        {
            writer.WriteLine(message);
        }
    }
}

public class JsonLogRepository : ILogRepository
{
    private readonly string _filePath;

    public JsonLogRepository(string filePath)
    {
        _filePath = filePath;
    }

    public void SaveLog(string message)
    {
        using (StreamWriter writer = new StreamWriter(_filePath, true))
        {
            writer.WriteLine($"{{\"message\": \"{message}\"}}");
        }
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        MyLogger logger = new MyLogger(new TextLogRepository("log.txt"));

        logger.Log("This is a text log");

        MyLogger jsonLogger = new MyLogger(new JsonLogRepository("log.json"));

        jsonLogger.Log("This is a JSON log");
    }
}
