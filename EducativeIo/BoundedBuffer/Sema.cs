namespace EducativeIo.BoundedBuffer
{
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
            Monitor.PulseAll(m_sync);
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
            Monitor.PulseAll(m_sync);
            Monitor.Exit(m_sync);
        }
    }
}