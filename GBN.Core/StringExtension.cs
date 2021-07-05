using System;
using System.Linq;

namespace UnivIntel.Common
{
    public static class StringExtension
    {
        
        public static string RemoveWhitespace(this string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return null;

            return new string(input.ToCharArray()
                .Where(c => !char.IsWhiteSpace(c))
                .ToArray());
        }

        public static double? GetDoubleInterval(TimeSpan value)
        {
            if (value.TotalMilliseconds == 0)
            {
                return null;
            }
            var doubleValue = value.TotalMilliseconds / 1000 / 60 / 60;
            return double.Parse(String.Format("{0:0.00}", doubleValue));
        }
    }
}
