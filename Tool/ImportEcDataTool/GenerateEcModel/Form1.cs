using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GenerateEcModel
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        protected readonly string baseFolderPath = @"C:\Users\QuangPhat\Desktop\sql.txt";
        protected string _tableName;
        private void btnGen_Click(object sender, EventArgs e)
        {
            _tableName = txtPath.Text.Trim();
            ReadFile(baseFolderPath);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _tableName = string.Empty;
        }
        public string ReadFile(string fileName)
        {

            if (!File.Exists(fileName))
                return string.Empty;
            string clause1 = string.Empty;
            string clause2 = string.Empty;
            string s = string.Empty;
            using (StreamReader sr = new StreamReader(fileName))
            {
                string line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    GenerateAddParamDapper(line, ref clause1);
                }
            }
            //string fullclasuse = $"insert into {_tableName} ({clause1}) values ({clause2})";
            Console.WriteLine(clause1);
            return string.Empty;
        }
        private void GenerateAddParamDapper(string input, ref string clause1)
        {
            input = input.Replace("\t", "");
            var arrStr = input.Split(',');
            clause1 += $"p.Add(\"{arrStr[0]}\", model.{arrStr[0]});" + Environment.NewLine;

        }
        private void GenerateInputStore(string input, ref string clause1)
        {
            input = input.Replace("\t", "");
            var arrStr = input.Split(',');
            clause1 += $"@{arrStr[0]} {arrStr[1]}({arrStr[2]})," + Environment.NewLine;
            
        }
        private void GenerateInsertStore(string input, ref string clause1, ref string clause2)
        {
            var arrStr = input.Split(',');
            clause1 += $"{arrStr[0]},";
            clause2 += $"@{arrStr[0]},";
        }
    }
}
