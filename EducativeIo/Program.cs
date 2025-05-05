using System.Numerics;
using System.Runtime.CompilerServices;

namespace EducativeIo;

internal class Solution
{
    public static void Main(string[] args)
    {
        int[] leftArray = new int[Vector<int>.Count];
        int[] rightArray = new int[Vector<int>.Count];

        leftArray[0] = 1;
        leftArray[1] = 2;
        leftArray[2] = 3;

        rightArray[0] = 4;
        rightArray[1] = 5;
        rightArray[2] = 6;

        Vector<int> l = new Vector<int>(leftArray);
        Vector<int> r = new Vector<int>(rightArray);

        Vector<int> sum = Sum(l, r);

        Console.WriteLine($"Sum: {sum}");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector<int> Sum(Vector<int> a, Vector<int> b) => a + b;
}
