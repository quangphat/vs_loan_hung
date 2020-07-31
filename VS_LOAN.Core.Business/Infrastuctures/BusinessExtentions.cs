using Dapper;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity;

namespace VS_LOAN.Core.Business.Infrastuctures
{
    public static class BusinessExtentions
    {
        public static void GetObjectParams(List<ImportExcelFrameWorkModel> frames)
        {
            List<DynamicParameters> pars = new List<DynamicParameters>();
            var param = new DynamicParameters();
            //foreach(var row in frames.Rows)
            //{
            //    var properties = row.GetType().GetProperties();
            //    foreach (var prop in properties)
            //    {
            //        var key = prop.Name;
            //        var value = prop.GetValue(row);
            //        param.Add(key, value);
            //    }
            //    var x = row;
            //    param.Add("", 1);
            //}
        }
        public static object TryGetValueFromCell(string value, string type)
        {
            if (type == "string")
                return value;
            if (type == "int")
            {
                if (string.IsNullOrWhiteSpace(value))
                    return 0;
                try
                {
                    return Convert.ToInt32(value);
                }
                catch
                {
                    return 0;
                }
            }
                
            return string.Empty;
        }
    }
}
