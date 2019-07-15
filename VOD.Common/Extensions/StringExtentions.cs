using System;
using System.Collections.Generic;
using System.Text;

namespace VOD.Common.Extensions
{
    public static class StringExtentions
    {
        public static string Trancate(this string value, int length)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            if (value.Length <= length) return value;
            return $"{value.Substring(0, length)}...";
        }

        public static bool IsNullOrEmptyOrWhiteSpace(this string value)
        {
            return string.IsNullOrEmpty(value) ||
                string.IsNullOrWhiteSpace(value);
        }

        public static bool IsNullOrEmptyOrWhiteSpace(this string[] value)
        {
            foreach (var val in value)
            {
                if (IsNullOrEmptyOrWhiteSpace(val)) return true;
            }
            return false;
        }
    }
}
