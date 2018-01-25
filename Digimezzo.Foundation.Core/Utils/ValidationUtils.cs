using System.Text.RegularExpressions;

namespace Digimezzo.Foundation.Core.Utils
{
    public static class ValidationUtils
    {
        /// <summary>
        /// Validates a source URL
        /// </summary>
        /// <param name="source">The source URL</param>
        /// <returns>True if the URL is valid</returns>
        public static bool IsUrl(string source)
        {

            bool retVal = false;

            // As found at http://www.dotnetfunda.com/codes/show/1519/regex-pattern-to-validate-url
            Match m = Regex.Match(source, @"^(file|http|https):/{2}[a-zA-Z./&\d_-]+", RegexOptions.IgnoreCase);

            if ((m.Success))
            {
                retVal = true;
            }

            return retVal;
        }

        /// <summary>
        /// Validates a source e-mail address
        /// </summary>
        /// <param name="source">The source e-mail address</param>
        /// <returns>True if the e-mail address is valid</returns>
        public static bool IsEmail(string source)
        {

            bool retVal = false;

            // As found at http://www.regular-expressions.info/email.html
            Match m = Regex.Match(source, @"\b[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}\b", RegexOptions.IgnoreCase);

            if ((m.Success))
            {
                retVal = true;
            }

            return retVal;
        }
    }
}
