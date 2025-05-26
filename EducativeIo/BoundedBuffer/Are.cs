namespace EducativeIo.BoundedBuffer
{
    public class Are // auto reset event
    {
        private bool m_isSignaled;
        private readonly object m_sync;
        private readonly Semaphore mr_semaSync;
        private readonly Semaphore mr_sema;
        public Are(bool isSignaled = false)
        {
            m_isSignaled = isSignaled;
            mr_sema = new Semaphore(isSignaled ? 1 : 0, 1);
            mr_semaSync = new Semaphore(1, 1);
            m_sync = new object();
            // m_sync = new object();
        }
        public void Signal()
        {
            // Monitor.Enter(m_sync);
            // m_isSignaled = true;
            // Monitor.Pulse(m_sync);
            // Monitor.Exit(m_sync);

            mr_semaSync.WaitOne();
            if (!m_isSignaled)
            {
                m_isSignaled = true;
                mr_sema.Release();
            }
            mr_semaSync.Release();
        }
        public void Wait()
        {
            // Monitor.Enter(m_sync);
            // while (!m_isSignaled)
            // {
            //     Monitor.Wait(m_sync);
            // }
            // m_isSignaled = false;
            // Monitor.Exit(m_sync);

            mr_semaSync.WaitOne();

            while (!m_isSignaled)
            {
                mr_semaSync.Release();
                mr_sema.WaitOne();
                mr_semaSync.WaitOne();
            }
            m_isSignaled = false;

            mr_semaSync.Release();
        }
    }
}