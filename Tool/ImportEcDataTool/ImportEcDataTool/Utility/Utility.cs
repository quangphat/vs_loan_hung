using ImportEcDataTool.Models;
using LumenWorks.Framework.IO.Csv;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImportEcDataTool.Utility
{
    public static class Utility
    {
        public static string GetFilePath()
        {
            string filePath = string.Empty;
            using (OpenFileDialog fileDialog = new OpenFileDialog())
            {
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = fileDialog.FileName;
                }
            }
            return filePath;
        }
        
    }
}
