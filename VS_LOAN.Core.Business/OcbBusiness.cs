using Dapper;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Business.Infrastuctures;
using VS_LOAN.Core.Business.Interfaces;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Entity.MCreditModels;
using VS_LOAN.Core.Entity.RevokeDebt;
using VS_LOAN.Core.Repository.Interfaces;

namespace VS_LOAN.Core.Business
{
    public class OcbBusiness : IOcbBusiness
    {
        protected readonly IOcbRepository _rpRevokeDebt;
        public OcbBusiness(IOcbRepository revokeDebtRepository)
        {
            _rpRevokeDebt = revokeDebtRepository;
        }
        public async Task<bool> HandleFileImport(Stream stream, int userId)
        {
          
            using (var fileStream = new MemoryStream())
            {
                await stream.CopyToAsync(fileStream);
                var result = new TupleModel();
                var workBook = WorkbookFactory.Create(stream);
                var sheet = workBook.GetSheetAt(0);
                var rows = sheet.GetRowEnumerator();
                var hasData = rows.MoveNext();
                int skipCell = 0;
                for (int i = 1; i < sheet.PhysicalNumberOfRows; i++)
                {
                    var row = sheet.GetRow(i);

                    if (row == null)
                        continue;
                    var item = new OcbStatusImportModel();
                    item.ImportDate = row.Cells[0].DateCellValue;
                    item.MonthImport = row.Cells[1].StringCellValue;
                    item.CustomerId = Convert.ToInt32(row.Cells[2].NumericCellValue);
                    try
                    {
                        item.FirstCallDate = row.Cells[6].DateCellValue;
                    }
                    catch (Exception)
                    {

                    }
                    try
                    {
                        item.LastCallDate = row.Cells[8].DateCellValue;
                    }
                    catch (Exception)
                    {
                    }
                    try
                    {
                        item.DisbureseDate = row.Cells[13].DateCellValue;
                    }
                    catch (Exception)
                    {

                       
                    }
                    item.FirstCallStatus = row.Cells[7].StringCellValue;
                    item.LastCallStatus = row.Cells[9].StringCellValue;
                    item.AppStatusForSale = row.Cells[11].StringCellValue;
                    item.AppProcessStatus = row.Cells[12].StringCellValue;
                    item.DisbureseMonth = row.Cells[14].StringCellValue;
                    item.RejectCode = row.Cells[17].StringCellValue;
                    item.CancelCode = row.Cells[16].StringCellValue;
                    try
                    {
                        item.lastCallNote = row.Cells[10].StringCellValue;
                    }
                    catch (Exception)
                    {
                        try
                        {
                            item.lastCallNote = row.Cells[10].NumericCellValue.ToString();
                        }
                        catch (Exception)
                        {

                            item.lastCallNote = "";
                        }
                        
                    }
                    
                    try
                    {
                        item.Volumn = Convert.ToInt32(row.Cells[15].NumericCellValue);


                    }
                    catch (Exception)
                    {

                        item.Volumn = 0;
                    }


                    try
                    {
                        item.AppCreate = Convert.ToInt32(row.Cells[18].NumericCellValue);
                    }
                    catch (Exception)
                    {

                      
                    }

                    try
                    {
                        item.AppLoan = Convert.ToInt32(row.Cells[19].NumericCellValue);
                    }
                    catch (Exception)
                    {
                    }

                    try
                    {
                        item.AppAprove = Convert.ToInt32(row.Cells[20].NumericCellValue);
                    }
                    catch (Exception)
                    {
                    }

                    try
                    {
                        item.AppCancel = Convert.ToInt32(row.Cells[21].NumericCellValue);
                    }
                    catch (Exception)
                    {


                    }

                    try
                    {
                        item.AppReject = Convert.ToInt32(row.Cells[22].NumericCellValue);
                    }
                    catch (Exception)
                    {


                    }


                    item.UpdatedBy = userId;
                    
               
                
                    await _rpRevokeDebt.UpdateOCBProileReport(item);
                }
            }
            return true;
        }

       
    }
}
