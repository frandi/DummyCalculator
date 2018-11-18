using System;
using System.Linq;

namespace DummyCalculator.Extensions
{
    public static class ArrayExtensions
    {
        public static TResult[] Slice<TSource, TResult>(this TSource[] source, int start)
        {
            return Slice<TSource, TResult>(source, start, 0);
        }

        public static TResult[] Slice<TSource, TResult>(this TSource[] source, int start, int end)
        {
            // Handles negative ends.
            if (end <= 0)
            {
                end = source.Length + end;
            }
            int len = end - start;
            
            // Get new array.
            TSource[] result = new TSource[len];
            for (int i = 0; i < len; i++)
            {
                result[i] = source[i + start];
            }

            // Convert result
            return result.Select(r => (TResult)Convert.ChangeType(r, typeof(TResult))).ToArray();
        }
    }
}
