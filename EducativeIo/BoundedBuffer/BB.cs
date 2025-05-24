using System.Diagnostics.Metrics;

namespace EducativeIo.BoundedBuffer
{
    public class BBTest
    {
        private readonly BoundedBuffer<int> _bb;
        private readonly BoundedBuffer<string> _logs;
        private readonly AutoResetEvent _are = new AutoResetEvent(false);
        private long _counter = 0;
        public BBTest(int capacity)
        {
            _bb = new BoundedBuffer<int>(capacity);
            _logs = new BoundedBuffer<string>(100);
        }
        private void ConsumerThread()
        {
            try
            {
                while (true)
                {
                    int item = _bb.Dequeue();
                    Interlocked.Increment(ref _counter);
                    // _logs.Enqueue($"Consumed: {item} at {DateTime.Now:HH:mm:ss:ffffff}");
                    // _are.Set();
                    // Thread.Sleep(new Random().Next(3000, 5500));
                }
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
        private void ProducerThread()
        {
            try
            {
                while (true)
                {
                    int item = new Random().Next(1, 100);
                    _bb.Enqueue(item);
                    // _logs.Enqueue($"Produced: {item} at {DateTime.Now:HH:mm:ss:ffffff}");
                    // _are.Set();
                    // Thread.Sleep(new Random().Next(500, 1500));
                }
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        private void LoggerThread()
        {
            while (true)
            {
                _are.WaitOne();
                string log = _logs.Dequeue();
                Console.WriteLine($"Log: {log} ");
            }
        }

        private void CounterThread()
        {
            try
            {
                while (true)
                {
                    long before = Interlocked.Read(ref _counter);
                    Thread.Sleep(1000);
                    long after = Interlocked.Read(ref _counter);
                    Console.WriteLine(after - before);
                }
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        public void Run()
        {
            for (int i = 0; i < 1; i++)
            {
                ThreadPool.QueueUserWorkItem(state => ConsumerThread());
            }

            for (int i = 0; i < 5; i++)
            {
                ThreadPool.QueueUserWorkItem(state => ProducerThread());
            }

            // ThreadPool.QueueUserWorkItem(state => LoggerThread());
            ThreadPool.QueueUserWorkItem(state => CounterThread());
            Thread.Sleep(10000);

            // for (int i = 0; i < _logs.Count; i++)
            // {
            //     Console.WriteLine(_logs.Dequeue());
            // }
        }
    }
    public class BoundedBuffer<T>
    {
        private T[] _buffer;
        private int _capacity;
        private int _occupied;
        private int _head;
        private int _tail;
        private Mutex _mutex;
        private object _sync;
        public BoundedBuffer(int capacity)
        {
            _capacity = capacity;
            _buffer = new T[capacity];
            _occupied = 0;
            _head = 0;
            _tail = 0;
            _mutex = new Mutex();
            _sync = new object();
        }
        public int Count => _occupied;
        public void Enqueue(T item)
        {
            // _mutex.WaitOne();
            // while (_occupied == _capacity)
            // {
            //     _mutex.ReleaseMutex();
            //     _mutex.WaitOne();
            // }
            Monitor.Enter(_sync);
            while (_occupied == _capacity) Monitor.Wait(_sync);
            if (_tail == _capacity) _tail = 0;

            _buffer[_tail] = item;
            _occupied++;
            _tail++;

            // Monitor.Pulse(_sync);
            Monitor.PulseAll(_sync);
            Monitor.Exit(_sync);
            // _mutex.ReleaseMutex();
        }

        public T Dequeue()
        {
            // _mutex.WaitOne();
            // while (_occupied == 0)
            // {
            //     _mutex.ReleaseMutex();
            //     _mutex.WaitOne();
            // }

            Monitor.Enter(_sync);
            while (_occupied == 0) Monitor.Wait(_sync);

            if (_head == _capacity) _head = 0;
            T item = _buffer[_head];
            _head++;
            _occupied--;

            // Monitor.Pulse(_sync);
            Monitor.PulseAll(_sync);
            Monitor.Exit(_sync);
            // _mutex.ReleaseMutex();
            return item;
        }
    }
}