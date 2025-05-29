using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using EducativeIo.BoundedBuffer;

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

    // static int dummy = 0;

    static EventWaitHandle eventWaitHandleManual = new EventWaitHandle(
        false,
        EventResetMode.ManualReset
    );

    static EventWaitHandle eventWaitHandleAuto = new EventWaitHandle(
        false,
        EventResetMode.AutoReset
    );

    static AutoResetEvent are1 = new AutoResetEvent(false);
    static AutoResetEvent are2 = new AutoResetEvent(false);

    static ManualResetEvent mre1 = new ManualResetEvent(false);
    static ManualResetEvent mre2 = new ManualResetEvent(false);

    static bool printTurn = false;
    static volatile bool shutDown = false;
    static int val = 0;
    static object pad = new object();

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

        // Thread[] threads = new Thread[10];
        // for (int i = 0; i < threads.Length; i++)
        // {
        //     threads[i] = new Thread(() =>
        //     {
        //         for (int j = 0; j < 10000; j++)
        //         {
        //             // dummy++;
        //             Interlocked.Add(ref dummy, 1);
        //         }
        //     });
        // }
        // for (int i = 0; i < threads.Length; i++)
        // {
        //     threads[i].Start();
        // }
        // for (int i = 0; i < threads.Length; i++)
        // {
        //     threads[i].Join();
        // }

        // Console.WriteLine(dummy);

        // ThreadPool.QueueUserWorkItem(_ =>
        // {
        //     // eventWaitHandleManual.WaitOne();
        //     // are1.WaitOne();
        //     mre1.WaitOne();
        //     Console.WriteLine("Manual event signaled 1");
        //     mre2.Set();
        //     // are2.Set();
        //     // eventWaitHandleAuto.Set();
        // });

        // ThreadPool.QueueUserWorkItem(_ =>
        // {
        //     // eventWaitHandleManual.WaitOne();
        //     // are1.WaitOne();
        //     mre1.WaitOne();
        //     Console.WriteLine("Manual event signaled 2");
        //     mre2.Set();
        //     // are2.Set();
        //     // eventWaitHandleAuto.Set();
        // });

        // // WaitHandle.SignalAndWait(eventWaitHandleManual, eventWaitHandleAuto);
        // // are1.Set();
        // // are2.WaitOne();

        // mre1.Set();
        // mre2.WaitOne();

        // // mre1.Reset();
        // mre1.WaitOne();
        // Console.WriteLine("Main thread exiting");

        // Thread tp1 = new Thread(Printer) { IsBackground = true };
        // Thread tp2 = new Thread(Printer) { IsBackground = true };
        // Thread tp3 = new Thread(Printer) { IsBackground = true };
        // Thread tf1 = new Thread(Finder) { IsBackground = true };
        // Thread tf2 = new Thread(Finder) { IsBackground = true };

        // tp1.Start();
        // tp2.Start();
        // tp3.Start();
        // tf1.Start();
        // tf2.Start();

        // Thread.Sleep(15000);
        // shutDown = true;
        // Console.WriteLine("Shutting down");

        // tp1.Join();
        // tp2.Join();
        // tp3.Join();
        // tf1.Join();
        // tf2.Join();

        // bool flag = false;
        // object obj = new object();

        // ThreadPool.QueueUserWorkItem(_ =>
        // {
        //     while (true)
        //     {
        //         Monitor.Enter(obj);

        //         while (flag) Monitor.Wait(obj);

        //         Console.WriteLine("Ping");
        //         flag = !flag;
        //         Thread.Sleep(1000);
        //         Monitor.Exit(obj);
        //     }
        // });
        // ThreadPool.QueueUserWorkItem(_ =>
        // {
        //     while (true)
        //     {
        //         Monitor.Enter(obj);

        //         while (!flag) Monitor.Wait(obj);

        //         Console.WriteLine("Pong");
        //         flag = !flag;
        //         Thread.Sleep(2000);
        //         Monitor.Exit(obj);
        //     }
        // });
        // ThreadPool.QueueUserWorkItem(state =>
        // {
        //     while (true)
        //     {
        //         ThreadPool.GetAvailableThreads(out int workerThreads, out int completionPortion);
        // {
        //         ThreadPool.GetMaxThreads(out int maxWorker, out int maxCompl);
        //         Console.WriteLine($"{workerThreads}  {completionPortion}  {maxWorker} {maxCompl}");
        //         Thread.Sleep(1000);
        //     }
        // }, new object(), true);
        // Console.ReadKey();

        // var res = await new MyAwaiter(2000);

        // Task<int> tcs = new TcsTst().Wait();

        // var completed = await Task.WhenAny(tcs, Task.Delay(5000));

        // if (completed == tcs)
        //     Console.WriteLine("Task completed successfully");
        //     var result = await tcs;
        //     Console.WriteLine($"Result from TCS: {result}");
        // }
        // else
        // {
        //     Console.WriteLine("Task timed out");
        // }

        // new BBTest(1).Run();
        // new TokenBucketFilterTest().Run();

        // new DefferedCallbackExecutorTest().Run2();
        // new DeferredCallbackExecutorTest().run();

        // new SemaTest().Run();
        // new ReadersWriteLockTest().run();
        // new UnisexRoomTest().run();
        // new BarrierTest().run();
        // new UberSeatingProblemTest().run();
        // new DiningPhilosophersTest().Run();
        // new BarberShopTest().Run();
        // new SupermanTest().Test();

        // int[] arr = [2, 5, 4, 1, 23, 0, 15, 0, -1];
        // new MultiThreadedMergeSort().Sort(0, arr.Length - 1, arr);
        // Console.WriteLine("Sorted array: " + string.Join(", ", arr));

        SyncExecutor executor = new SyncExecutor();
        executor.AsynchronousExecution(new Callback());
        Console.WriteLine("Main thread waiting for callback to complete...");
    }

    public static void Printer()
    {
        while (!shutDown)
        {
            Monitor.Enter(pad);
            while (!printTurn && !shutDown)
                Monitor.Wait(pad);

            printTurn = false;
            Console.WriteLine($"Thread {System.Environment.CurrentManagedThreadId} printing {val}");
            Thread.Sleep(1000);
            Monitor.PulseAll(pad);
            Monitor.Exit(pad);
        }
    }

    public static void Finder()
    {
        while (!shutDown)
        {
            Monitor.Enter(pad);
            while (printTurn && !shutDown)
                Monitor.Wait(pad);

            printTurn = true;
            val = new Random().Next(1, 100);
            Console.WriteLine($"Thread {System.Environment.CurrentManagedThreadId} finding {val}");
            Thread.Sleep(2000);
            Monitor.PulseAll(pad);
            Monitor.Exit(pad);
        }
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

// public class QuizQuestion
// {

//     private readonly Object obj = new Object();

//     void MonitorExit()
//     {
//         Thread.Sleep(500);
//         Monitor.Exit(obj);
//     }

//     void MonitorEnter()
//     {
//         Monitor.Enter(obj);
//         Thread.Sleep(500);
//     }

//     public void run()
//     {

//         Thread t1 = new Thread(new ThreadStart(MonitorEnter));
//         Thread t2 = new Thread(new ThreadStart(MonitorExit));

//         t1.Start();
//         t2.Start();

//         t1.Join();
//         t2.Join();

//         Console.WriteLine("Hello");
//         Monitor.Enter(obj);
//         Console.WriteLine("World");
//         Monitor.Exit(obj);
//     }
// }
public class QuizQuestion
{
    private Object obj = false;

    public void printMessage()
    {
        Monitor.Enter(obj);
        obj = true;
        Thread.Sleep(3000);
        Monitor.Exit(obj);
        Console.WriteLine("All is good");
    }

    // public Task run()
    // {

    //     Thread t1 = new Thread(new ThreadStart(printMessage));
    //     t1.Start();

    //     Thread t2 = new Thread(new ThreadStart(printMessage));
    //     Thread.Sleep(5000);
    //     t2.Start();

    //     t1.Join();
    //     t2.Join();

    // }
}

public class MyAwaiter : INotifyCompletion
{
    private bool _isCompleted;
    private int _sleepDuration;

    public MyAwaiter(int sleepDuration)
    {
        _sleepDuration = sleepDuration;
        _isCompleted = false;
    }

    public bool IsCompleted => _isCompleted;

    public void OnCompleted(Action continuation)
    {
        new Thread(() =>
        {
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} started");
            Thread.Sleep(_sleepDuration);
            _isCompleted = true;
            continuation();
        }).Start();
    }

    public string GetResult() => "Hello World";

    public MyAwaiter GetAwaiter() => this;
}

public class TcsTst
{
    private TaskCompletionSource<int> _tcs { get; }

    public TcsTst()
    {
        _tcs = new TaskCompletionSource<int>();
    }

    public Task<int> Wait()
    {
        ThreadPool.QueueUserWorkItem(_ =>
        {
            Console.WriteLine("Waiting for 2 seconds...");
            Thread.Sleep(2000);
            // _tcs.SetResult(42);
            Console.WriteLine("Task completed");
        });

        return _tcs.Task;
    }
}
