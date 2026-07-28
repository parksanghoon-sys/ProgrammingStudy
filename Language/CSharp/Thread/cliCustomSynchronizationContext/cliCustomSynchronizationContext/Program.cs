
using cliCustomSynchronizationContext;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        Console.WriteLine("Start Main Thread : " + Thread.CurrentThread.ManagedThreadId);

        Console.ReadLine();

        //PostTest();

        SendTest();

        Console.ReadLine();

        StaSynchronizationContext.Release();
        
    }
    private static void PostTest()
    {
        for (int i = 0; i < 10; i++)
        {
            var seq = i;
            StaSynchronizationContext.Post(o => TestActionAsync(seq), null);

            Console.WriteLine("Post completed : {0} , thread : {1} , seq : {2} ",
               DateTime.Now,
               Thread.CurrentThread.ManagedThreadId,
               seq);
        }
    }

    private static void SendTest()
    {
        for (int i = 0; i < 10; i++)
        {
            var seq = i;
            ThreadPool.QueueUserWorkItem(o =>
            {
                StaSynchronizationContext.Send(d => TestActionAsync(seq), null);
                Console.WriteLine("Send completed : {0} , thread : {1} , seq : {2} ",
                   DateTime.Now,
                   Thread.CurrentThread.ManagedThreadId,
                   seq);
            });
        }
    }
    private static void TestAction(int seq)
    {
        if (StaSynchronizationContext.IsInWorker == false)
        {
            Debug.Assert(false, "Caution! Not in synchronization context.");
            Console.WriteLine("Caution! Not in synchronization context.");
            return;
        }

        Console.WriteLine("Woring Thread : {0} , seq : {1}",
           Thread.CurrentThread.ManagedThreadId, seq);

        var task = Task.Run(() =>
        {
            var newThreading = new ThreadingTest();
            newThreading.Dispose();

            Console.WriteLine("In Working : {0} , seq : {1}",
               Thread.CurrentThread.ManagedThreadId, seq);
            return newThreading;
        });

        Thread.Sleep(500);

        Console.WriteLine("Working End : {0} , seq : {1}, task : {2} ",
           Thread.CurrentThread.ManagedThreadId, seq, task.IsCompleted);

        Console.WriteLine("Calling time : " + DateTime.Now);
    }
    private async static void TestActionAsync(int seq)
    {
        if (StaSynchronizationContext.IsInWorker == false)
        {
            Debug.Assert(false, "Caution! Not in synchronization context.");
            Console.WriteLine("Caution! Not in synchronization context.");
            return;
        }

        Console.WriteLine("Wor king Thread : {0} , seq : {1} ",
           Thread.CurrentThread.ManagedThreadId, seq);

        var task = await Task.Run(() =>
        {
            var newThreading = new ThreadingTest();
            newThreading.Dispose();

            Console.WriteLine("In Working : {0} , seq : {1}",
               Thread.CurrentThread.ManagedThreadId, seq);
            return newThreading;
        });

        Thread.Sleep(1000);

        Console.WriteLine("Working End : {0} , seq : {1}",
           Thread.CurrentThread.ManagedThreadId, seq);

        Console.WriteLine("Calling time : " + DateTime.Now);
    }

}
