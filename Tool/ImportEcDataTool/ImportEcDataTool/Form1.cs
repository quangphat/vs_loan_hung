using ImportEcDataTool.Models;
using ImportEcDataTool.Repository;
using ImportEcDataTool.Utility;
using LumenWorks.Framework.IO.Csv;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
 
namespace ImportEcDataTool
{
    public partial class Form1 : Form
    {
        protected readonly EcDataRepository _rpEc;
        protected readonly string baseFolderPath = @"D:\Development\outsource\docs\Case Creation Master Data UAT\";
        public Form1()
        {
            InitializeComponent();
            _rpEc = new EcDataRepository();
            //txtPath.Text = baseFolderPath;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            txtPath.Text = Utility.Utility.GetFilePath();
        }
        

        private async void btnImport_Click(object sender, EventArgs e)
        {
            string fullPath = baseFolderPath + txtPath.Text;
            var result = await ReadEcLocation(fullPath);
            MessageBox.Show(result.ToString());
        }
        private async Task<bool> ReadEcProductInfo(string fileName)
        {
            //List<EcLocation> result = new List<EcLocation>();
            using (CsvReader csv = new CsvReader(new StreamReader(fileName), true))
            {
                int fieldCount = csv.FieldCount;

                string[] headers = csv.GetFieldHeaders();
                EcProductInfo obj = null;
                while (csv.ReadNextRecord())
                {
                    obj = new EcProductInfo
                    {
                        Id = Convert.ToInt32(csv[0]),
                        Code = Convert.ToString(csv[1]),
                        Description_Vi = csv[2].ToString(),
                        OccupationCode = Convert.ToString(csv[3]),
                        MinLoanAmountStr = Convert.ToString(csv[4]),
                        MaxLoanAmountStr = Convert.ToString(csv[5]),
                        MinTenor = Convert.ToInt32(csv[6]),
                        MaxTenor = Convert.ToInt32(csv[7])
                    };
                    await _rpEc.InsertProductInfo(obj);
                }
            }
            return true;
        }
        private async Task<bool> ReadEcPersonalInfo(string fileName)
        {
            //List<EcLocation> result = new List<EcLocation>();
            using (CsvReader csv = new CsvReader(new StreamReader(fileName), true))
            {
                int fieldCount = csv.FieldCount;

                string[] headers = csv.GetFieldHeaders();
                EcPersonalInfo obj = null;
                while (csv.ReadNextRecord())
                {
                    obj = new EcPersonalInfo
                    {
                        Id = Convert.ToInt32(csv[0]),
                        Code = Convert.ToString(csv[1]),
                        Description_Vi = csv[2].ToString(),
                        Description_En = Convert.ToString(csv[3]),
                        Type = Convert.ToString(csv[4])
                    };
                    await _rpEc.InsertPersonalInfo(obj);
                }
            }
            return true;
        }
        private async Task<bool> ReadEcIssuePlace(string fileName)
        {
            //List<EcLocation> result = new List<EcLocation>();
            using (CsvReader csv = new CsvReader(new StreamReader(fileName), true))
            {
                int fieldCount = csv.FieldCount;

                string[] headers = csv.GetFieldHeaders();
                EcIssuePlace obj = null;
                while (csv.ReadNextRecord())
                {
                    obj = new EcIssuePlace
                    {
                        Id = Convert.ToInt32(csv[0]),
                        Code = Convert.ToString(csv[1]),
                        Description_Vi = csv[2].ToString(),
                        Description_En = Convert.ToString(csv[3]),
                        
                    };
                    await _rpEc.InsertEcIssuePlace(obj);
                }
            }
            return true;
        }
        private async Task<bool> ReadEcEmployeeType(string fileName)
        {
            //List<EcLocation> result = new List<EcLocation>();
            using (CsvReader csv = new CsvReader(new StreamReader(fileName), true))
            {
                int fieldCount = csv.FieldCount;

                string[] headers = csv.GetFieldHeaders();
                EcEmployeeType obj = null;
                while (csv.ReadNextRecord())
                {
                    obj = new EcEmployeeType
                    {
                        Id = Convert.ToInt32(csv[0]),
                        RefCode = Convert.ToString(csv[1]),
                        Description_Vi = csv[2].ToString(),
                        Description_En = Convert.ToString(csv[3]),
                        
                        TypeDescription = Convert.ToString(csv[4])
                    };
                    await _rpEc.InsertEcEmployeeType(obj);
                }
            }
            return true;
        }
        private async Task<bool> ReadEcBundle(string fileName)
        {
            //List<EcLocation> result = new List<EcLocation>();
            using (CsvReader csv = new CsvReader(new StreamReader(fileName), true))
            {
                int fieldCount = csv.FieldCount;

                string[] headers = csv.GetFieldHeaders();
                EcBundle obj = null;
                while (csv.ReadNextRecord())
                {
                    obj = new EcBundle
                    {
                        Id = Convert.ToInt32(csv[0]),
                        DocType = Convert.ToString(csv[1]),
                        Description_Vi = csv[2].ToString(),
                        Description_En = Convert.ToString(csv[3]),
                        RefBundleCode = csv[4].ToString(),
                        RefCodeId = Convert.ToString(csv[5]),
                        BundleName = Convert.ToString(csv[6])
                    };
                    await _rpEc.InsertEcBundle(obj);
                }
            }
            return true;
        }
        private async Task<bool> ReadEcBank(string fileName)
        {
            //List<EcLocation> result = new List<EcLocation>();
            using (CsvReader csv = new CsvReader(new StreamReader(fileName), true))
            {
                int fieldCount = csv.FieldCount;

                string[] headers = csv.GetFieldHeaders();
                EcBank obj = null;
                while (csv.ReadNextRecord())
                {
                    obj = new EcBank
                    {
                        Id = Convert.ToInt32(csv[0]),
                        RefIndividual = Convert.ToString(csv[1]),
                        BankName = csv[2].ToString(),
                        BranchCode = Convert.ToString(csv[3]),
                        BankProvince = csv[4].ToString(),
                        BranchName = Convert.ToString(csv[5]),
                        BankCode = Convert.ToInt32(csv[6])
                    };
                    await _rpEc.InsertEcBank(obj);
                }
            }
            return true;
        }
        private async Task<bool> ReadEcLocation(string fileName)
        {
            //List<EcLocation> result = new List<EcLocation>();
            using (CsvReader csv = new CsvReader(new StreamReader(fileName), true))
            {
                int fieldCount = csv.FieldCount;

                string[] headers = csv.GetFieldHeaders();
                EcLocation obj = null;
                while (csv.ReadNextRecord())
                {
                    obj = new EcLocation
                    {
                        Id = Convert.ToInt32(csv[0]),
                        WardCode = Convert.ToInt32(csv[1]),
                        WardName = csv[2].ToString(),
                        DistrictCode = Convert.ToInt32(csv[3]),
                        DistrictName = csv[4].ToString(),
                        ProvinceCode = Convert.ToInt32(csv[5]),
                        ProvinceName = csv[6].ToString()
                    };
                    if(obj.ProvinceCode == 2)
                    {
                        obj.OrderValue = 79;
                    }
                    else
                    if (obj.ProvinceCode == 79)
                    {
                        obj.OrderValue = 2;
                    }
                    else
                    {
                        obj.OrderValue = obj.ProvinceCode;
                    }
                    await _rpEc.InsertEcLocation(obj);
                }
            }
            return true;
        }

    }
}
