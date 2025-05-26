namespace EducativeIo.BoundedBuffer
{
    public class BarrierTest
    {
        private Barrier barrier = new Barrier(3);

        public void threadTask(Object? sleepFor)
        {
            Thread.Sleep((int)sleepFor!);
            Console.WriteLine(String.Format("Thread with id {0} reached the barrier", Thread.CurrentThread.ManagedThreadId));
            barrier.Arrive();

            Thread.Sleep((int)sleepFor);
            Console.WriteLine(String.Format("Thread with id {0} reached the barrier", Thread.CurrentThread.ManagedThreadId));
            barrier.Arrive();

            Thread.Sleep((int)sleepFor);
            Console.WriteLine(String.Format("Thread with id {0} reached the barrier", Thread.CurrentThread.ManagedThreadId));
            barrier.Arrive();
        }

        public void run()
        {
            Thread t1 = new Thread(new ParameterizedThreadStart(threadTask));
            Thread t2 = new Thread(new ParameterizedThreadStart(threadTask));
            Thread t3 = new Thread(new ParameterizedThreadStart(threadTask));

            t1.Start(50);
            t2.Start(50);
            t3.Start(50);

            t1.Join();
            t2.Join();
            t3.Join();

        }
    }
    public class Barrier
    {
        private int m_size;
        private int m_arrived;
        private int m_released;
        private readonly object mr_sync;

        public Barrier(int size)
        {
            m_size = size;
            m_arrived = 0;
            m_released = 0;
            mr_sync = new object();
        }

        public void Arrive()
        {
            Monitor.Enter(mr_sync);
            m_arrived++;

            while (m_arrived != m_size)
            {
                Monitor.Wait(mr_sync);
            }
            m_released++;

            if (m_released == m_size)
            {
                m_arrived = 0;
                m_released = 0;
            }
            Console.WriteLine(String.Format("Thread with id {0} released", Thread.CurrentThread.ManagedThreadId));

            Monitor.PulseAll(mr_sync);
            Monitor.Exit(mr_sync);
        }
    }
}