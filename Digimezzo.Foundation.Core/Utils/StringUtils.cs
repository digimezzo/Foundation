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

        /// <summary>
        /// Splits strings on spaces, but preserves strings which are between double quotes.
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static string[] SplitWords(string inputString)
        {
            return inputString.Split('"')
                     .Select((element, index) => index % 2 == 0  // If even index
                                           ? element.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)  // Split the item
                                           : new string[] { element })  // Keep the entire item
                     .SelectMany(element => element).ToArray();
        }
    }
}
