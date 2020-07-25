using Dapper;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Business.Infrastuctures;
using VS_LOAN.Core.Business.Interfaces;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Repository.Interfaces;

namespace VS_LOAN.Core.Business
{
    public class RevokeDebtBusiness : IRevokeDebtBusiness
    {
        protected readonly IRevokeDebtRepository _rpRevokeDebt;
        protected readonly ITailieuRepository _rpTailieu;
        public RevokeDebtBusiness(IRevokeDebtRepository revokeDebtRepository, ITailieuRepository tailieuRepository)
        {
            _rpTailieu = tailieuRepository;
            _rpRevokeDebt = revokeDebtRepository;
        }
        public async Task<BaseResponse<bool>> InsertFromFile(MemoryStream stream, int userId)
        {
            var pars = await ReadXlsxFile(stream);
            if (pars == null || !pars.Any())
                return new BaseResponse<bool>("Không có dữ liệu hoặc không thể import", false);
            var tasks = new List<Task>();
            foreach (var param in pars)
            {
                tasks.Add(_rpRevokeDebt.InsertManyByParameter(param, userId));
            }
            await Task.WhenAll(tasks);
            return new BaseResponse<bool>(string.Empty, true);
        }
        protected async Task<List<DynamicParameters>> ReadXlsxFile(MemoryStream stream)
        {
            var importExelFrameWork = await _rpTailieu.GetImportTypes((int)HosoType.RevokeDebt);
            //return null;
            var result = new TupleModel();
            var workBook = WorkbookFactory.Create(stream);
            var sheet = workBook.GetSheetAt(0);
            var rows = sheet.GetRowEnumerator();
            var hasData = rows.MoveNext();
            var param = new DynamicParameters();
            var pars = new List<DynamicParameters>();
            int skipCell = 0;
            for (int i = 2; i < sheet.PhysicalNumberOfRows; i++)
            {
                try
                {
                    param = new DynamicParameters();
                    var row = sheet.GetRow(i);
                    if (row != null)
                    {
                        if (row.Cells.Count > 1)
                        {
                            bool isNullRow = row.Cells.Count < 3 ? true : false;
                        }

                        foreach (var col in importExelFrameWork)
                        {
                            try
                            {
                                if(row.GetCell(col.Position) == null)
                                {
                                    param.Add(col.Name, string.Empty);
                                    skipCell += 1;
                                }
                                else
                                {
                                    param.Add(col.Name, BusinessExtentions.TryGetValueFromCell(row.Cells[col.Position-skipCell].ToString(), col.ValueType));
                                }
                               
                            }
                            catch (Exception e)
                            {
                                param = null;
                            }

                        }
                        if (param != null)
                        {
                            skipCell = 0;
                            pars.Add(param);
                           
                        }
                            
                    }
                }
                catch
                {

                }

            }
            return pars;
        }
    }
}
