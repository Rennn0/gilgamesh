namespace EducativeIo.BoundedBuffer
{
    public class SemaTest
    {
        public void Run()
        {
            Sema sema = new Sema(3);
            Thread[] threads = new Thread[5];
            for (int i = 0; i < 5; i++)
            {
                int tid = i;
                threads[i] = new Thread(() =>
                {
                    sema.Acquire();
                    Console.WriteLine($"Thread {tid} has sema");
                    Thread.Sleep(new Random().Next(5000, 10000));
                    sema.Release();
                    Console.WriteLine($"Thread {tid} released sema");
                });
            }
            for (int i = 0; i < 5; i++)
            {
                threads[i].Start();
            }
            for (int i = 0; i < 5; i++)
            {
                threads[i].Join();
            }
        }
    }
    public class Sema
    {
        private int m_max;
        private int m_spent;
        private readonly object m_sync;
        public Sema(int max)
        {
            m_max = max;
            m_spent = 0;
            m_sync = new object();
        }
        public void Release()
        {
            Monitor.Enter(m_sync);

            while (m_spent <= 0)
            {
                Monitor.Wait(m_sync);
            }

            m_spent--;
            Monitor.Pulse(m_sync);

            Monitor.Exit(m_sync);
        }

        public void Acquire()
        {
            Monitor.Enter(m_sync);

            while (m_spent >= m_max)
            {
                Monitor.Wait(m_sync);
            }

            m_spent++;
            Monitor.Pulse(m_sync);

            Monitor.Exit(m_sync);
        }
    }
}