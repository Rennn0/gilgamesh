// ReSharper disable InvertIf
// ReSharper disable ConvertIfStatementToNullCoalescingAssignment

using Console = System.Console;

namespace EducativeIo.BoundedBuffer;

public class SupermanTest()
{
    void WorkWithSuperman()
    {
        while (true)
            Superman.Instance.Fly();
    }

    public void Test()
    {
        const int threadsCount = 35;
        Thread[] threads = new Thread[threadsCount];

        for (int i = 0; i < threadsCount; i++)
        {
            threads[i] = new Thread(new ThreadStart(WorkWithSuperman));
        }

        for (int i = 0; i < threadsCount; i++)
        {
            threads[i].Start();
        }

        for (int i = 0; i < threadsCount; i++)
        {
            threads[i].Join();
        }
    }
}

public class Superman
{
    // private static volatile Superman? s_instance;
    // private static readonly object mr_padlock = new object();

    private static readonly Lazy<Superman> smr_instance = new Lazy<Superman>(
        () => new Superman(),
        LazyThreadSafetyMode.ExecutionAndPublication
    );

    public static Superman Instance => smr_instance.Value;

    // BAD START
    // private static Superman? smr_instance;
    // public static Superman Instance
    // {
    //     get
    //     {
    //         if (smr_instance is null)
    //         {
    //             smr_instance = new Superman();
    //         }
    //
    //         return smr_instance;
    //     }
    // }
    // BAD END

    // public static Superman Instance => InstanceHelper.Instance;

    private Superman()
    {
        Console.WriteLine("Invoked");
        Interlocked.Increment(ref Counter);
    }

    static Superman() { }

    public static int Counter = 0;

    public void Fly()
    {
        // Console.WriteLine("Superman is flying!");
        int c = Counter;
        Console.WriteLine(c);
    }

    // public static Superman Instance
    // {
    //     get
    //     {
    //         if (s_instance is null)
    //         {
    //             Monitor.Enter(mr_padlock);
    //
    //             if (s_instance is null)
    //             {
    //                 s_instance = new Superman();
    //             }
    //
    //             Monitor.Exit(mr_padlock);
    //         }
    //
    //         return s_instance;
    //     }
    // }

    // private class InstanceHelper
    // {
    //     static InstanceHelper() { }
    //
    //     internal static readonly Superman Instance = new Superman();
    // }
}
