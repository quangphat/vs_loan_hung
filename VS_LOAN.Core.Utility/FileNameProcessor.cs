using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Utility
{
    public class FileNameProcessor
    {
        private static readonly int FILENAME_LENGTH = 100;
        public static string StandardizeFilexNames(string fileName)
        {
            string invalid = new string(System.IO.Path.GetInvalidFileNameChars()) + new string(System.IO.Path.GetInvalidPathChars()) + "+";
            string[] temp = fileName.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
            string dangFile = "";
            try
            {
                dangFile = "." + temp[temp.Length - 1];
            }
            catch (Exception)
            {
            }
            fileName = fileName.Replace(dangFile, "");
            foreach (char c in invalid)
            {
                fileName = fileName.Replace(c.ToString(), ""); 
            }
            if (fileName.Length > FILENAME_LENGTH)
                fileName = fileName.Substring(0, FILENAME_LENGTH);
            fileName += dangFile;
            return fileName;
        }
    }
}
