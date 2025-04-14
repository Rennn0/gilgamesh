using System.Text;
using BenchmarkDotNet.Attributes;

namespace __BENCH__.Source
{
    [MemoryDiagnoser]
    [ShortRunJob]
    public class StringManipulation
    {

        [Params(10, 100)]
        public int N;
        [Benchmark(Baseline = true)]
        public string ConcatWithPlus()
        {
            string result = string.Empty;
            for (int i = 0; i < N; i++)
            {
                result += "a";
            }
            return result;  
        }

        [Benchmark]
        public string ConcatWithStringBuilder()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < N; i++)
            {
                sb.Append('a');
            }
            return sb.ToString();
        }
    }
}