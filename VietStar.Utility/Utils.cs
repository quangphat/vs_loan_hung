using System;
using System.Collections.Generic;
using System.Linq;

namespace VietStar.Utility
{
    public static class Utils
    {
        static string start = "KPMG_EV";
        static string end = "KPMG_PM";
        public static string getMD5(string data)
        {
            return BitConverter.ToString(encryptData(start + data + end)).Replace("-", "").ToLower();
        }
        private static byte[] encryptData(string data)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md5Hasher = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] hashedBytes;
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            hashedBytes = md5Hasher.ComputeHash(encoder.GetBytes(data));
            return hashedBytes;
        }
        public static bool IsSubsetOf<T>(this IEnumerable<T> a, IEnumerable<T> b)
        {
            return !a.Except(b).Any();
        }
        public static TimeSpan GetTimeZoneOffset(string timeZoneId)
        {
            TimeZoneInfo systemTimeZoneById;
            if (string.IsNullOrEmpty(timeZoneId) || (systemTimeZoneById = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId)) == null)
                return new TimeSpan(0L);
            return systemTimeZoneById.BaseUtcOffset;
        }

        public static DateTime GetStartDate(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
        }

        public static DateTime GetEndDate(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
        }
        public static DateTime ToStartDateTime(this object item)
        {
            return item.ToDateTime(new DateTime()).StartDateTime();
        }
        public static DateTime ToDateTime(this object item, DateTime defaultDateTime = default(DateTime))
        {
            DateTime result;
            if (string.IsNullOrEmpty(item?.ToString()) || !DateTime.TryParse(item.ToString(), out result))
                return defaultDateTime;
            return result;
        }
        public static DateTime StartDateTime(this DateTime item)
        {
            return new DateTime(item.Year, item.Month, item.Day);
        }
        public static DateTime? StartDateTime(this DateTime? item)
        {
            if (!item.HasValue)
                return new DateTime?();
            return new DateTime?(new DateTime(item.Value.Year, item.Value.Month, item.Value.Day));
        }
        public static DateTime? ToStartDateTimeNull(this object item)
        {
            return item.ToDateTimeNull().StartDateTime();
        }
        public static DateTime? ToDateTimeNull(this object item)
        {
            if (string.IsNullOrEmpty(item?.ToString()))
                return new DateTime?();
            DateTime result;
            if (!DateTime.TryParse(item.ToString(), out result))
                return new DateTime?();
            return new DateTime?(result);
        }
        public static DateTime ToEndDateTime(this object item)
        {
            return item.ToDateTime(new DateTime()).EndDateTime();
        }

        public static DateTime? ToEndDateTimeNull(this object item)
        {
            return item.ToDateTimeNull().EndDateTime();
        }
        public static DateTime EndDateTime(this DateTime item)
        {
            return new DateTime(item.Year, item.Month, item.Day, 23, 59, 59);
        }

        public static DateTime? EndDateTime(this DateTime? item)
        {
            if (!item.HasValue)
                return new DateTime?();
            return new DateTime?(new DateTime(item.Value.Year, item.Value.Month, item.Value.Day, 23, 59, 59));
        }
    }
}
