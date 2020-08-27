using System;
using VS_LOAN.Core.Entity;

namespace VS_LOAN.Core.Utility
{
    public static class DateTimeFormat
    {
        public static DateTime ConvertddMMyyyyToDateTime(string str)
        {
            string[] p = str.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
            DateTime date = new DateTime(Convert.ToInt32(p[2]), Convert.ToInt32(p[1]), Convert.ToInt32(p[0]));
            return date;

        }
        public static DateTime ConvertddMMyyyyToDateTimeNew(string str)
        {
            string[] p = str.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            DateTime date = new DateTime(Convert.ToInt32(p[0]), Convert.ToInt32(p[1]), Convert.ToInt32(p[2]));
            return date;

        }

        public static DatetimeConvertModel ConvertddMMyyyyToDateTimeV2(string str)
        {
            try
            {
                string[] p = str.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
                DateTime date = new DateTime(Convert.ToInt32(p[2]), Convert.ToInt32(p[1]), Convert.ToInt32(p[0]));
                return new DatetimeConvertModel {
                    Message = string.Empty,
                    Value = date,
                    Success = true
                };
            }
            catch(Exception e)
            {
                return new DatetimeConvertModel
                {
                    Message = "Ngày tháng không đúng định dạng. Vd: 30-12-2019",
                    Value = DateTime.Now,
                    Success = false
                };
            }

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