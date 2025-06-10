// See https://aka.ms/new-console-template for more information
using System.Threading;
internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Progess Start");
        var thread1 = new Thread(DoWork);
        var thread2 = new Thread(DoWork);

        thread1.Start();
        thread2.Start();

        thread1.Join();
        thread2.Join();

        Console.WriteLine("All Done");
    }
    static void DoWork()
    {
        Console.WriteLine("Doing work.,.");
        Thread.Sleep(1000);
        Console.WriteLine("Doing End");
    }
}