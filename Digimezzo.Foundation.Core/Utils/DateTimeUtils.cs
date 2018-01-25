using System;
using System.Globalization;

namespace Digimezzo.Foundation.Core.Utils
{
    public static class DateTimeUtils
    {
        /// <summary>
        /// Converts a DateTime to Unix time
        /// </summary>
        /// <param name="dateTime">The DateTime to convert to Unix time</param>
        /// <returns>The Unix time for the given DateTime</returns>
        public static long ConvertToUnixTime(DateTime dateTime)
        {
            DateTime referenceTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (long)(dateTime.ToUniversalTime() - referenceTime).TotalSeconds;
        }

        /// <summary>
        /// Validates a date, taking leap years into account.
        /// </summary>
        /// <param name="year">The year of the date to validate</param>
        /// <param name="month">The month of the date to validate</param>
        /// <param name="day">The day of the date to validate</param>
        /// <returns>True if the date is valid</returns>
        public static bool IsValidDate(int year, int month, int day)
        {
            DateTime dateValue;

            if (DateTime.TryParseExact($"{year}-{month.ToString("D2")}-{day.ToString("D2")}", "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue))
            {
                return true;
            }

            return false;
        }
    }
}
