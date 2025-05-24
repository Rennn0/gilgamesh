using System.Diagnostics;

namespace EducativeIo.BoundedBuffer
{
    public class TokenBucketFilterTest
    {

        public void Run()
        {
            RateLimiter tokenBucketFilter = new RateLimiter(500, 0, 1_00_000);
            for (int i = 0; i < 5; i++)
            {
                new Thread(() =>
                {
                    while (true)
                    {
                        tokenBucketFilter.GetToken();
                    }
                }).Start();
            }

            new Thread(() =>
            {
                while (true)
                {
                    long a = tokenBucketFilter.Count;
                    Thread.Sleep(1000);
                    long b = tokenBucketFilter.Count;

                    Console.WriteLine($"TPS = {b - a}");
                }

            }).Start();
        }
    }
    public class RateLimiter
    {
        private const int c_penalty = 1;
        private long m_tps;
        private long mr_max;
        private double m_lastRequestTimeMicroSeconds;
        private double m_possibleTokens;
        private readonly Mutex mr_mutex;
        private readonly DateTime mr_start;
        private long m_tokensCreated;
        private Stopwatch m_stopwatch;
        public RateLimiter(long maxTokens, long initialTokens = 0, long tokenPerSecond = 100)
        {
            m_stopwatch = Stopwatch.StartNew();

            mr_max = maxTokens;
            m_tps = tokenPerSecond;
            m_possibleTokens = Math.Min(maxTokens, initialTokens);
            mr_mutex = new Mutex();
            mr_start = new DateTime(1970, 1, 1);
            m_lastRequestTimeMicroSeconds = GetEpochTimeMicroSeconds();
            m_tokensCreated = 0;
        }
        public long Count => Interlocked.Read(ref m_tokensCreated);
        public void GetToken()
        {
            mr_mutex.WaitOne();

            double now = GetEpochTimeMicroSeconds();
            double elapsed = now - m_lastRequestTimeMicroSeconds;

            double accumulatedTokens = (elapsed / 1_000_000d) * m_tps;

            m_possibleTokens = Math.Min(mr_max, m_possibleTokens + accumulatedTokens);
            m_lastRequestTimeMicroSeconds = GetEpochTimeMicroSeconds();

            // Console.WriteLine($"{m_possibleTokens} {accumulatedTokens}");

            if (m_possibleTokens >= 1d)
            {
                m_possibleTokens -= 1d;
                mr_mutex.ReleaseMutex();

                Interlocked.Increment(ref m_tokensCreated);
            }
            else
            {
                mr_mutex.ReleaseMutex();
                Thread.Sleep(c_penalty);
                GetToken();
                return;
            }
        }

        private double GetEpochTimeMicroSeconds() => (m_stopwatch.Elapsed).TotalMicroseconds;
    }

}