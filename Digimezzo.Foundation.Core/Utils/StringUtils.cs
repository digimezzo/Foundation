using System;
using System.Linq;

namespace Digimezzo.Foundation.Core.Utils
{
    public static class StringUtils
    {
        /// <summary>
        /// Makes the first character of a string uppercase
        /// </summary>
        /// <param name="input">The input string</param>
        /// <returns>The input string with the first character in uppercase</returns>
        public static string FirstCharToUpper(string input)
        {
            if (String.IsNullOrEmpty(input)) return input;
            return input.First().ToString().ToUpper() + input.Substring(1);
        }
    }
}
