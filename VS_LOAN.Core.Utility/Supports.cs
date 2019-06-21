using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VS_LOAN.Core.Utility
{
    public static class Supports
    {
        public static DateTime? ConvertDateTime(string strDate)
        {
            try
            {
                var arr = strDate.Split('/');
                if (arr.Length != 3)
                {
                    return null;
                }
                return new DateTime(int.Parse(arr[2]), int.Parse(arr[1]), int.Parse(arr[0]));
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public static string GetDateTimeStringValue(DateTime dateTime)
        {
            TimeSpan dtspan = DateTime.Now - dateTime;
            string value = "";
            if (dtspan.Hours > 1)
            {
                value = dtspan.Hours + " hours ";
            }
            else if (dtspan.Hours > 0)
            {
                value = dtspan.Hours + " hour ";
            }
            if (dtspan.Minutes > 1)
                value += dtspan.Minutes + " minutes ";
            else if (dtspan.Minutes > 0)
                value += dtspan.Minutes + " minute ";
            if (dtspan.Minutes == 0 && dtspan.Hours == 0)
            {
                if(dtspan.Seconds>1)
                    value += dtspan.Seconds + " seconds";
                else
                    value += dtspan.Seconds + " second";
            }
            if (dtspan.Days > 1)
                value = dtspan.Days + " days";
            else if (dtspan.Days > 0)
                value = dtspan.Days + " day";
            if (dtspan.Days >= 30)
                value = dtspan.Days / 30 + " month(s)";
            if (dtspan.Days >= 365)
                value = dtspan.Days / 365 + " year(s) ";
            value = value + " ago.";
            return value;
        }
        static readonly string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
        public  static string SizeSuffix(Int64 value)
        {
            if (value < 0) { return "-" + SizeSuffix(-value); }
            if (value == 0) { return "0.0 bytes"; }

            int mag = (int)Math.Log(value, 1024);
            decimal adjustedSize = (decimal)value / (1L << (mag * 10));

            return string.Format("{0:n1} {1}", adjustedSize, SizeSuffixes[mag]);
        }
       

    }
}
