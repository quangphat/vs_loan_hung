using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VS_LOAN.Core.Utility
{
    public class DateTimeFormat
    {
        public static DateTime ConvertddMMyyyyToDateTime(string str)
        {
            string[] p = str.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
            DateTime date = new DateTime(Convert.ToInt32(p[2]), Convert.ToInt32(p[1]), Convert.ToInt32(p[0]));
            return date;

        }
        public static DateTime ConvertddMMyyyyHHssToDateTime(string str)
        {
            string ddMMYYYY = str.Substring(0,10);
            string[] aa = str.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            string HHMM = str.Substring(ddMMYYYY.Length+1,str.Length - ddMMYYYY.Length-1);
            string[] p = ddMMYYYY.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            string[] h = HHMM.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
            DateTime date = new DateTime(Convert.ToInt32(p[2]), Convert.ToInt32(p[1]), Convert.ToInt32(p[0]), Convert.ToInt16(h[0]),Convert.ToInt16(h[1]),0);
            return date;

        }
        public static DateTime ConvertyyyyMMddToDateTime(string str)
        {
            string[] p = str.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
            DateTime date = new DateTime(Convert.ToInt32(p[0]), Convert.ToInt32(p[1]), Convert.ToInt32(p[2]));
            return date;

        }
        public static object ConvertddMMyyyy(string str)
        {
            string[] p = str.Split(new string[] { "-","/","." }, StringSplitOptions.RemoveEmptyEntries);
            DateTime date = new DateTime(Convert.ToInt32(p[2]), Convert.ToInt32(p[1]), Convert.ToInt32(p[0]));
            return date;

        }
    }
}