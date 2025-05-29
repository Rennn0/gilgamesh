namespace EducativeIo.BoundedBuffer;

public interface ICallback
{
    void Done();
}

public class Executor
{
    public virtual void AsynchronousExecution(ICallback callback)
    {
        Thread t = new Thread(() =>
        {
            Console.WriteLine("Thread started");
            Thread.Sleep(5000);
            callback.Done();
        });
        t.Start();
    }
}

public class Callback : ICallback
{
    public void Done()
    {
        Console.WriteLine("Asynchronous execution completed.");
    }
}

public class CallbackWrapper : ICallback
{
    private readonly bool[] _isDone;
    private readonly Dealer _dealer;
    private readonly object _padlock;
    private readonly ICallback _callback;
    private readonly Semaphore _semaphore;

    public CallbackWrapper(bool[] isDone, object padlock, ICallback callback)
    {
        _isDone = isDone;
        _padlock = padlock;
        _callback = callback;
        _dealer = new Dealer();
        _semaphore = new Semaphore(0, 1);
    }

    public CallbackWrapper(Dealer dealer, object padlock, ICallback callback)
    {
        _dealer = dealer;
        _padlock = padlock;
        _callback = callback;
        _isDone = [false];
        _semaphore = new Semaphore(0, 1);
    }

    public CallbackWrapper(Semaphore sema, object padlock, ICallback callback)
    {
        _dealer = new Dealer();
        _padlock = padlock;
        _callback = callback;
        _isDone = [false];
        _semaphore = sema;
    }

    public void Done()
    {
        _callback.Done();

        // Monitor.Enter(_padlock);
        // _isDone[0] = true;
        // _dealer.IsDone = true;
        _semaphore.Release();
        // Monitor.PulseAll(_padlock);
        // Monitor.Exit(_padlock);
    }
}

public class SyncExecutor : Executor
{
    public override void AsynchronousExecution(ICallback callback)
    {
        object padlock = new object();
        // bool[] isDone = [false];
        // Dealer dealer = new Dealer();
        Semaphore sema = new Semaphore(0, 1);
        CallbackWrapper wrapper = new CallbackWrapper(sema, padlock, callback);

        base.AsynchronousExecution(wrapper);

        sema.WaitOne();
        // Monitor.Enter(padlock);
        // while (!dealer.IsDone)
        // {
        //     Monitor.Wait(padlock);
        // }
        // Monitor.Exit(padlock);
    }
}

public class Dealer
{
    public bool IsDone { get; set; } = false;
}
