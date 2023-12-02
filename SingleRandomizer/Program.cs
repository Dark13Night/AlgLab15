using System;
using System.Threading;

public class SingleRandomizer
{
    private static SingleRandomizer instance;
    private static readonly object lockObject = new object();

    private Random random;

    private SingleRandomizer()
    {
        random = new Random();
    }

    public static SingleRandomizer Instance
    {
        get
        {
            if (instance == null)
            {
                lock (lockObject)
                {
                    if (instance == null)
                    {
                        instance = new SingleRandomizer();
                    }
                }
            }
            return instance;
        }
    }

    public int Next()
    {
        lock (lockObject)
        {
            return random.Next();
        }
    }
}
public class Program
{
    public static void Main(string[] args)
    {
        // Создаем несколько задач, которые будут вызывать метод Next() класса SingleRandomizer
        Task[] tasks = new Task[5];
        for (int i = 0; i < tasks.Length; i++)
        {
            tasks[i] = Task.Run(() => TestRandomizer());
        }

        // Ожидаем завершения всех задач
        Task.WaitAll(tasks);
    }

    public static void TestRandomizer()
    {
        SingleRandomizer randomizer = SingleRandomizer.Instance;

        for (int i = 0; i < 5; i++)
        {
            int randomNumber = randomizer.Next();
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: {randomNumber}");
        }
    }
}