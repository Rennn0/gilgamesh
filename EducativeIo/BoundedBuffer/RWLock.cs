namespace EducativeIo.BoundedBuffer
{
    public class ReadersWriteLockTest
    {
        RWLock rwLock = new RWLock();
        Random random = new Random();
        readonly DateTime epochStart = new DateTime(1970, 1, 1);
        private volatile bool shutdown = false;

        public void writerThread()
        {
            while (!shutdown)
            {
                rwLock.AcquireWrite();

                Console.WriteLine(String.Format("\nwriter-{0} writing at {1} and current readers = {2}", Thread.CurrentThread.ManagedThreadId, getEpochTime(), rwLock.getReaders()));

                int writeFor = random.Next(1, 6) * 1000;
                Thread.Sleep(writeFor);

                Console.WriteLine(String.Format("\nwriter-{0} releasing at {1} and current readers = {2}", Thread.CurrentThread.ManagedThreadId, getEpochTime(), rwLock.getReaders()));

                rwLock.ReleaseWrite();
                Thread.Sleep(1000);
            }
        }

        private long getEpochTime()
        {
            TimeSpan span = DateTime.UtcNow - epochStart;
            return (long)span.TotalSeconds;
        }

        public void readerThread()
        {
            while (!shutdown)
            {
                rwLock.AcquireRead();

                Console.WriteLine(String.Format("\nreader-{0} reading at {1} and write in progress = {2}", Thread.CurrentThread.ManagedThreadId, getEpochTime(),
                   rwLock.isWriteInProgress()));

                int readFor = random.Next(1, 3) * 1000;
                Thread.Sleep(readFor);

                Console.WriteLine(String.Format("\nreader-{0} releasing at {1} and write in progress = {2}", Thread.CurrentThread.ManagedThreadId, getEpochTime(),
                   rwLock.isWriteInProgress()));

                rwLock.ReleaseRead();
                Thread.Sleep(1000);
            }
        }

        public void run()
        {

            Thread writer1 = new Thread(new ThreadStart(writerThread));
            Thread writer2 = new Thread(new ThreadStart(writerThread));

            writer1.Start();

            Thread[] readers = new Thread[3];

            for (int i = 0; i < 3; i++)
            {
                readers[i] = new Thread(new ThreadStart(readerThread));
            }

            for (int i = 0; i < 3; i++)
            {
                readers[i].Start();
            }

            writer2.Start();

            Thread.Sleep(15000);
            shutdown = true;

            for (int i = 0; i < 3; i++)
            {
                readers[i].Join();
            }

            writer1.Join();
            writer2.Join();
        }
    }

    public class RWLock
    {
        private int m_readers;
        private bool m_isWriting;
        private readonly object mr_sync;
        public RWLock()
        {
            m_readers = 0;
            m_isWriting = false;
            mr_sync = new object();
        }
        public int getReaders() => m_readers;
        public bool isWriteInProgress() => m_isWriting;
        public void AcquireRead()
        {
            Monitor.Enter(mr_sync);
            while (m_isWriting)
            {
                Monitor.Wait(mr_sync);
            }
            m_readers++;
            Monitor.Exit(mr_sync);
        }
        public void AcquireWrite()
        {
            Monitor.Enter(mr_sync);
            while (m_readers != 0 || m_isWriting)
            {
                Monitor.Wait(mr_sync);
            }
            m_isWriting = true;
            Monitor.PulseAll(mr_sync);
            Monitor.Exit(mr_sync);
        }
        public void ReleaseRead()
        {
            Monitor.Enter(mr_sync);
            m_readers--;

            Monitor.PulseAll(mr_sync);
            Monitor.Exit(mr_sync);
        }
        public void ReleaseWrite()
        {
            Monitor.Enter(mr_sync);
            m_isWriting = false;

            Monitor.PulseAll(mr_sync);
            Monitor.Exit(mr_sync);
        }
    }
}