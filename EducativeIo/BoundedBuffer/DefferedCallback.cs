namespace EducativeIo.BoundedBuffer
{
    public class DefferedCallback : IComparable<DefferedCallback>
    {
        private long m_time;
        private readonly string mr_message;
        public DefferedCallback(long time, string message)
        {
            m_time = time;
            mr_message = message;
        }
        public long Time
        {
            get => m_time;
            set => m_time = value;
        }
        public string Message => mr_message;

        public int CompareTo(DefferedCallback? other)
        {
            return (int)(Time - (other?.Time ?? 0));
        }
    }

    public class DefferedCallbackExecutor
    {
        private readonly PriorityQueue<DefferedCallback> mr_queue;
        private readonly object mr_sync;
        private readonly DateTime mr_start;
        public DefferedCallbackExecutor()
        {
            mr_queue = new PriorityQueue<DefferedCallback>();
            mr_sync = new object();
            mr_start = new DateTime(1970, 1, 1);
        }
        private long GetEpochTimeMs() => (long)(DateTime.UtcNow - mr_start).TotalMilliseconds;
        private long GetSleepDuration() => mr_queue.Peek().Time - GetEpochTimeMs();
        public void AddCallback(DefferedCallback callback)
        {
            callback.Time = (GetEpochTimeMs() + callback.Time);
            Monitor.Enter(mr_sync);
            mr_queue.Add(callback);
            Monitor.PulseAll(mr_sync);
            Monitor.Exit(mr_sync);
        }
        public void Start()
        {
            while (true)
            {
                Monitor.Enter(mr_sync);

                while (mr_queue.Size() == 0)
                {
                    Monitor.Wait(mr_sync);
                }

                while (mr_queue.Size() != 0)
                {
                    long sleep = GetSleepDuration();
                    if (sleep <= 0) break;
                    Monitor.Wait(mr_sync, (int)sleep);
                }

                DefferedCallback callback = mr_queue.Poll();

                Console.WriteLine($"Executing callback: {callback.Message} at {GetEpochTimeMs()} ms scheduled queue time {callback.Time} ms");
                Monitor.Exit(mr_sync);

            }
        }
    }

    public class DefferedCallbackExecutorTest
    {
        public void Run1()
        {
            DefferedCallback cb1 = new DefferedCallback(1000, "Callback 1");
            DefferedCallback cb2 = new DefferedCallback(6400, "Callback 2");
            DefferedCallback cb3 = new DefferedCallback(5500, "Callback 3");
            DefferedCallback cb4 = new DefferedCallback(3500, "Callback 4");

            DefferedCallbackExecutor executor = new DefferedCallbackExecutor();
            new Thread(new ThreadStart(executor.Start)).Start();

            executor.AddCallback(cb1);
            executor.AddCallback(cb2);
            executor.AddCallback(cb3);
            executor.AddCallback(cb4);
        }

        public void Run2()
        {
            Thread[] allThreads = new Thread[5];
            DefferedCallbackExecutor deferredCallbackExecutor = new DefferedCallbackExecutor();

            Thread service = new Thread(new ThreadStart(deferredCallbackExecutor.Start));
            service.Start();

            for (int i = 0; i < 5; i++)
            {
                Thread thread = new Thread(() => deferredCallbackExecutor.AddCallback(new DefferedCallback(1, "Hello this is thread # " + i)));
                allThreads[i] = thread;
                thread.Start();
                Thread.Sleep(new Random().Next(1, 5) * 1000);
            }

            for (int i = 0; i < 5; i++)
            {
                allThreads[i].Join();
            }
        }
    }
}

// 1748119819291
// 1748120133

// public class DeferredCallbackExecutor
// {
//     private EducativeIo.BoundedBuffer.PriorityQueue<DeferredAction> q = new EducativeIo.BoundedBuffer.PriorityQueue<DeferredAction>();
//     private object padlock = new object();
//     private readonly DateTime epochStart = new DateTime(1970, 1, 1);

//     private long getEpochTime()
//     {
//         TimeSpan span = DateTime.UtcNow - epochStart;
//         return (long)span.TotalMilliseconds;
//     }

//     private long findSleepDuration()
//     {
//         long currentTime = getEpochTime();
//         return q.Peek().getExecutesAt() - currentTime;
//     }


//     // Run by the Executor thread
//     public void start()
//     {
//         long sleepFor = 0;

//         while (true)
//         {
//             Monitor.Enter(padlock);

//             while (q.Size() == 0)
//             {
//                 Monitor.Wait(padlock);
//             }

//             while (q.Size() != 0)
//             {

//                 sleepFor = findSleepDuration();
//                 if (sleepFor <= 0)
//                     break;

//                 Monitor.Wait(padlock, (int)sleepFor);
//             }

//             DeferredAction action = q.Poll();

//             Console.WriteLine("Executed at " + getEpochTime() / 1000 + " required at " + action.getExecutesAt() / 1000
//                             + ": message:" + action.getMessage());

//             Monitor.Exit(padlock);
//         }


//     }

//     public void registerCallback(object obj)
//     {

//         DeferredAction action = (DeferredAction)obj;
//         action.setExecutesAt(getEpochTime() + (action.getExecutesAt() * 1000));

//         Monitor.Enter(padlock);
//         q.Add(action);
//         Monitor.Pulse(padlock);
//         Monitor.Exit(padlock);
//     }
// }

// public class DeferredAction : IComparable<DeferredAction>
// {
//     private long executesAt;
//     private String message;

//     public DeferredAction(long executesAt, String message)
//     {
//         this.executesAt = executesAt;
//         this.message = message;
//     }

//     public long getExecutesAt()
//     {
//         return executesAt;
//     }

//     public void setExecutesAt(long epochExecTime)
//     {
//         executesAt = epochExecTime;
//     }

//     public String getMessage()
//     {
//         return message;
//     }

//     public int CompareTo(DeferredAction otherAction)
//     {
//         return (int)(executesAt - otherAction.getExecutesAt());
//     }
// }


// public class DeferredCallbackExecutorTest
// {
//     private Random random = new Random();

//     public void run()
//     {
//         run2();
//     }

//     public void run1()
//     {
//         Thread[] allThreads = new Thread[5];
//         DeferredCallbackExecutor deferredCallbackExecutor = new DeferredCallbackExecutor();

//         Thread service = new Thread(new ThreadStart(deferredCallbackExecutor.start));
//         service.Start();

//         for (int i = 0; i < 5; i++)
//         {
//             Thread thread = new Thread(new ParameterizedThreadStart(deferredCallbackExecutor.registerCallback));
//             allThreads[i] = thread;

//             DeferredAction action = new DeferredAction(1, "Hello this is thread # " + i);
//             thread.Start(action);
//             Thread.Sleep(random.Next(1, 5) * 1000);
//         }

//         for (int i = 0; i < 5; i++)
//         {
//             allThreads[i].Join();
//         }
//     }

//     public void run2()
//     {
//         DeferredAction action1 = new DeferredAction(3, "Thread A ");
//         DeferredAction action2 = new DeferredAction(2, "Thread B ");
//         DeferredAction action3 = new DeferredAction(1, "Thread C ");
//         DeferredAction action4 = new DeferredAction(7, "Thread D ");

//         DeferredCallbackExecutor deferredCallbackExecutor = new DeferredCallbackExecutor();

//         Thread service = new Thread(new ThreadStart(deferredCallbackExecutor.start));
//         service.Start();

//         deferredCallbackExecutor.registerCallback(action1);
//         deferredCallbackExecutor.registerCallback(action2);
//         deferredCallbackExecutor.registerCallback(action3);
//         deferredCallbackExecutor.registerCallback(action4);

//         service.Join();
//     }
// }

