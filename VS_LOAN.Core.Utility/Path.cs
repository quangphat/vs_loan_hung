using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Utility
{
   public class Path
    {
        public static  string DownloadBill = "/App_Data/Download/";
        public static string ReportTemplate = "/App_Data/TemplateReport/";
        public static string UploadFile(string root,string fileName)
        {
            fileName =  DateTime.Now.ToString("yyyyMMMddHHmmssfff") + "_" + fileName;
            fileName = FileNameProcessor.StandardizeFilexNames(fileName);
            string path = string.Empty;
            if (!Directory.Exists(root))
                System.IO.Directory.CreateDirectory(root);
            string pathYear = System.IO.Path.Combine(root, DateTime.Now.Year.ToString());
            if (!Directory.Exists(pathYear))
                System.IO.Directory.CreateDirectory(pathYear);
            string pathMonth = System.IO.Path.Combine(pathYear, DateTime.Now.Month.ToString());
            if (!Directory.Exists(pathMonth))
                System.IO.Directory.CreateDirectory(pathMonth);
            string pathDay = System.IO.Path.Combine(pathMonth, DateTime.Now.Day.ToString());
            if (!Directory.Exists(pathDay))
                System.IO.Directory.CreateDirectory(pathDay);
             path = System.IO.Path.Combine(pathDay, fileName);
            return path;
        }
    }
    
}
