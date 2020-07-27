﻿using Dapper;
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
using VS_LOAN.Core.Entity.RevokeDebt;
using VS_LOAN.Core.Repository.Interfaces;

namespace VS_LOAN.Core.Business
{
    public class RevokeDebtBusiness : IRevokeDebtBusiness
    {
        protected readonly IRevokeDebtRepository _rpRevokeDebt;
        protected readonly ITailieuRepository _rpTailieu;
        protected readonly ISystemconfigRepository _rpConfig;
        public RevokeDebtBusiness(IRevokeDebtRepository revokeDebtRepository, ITailieuRepository tailieuRepository, ISystemconfigRepository systemconfigRepository)
        {
            _rpTailieu = tailieuRepository;
            _rpRevokeDebt = revokeDebtRepository;
            _rpConfig = systemconfigRepository;
        }
        public async Task<DataPaging<List<RevokeDebtSearch>>> Search(int userId, string freeText, string status, int page, int limit, int groupId = 0)
        {
            var data = await _rpRevokeDebt.Search(userId, freeText, status, page, limit, groupId);
            if (data == null || !data.Any())
            {
                return DataPaging.Create(null as List<RevokeDebtSearch>, 0);
            }
            var result = DataPaging.Create(data, data[0].TotalRecord);
            return result;
        }
        public async Task<BaseResponse<bool>> InsertFromFile(MemoryStream stream, int userId)
        {
            var result = await ReadXlsxFile(stream);
            if (result.IsSuccess == false || result.Data == null)
                return new BaseResponse<bool>(result.Message, false, false);
            if (result.Data == null || !result.Data.Any())
                return new BaseResponse<bool>("Không có dữ liệu hoặc không thể import", false, false);
            foreach (var param in result.Data)
            {
                await _rpRevokeDebt.InsertManyByParameter(param, userId);
            }
            return new BaseResponse<bool>($"Đã import thành công {result.Data.Count} dòng", true, true);
        }
       
        protected async Task<BaseResponse<List<DynamicParameters>>> ReadXlsxFile(MemoryStream stream)
        {

            var importExelFrameWork = await _rpTailieu.GetImportTypes((int)HosoType.RevokeDebt);
            if (importExelFrameWork == null)
                return new BaseResponse<List<DynamicParameters>>("Không tìm thấy importExelFrameWork", null,  false);
            var config = await _rpConfig.GetByCode(Constanst.revoke_debt_max_row_import);
            //return null;
            var result = new TupleModel();
            var workBook = WorkbookFactory.Create(stream);
            var sheet = workBook.GetSheetAt(0);
            var rows = sheet.GetRowEnumerator();
            var hasData = rows.MoveNext();
            var param = new DynamicParameters();
            var pars = new List<DynamicParameters>();
            int skipCell = 0;
            if(sheet.PhysicalNumberOfRows - 2 > config.Value)
            {
                return new BaseResponse<List<DynamicParameters>>($"Số dòng của file không được nhiều hơn {config.Value}", null, false);
            }
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
                                if (row.GetCell(col.Position) == null)
                                {
                                    param.Add(col.Name, string.Empty);
                                    skipCell += 1;
                                }
                                else
                                {
                                    param.Add(col.Name, BusinessExtentions.TryGetValueFromCell(row.Cells[col.Position - skipCell].ToString(), col.ValueType));
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
            return new BaseResponse<List<DynamicParameters>>(string.Empty, pars, true);
        }
    }
}
