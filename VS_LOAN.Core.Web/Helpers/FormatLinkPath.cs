using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace VS_LOAN.Core.Web.Helpers
{
    public class FormatLinkPath
    {
        public static string GetSafeFilename(string filename)
        {
            //return Path.GetInvalidFileNameChars().Aggregate(filename, (current, c) => current.Replace(c.ToString(), string.Empty));
            //return string.Join("_", filename.Split(Path.GetInvalidFileNameChars()));
           return System.Text.RegularExpressions.Regex.Replace(filename, @"[^a-zA-Z0-9'.@]", string.Empty).Trim();
        }
    }
}