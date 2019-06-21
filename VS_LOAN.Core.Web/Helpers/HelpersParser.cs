using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VS_LOAN.Core.Web.Helpers
{
    public class HelpersParser
    {
        public static List<Dictionary<string, object>> Convert(List<Object> lstObject)
        {
            List<Dictionary<string, object>> lstRows = new List<Dictionary<string, object>>();
            //Dictionary<string, object> dictRow = null;

           /* foreach (DataRow dr in dtData.Rows)
            {
                dictRow = new Dictionary<string, object>();
                foreach (DataColumn col in dtData.Columns)
                {
                    dictRow.Add(col.ColumnName, dr[col]);
                }
                lstRows.Add(dictRow);
            }*/
            return lstRows;
        }
    }
}