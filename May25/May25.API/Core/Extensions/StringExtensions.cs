using System;

namespace May25.API.Core.Extensions
{
    public static class StringExtensions
    {
        public static int[] ToIntArray(this string value, char separator = ',')
        {
            if (value == null)
            {
                return Array.Empty<int>();
            }

            var values = value.Split(separator);
            var result = new int[values.Length];

            for (int i = 0; i < values.Length; i++)
            {
                result[i] = int.Parse(values[i]);
            }

            return result;
        }

        public static string SplitAndGetPosition(this string value, char separator = ',', int position = 0)
        {
            if (value == null)
            {
                return string.Empty;
            }

            var array = value.Split(separator);
            
            if (position > array.Length-1)
            {
                return string.Empty;
            }

            return array[position];
        }
    }
}
