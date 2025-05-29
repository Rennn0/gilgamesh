namespace EducativeIo.BoundedBuffer;

public interface ICallback
{
    void Done();
}

public class Executor
{
    public virtual void AsynchronousExecution(ICallback callback)
    {
        new Thread(() =>
        {
            Console.WriteLine("Thread started");
            Thread.Sleep(5000);
            callback.Done();
        }).Start();
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

    public CallbackWrapper(bool[] isDone, object padlock, ICallback callback)
    {
        _isDone = isDone;
        _padlock = padlock;
        _callback = callback;
        _dealer = new Dealer();
    }

    public CallbackWrapper(Dealer dealer, object padlock, ICallback callback)
    {
        _dealer = dealer;
        _padlock = padlock;
        _callback = callback;
        _isDone = [false];
    }

    public void Done()
    {
        _callback.Done();

        Monitor.Enter(_padlock);
        // _isDone[0] = true;
        _dealer.IsDone = true;
        Monitor.PulseAll(_padlock);
        Monitor.Exit(_padlock);
    }
}

public class SyncExecutor : Executor
{
    public override void AsynchronousExecution(ICallback callback)
    {
        object padlock = new object();
        // bool[] isDone = [false];

        Dealer dealer = new Dealer();

        CallbackWrapper wrapper = new CallbackWrapper(dealer, padlock, callback);

        base.AsynchronousExecution(wrapper);

        Monitor.Enter(padlock);
        while (!dealer.IsDone)
        {
            Monitor.Wait(padlock);
        }
        Monitor.Exit(padlock);
    }
}

public class Dealer
{
    public bool IsDone { get; set; } = false;
}
