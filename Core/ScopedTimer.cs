using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Core
{
    public class ScopedTimer : IDisposable
    {
        private readonly string m_title;
        private readonly Stopwatch m_stopwatch;
        private readonly Action<string, long> m_logger;

        public ScopedTimer(
            Action<string, long>? logger = null,
            [CallerMemberName] string member = "",
            [CallerFilePath] string file = "",
            [CallerLineNumber] int line = 0
        )
        {
            m_title = $"{member} at ({file}:{line})";
            m_stopwatch = Stopwatch.StartNew();
            m_logger = logger ?? DefaultLogger;
        }

        private static void DefaultLogger(string title, long ms)
        {
            Console.WriteLine($"[Scoped Timer] {title} took {ms} ms");
        }

        public void Dispose()
        {
            m_stopwatch.Stop();
            m_logger.Invoke(m_title, m_stopwatch.ElapsedMilliseconds);
        }
    }
}
