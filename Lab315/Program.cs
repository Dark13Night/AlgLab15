using System;
using System.IO;
using System.Collections.Generic;
using System.Timers;

// Интерфейс для наблюдателя
interface IObserver
{
    void Update(string filename);
}

// Класс, реализующий обозреваемый объект
class FileSystemWatcherSubject
{
    private string directoryPath;
    private System.Timers.Timer timer;
    private List<IObserver> observers;

    public FileSystemWatcherSubject(string directoryPath)
    {
        this.directoryPath = directoryPath;
        this.timer = new System.Timers.Timer(1000); // Таймер с интервалом в 1 секунду
        this.timer.Elapsed += TimerElapsed;
        this.observers = new List<IObserver>();
    }

    public void Attach(IObserver observer)
    {
        observers.Add(observer);
    }

    public void Detach(IObserver observer)
    {
        observers.Remove(observer);
    }

    public void Notify(string filename)
    {
        foreach (var observer in observers)
        {
            observer.Update(filename);
        }
    }

    public void StartWatching()
    {
        timer.Start();
    }

    public void StopWatching()
    {
        timer.Stop();
    }

    private void TimerElapsed(object sender, ElapsedEventArgs e)
    {
        var files = Directory.GetFiles(directoryPath);
        foreach (var file in files)
        {
            // Проверяем, что файл был добавлен или изменен
            var fileInfo = new FileInfo(file);
            if (fileInfo.CreationTime > DateTime.Now.AddSeconds(-1) || fileInfo.LastWriteTime > DateTime.Now.AddSeconds(-1))
            {
                Notify(file);
            }
        }
    }
}

// Класс, реализующий наблюдателя
class FileSystemWatcherObserver : IObserver
{
    public void Update(string filename)
    {
        Console.WriteLine($"Файл {filename} был изменен");
    }
}

class Program
{
    static void Main(string[] args)
    {
        var watcher = new FileSystemWatcherSubject("D:/Labs/Lab315/Directory");
        var observer = new FileSystemWatcherObserver();

        watcher.Attach(observer);
        watcher.StartWatching();

        Console.WriteLine("Нажмите Enter для выхода");
        Console.ReadLine();

        watcher.StopWatching();
    }
}
