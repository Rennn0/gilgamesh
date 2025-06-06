using BenchmarkDotNet.Attributes;

namespace __BENCH__.Source
{
    public struct MyStruct
    {
        public int X;
        public double Y;
    }
    [ShortRunJob, MemoryDiagnoser]
    public class ContainerBenchmark
    {
        private const int N = 1000;
        // [Benchmark(Baseline = true)]
        // public void NormalArray()
        // {
        //     int[] arr = new int[N];
        //     for (int i = 0; i < N; i++)
        //     {
        //         arr[i] = i * i;
        //     }

        //     for (int i = 0; i < N; i++)
        //     {
        //         Consume(arr[i]);
        //     }
        // }

        // [Benchmark()]
        // public void StackallocArray()
        // {
        //     Span<int> span = stackalloc int[N];
        //     for (int i = 0; i < N; i++)
        //     {
        //         span[i] = i * i;
        //     }

        //     for (int i = 0; i < N; i++)
        //     {
        //         Consume(span[i]);
        //     }
        // }

        // [Benchmark()]
        // public void NewArray()
        // {
        //     var arr = new int[N];
        //     for (int i = 0; i < N; i++)
        //     {
        //         arr[i] = i * i;
        //     }

        //     for (int i = 0; i < N; i++)
        //     {
        //         Consume(arr[i]);
        //     }
        // }

        [Benchmark(Baseline = true)]
        public void HeapArrayStruct()
        {
            var arr = new MyStruct[N];
            for (int i = 0; i < N; i++)
                arr[i] = new MyStruct { X = i, Y = i * 0.5 };
        }

        [Benchmark]
        public void StackallocArrayStruct()
        {
            Span<MyStruct> span = stackalloc MyStruct[N];
            for (int i = 0; i < N; i++)
                span[i] = new MyStruct { X = i, Y = i * 0.5 };
        }
        private void Consume(int value) { }

    }
}