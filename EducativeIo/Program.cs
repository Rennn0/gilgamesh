using System.Collections.Concurrent;
using System.Net.NetworkInformation;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace EducativeIo;

internal class Solution
{
    private static readonly Mutex _mutex = new Mutex();
    private static bool _flag { get; set; } = false;

    private static SemaphoreSlim s1 = new SemaphoreSlim(0, 1);
    private static SemaphoreSlim s2 = new SemaphoreSlim(1, 2);

    private static SemaphoreSlim sema = new SemaphoreSlim(3, 3);


    private static BlockingCollection<int> tasks = new BlockingCollection<int>();
    private static SemaphoreSlim semaProd = new SemaphoreSlim(10);
    private static SemaphoreSlim semaCons = new SemaphoreSlim(3);

    static int dummy = 0;
    public static void Main(string[] args)
    {
        // new Thread(Ping) { IsBackground = true }.Start();
        // new Thread(Pong) { IsBackground = true }.Start();

        // new Thread(PingSema) { IsBackground = true }.Start();
        // new Thread(PongSema) { IsBackground = true }.Start();
        // Thread.Sleep(100);

        // for (int i = 0; i < 10; i++)
        // {
        //     int id = i;
        //     new Thread(async _ => await SemaTestAsync(id)) { IsBackground = true }.Start();
        // }

        // Console.WriteLine("Sema started");
        // Console.ReadLine();
        // Task[] producers = new Task[7];
        // Task[] consumers = new Task[5];
        // for (int i = 0; i < producers.Length; i++)
        // {
        //     int id = i;
        //     producers[i] = Task.Run(async () => await Producer(id));
        // }

        // for (int i = 0; i < consumers.Length; i++)
        // {
        //     int id = i;
        //     consumers[i] = Task.Run(async () => await Consumer(id));
        // }

        // Task.WaitAll(producers);

        // tasks.CompleteAdding();

        // Task.WaitAll(consumers);
        // using (var semaphore = new Semaphore(2, 2, "semaphore"))
        // {
        //     Console.WriteLine("Semaphore created");
        //     semaphore.WaitOne();
        //     Console.WriteLine("Semaphore acquired");
        //     Console.ReadKey();
        //     semaphore.Release();
        //     Console.WriteLine("Semaphore released");
        // }

        Thread[] threads = new Thread[10];
        for (int i = 0; i < threads.Length; i++)
        {
            threads[i] = new Thread(() =>
            {
                for (int j = 0; j < 10000; j++)
                {
                    // dummy++;
                    Interlocked.Add(ref dummy, 1);
                }
            });
        }
        for (int i = 0; i < threads.Length; i++)
        {
            threads[i].Start();
        }
        for (int i = 0; i < threads.Length; i++)
        {
            threads[i].Join();
        }

        Console.WriteLine(dummy);
    }
    public static async Task Producer(int id)
    {
        while (true)
        {
            await semaProd.WaitAsync();
            try
            {
                Console.WriteLine($"Producer {id} producing");
                int task = new Random().Next(1, 100);
                tasks.Add(task);
                Console.WriteLine($"Producer {id} produced {task}++++++++");
            }
            finally
            {
                Console.WriteLine($"Producer {id} releasing");
                semaProd.Release();
                Console.WriteLine($"Tasks remaining: {tasks.Count} =======");
                await Task.Delay(10000);
            }
        }
    }

    public static async Task Consumer(int id)
    {
        foreach (var task in tasks.GetConsumingEnumerable())
        {
            await semaCons.WaitAsync();
            try
            {
                Console.WriteLine($"Consumer {id} consuming {task} -------");
            }
            finally
            {
                Console.WriteLine($"Consumer {id} releasing");
                semaCons.Release();
                Console.WriteLine($"Tasks remaining: {tasks.Count} =======");
                await Task.Delay(2500);
            }
        }
    }
    public static async Task SemaTestAsync(int id)
    {
        Console.WriteLine($"Thread {id} waiting");
        await sema.WaitAsync();
        try
        {
            Console.WriteLine($"Thread {id} working");
            await Task.Delay(1000);
        }
        finally
        {
            Console.WriteLine($"Thread {id} releasing");
            sema.Release();
        }
    }

    public static void PingSema()
    {
        while (true)
        {
            s1.Wait();
            Console.WriteLine("Ping");
            // Thread.Sleep(10);
            s1.Release();
        }
    }
    public static void PongSema()
    {
        while (true)
        {
            s1.Release();
            s1.Wait();
            Console.WriteLine("Pong");
            // Thread.Sleep(10);
        }
    }

    public static void Ping()
    {
        while (true)
        {
            _mutex.WaitOne();

            while (_flag)
            {
                _mutex.ReleaseMutex();
                _mutex.WaitOne();
            }

            Console.WriteLine("Ping");
            _flag = !_flag;
            Thread.Sleep(10);
            _mutex.ReleaseMutex();
        }
    }

    public static void Pong()
    {
        while (true)
        {
            _mutex.WaitOne();

            while (!_flag)
            {
                _mutex.ReleaseMutex();
                _mutex.WaitOne();
            }

            Console.WriteLine("Pong");
            _flag = !_flag;
            Thread.Sleep(10);
            _mutex.ReleaseMutex();

            var semaphore = new Semaphore(1, 1);
            semaphore.WaitOne();
            semaphore.Release();
        }
    }

}