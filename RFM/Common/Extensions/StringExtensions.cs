using System;

namespace RFM.Common.Extensions
{
    public static class StringExtensions
    {
        public static bool Contains(this string input, string searchText, StringComparison stringComparison)
        {
            return string.IsNullOrEmpty(searchText) || input.IndexOf(searchText, stringComparison) >= 0;
        }
    }
}
