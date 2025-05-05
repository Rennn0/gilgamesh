using System.Numerics;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;

namespace __BENCH__.Source;

//  SIMD (Single Instruction, Multiple Data)
[ShortRunJob]
[MemoryDiagnoser]
[MarkdownExporter]
public class SimdMimpl
{
    [Params(10, 100, 1000, 10000)]
    public int A { get; set; }

    [Params(20, 200, 2000, 20000)]
    public int B { get; set; }

    public int[] Span;

    [GlobalSetup]
    public void Setup()
    {
        Random r = new Random(0);
        Span = Enumerable.Range(0, A).Select(_ => r.Next(100)).ToArray();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Benchmark]
    public Vector<int> SumInlining()
    {
        int[] leftArray = new int[Vector<int>.Count];
        int[] rightArray = new int[Vector<int>.Count];

        leftArray[0] = A;
        rightArray[0] = B;

        Vector<int> l = new Vector<int>(leftArray);
        Vector<int> r = new Vector<int>(rightArray);

        Vector<int> sum = SumInlining(l, r);
        return sum;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector<int> SumInlining(Vector<int> a, Vector<int> b) => a + b;

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public Vector<int> SumOptimization(Vector<int> a, Vector<int> b) => a + b;

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    [Benchmark]
    public Vector<int> SumOptimization()
    {
        int[] leftArray = new int[Vector<int>.Count];
        int[] rightArray = new int[Vector<int>.Count];

        leftArray[0] = A;
        rightArray[0] = B;

        Vector<int> l = new Vector<int>(leftArray);
        Vector<int> r = new Vector<int>(rightArray);

        Vector<int> sum = SumOptimization(l, r);
        return sum;
    }

    [Benchmark]
    public int SumSpan()
    {
        Span<int> nums = new Span<int>(Span);
        int sum = 0;
        for (int i = 0; i < nums.Length; i++)
        {
            sum += nums[i];
        }

        return sum;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Benchmark]
    public int SumSpanInlining()
    {
        Span<int> nums = new Span<int>(Span);
        int sum = 0;
        for (int i = 0; i < nums.Length; i++)
        {
            sum += nums[i];
        }

        return sum;
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    [Benchmark]
    public int SumSpanOptimization()
    {
        Span<int> nums = new Span<int>(Span);
        int sum = 0;
        for (int i = 0; i < nums.Length; i++)
        {
            sum += nums[i];
        }

        return sum;
    }

    [Benchmark]
    public int SumLinqArr() => Span.Sum();

    [Benchmark]
    public int SumLinqList() => Span.ToList().Sum();
}
