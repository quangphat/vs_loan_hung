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
using VS_LOAN.Core.Entity.RevokeDebt;
using VS_LOAN.Core.Repository.Interfaces;

namespace VS_LOAN.Core.Business
{
    public class RevokeDebtBusiness : IRevokeDebtBusiness
    {
        protected readonly IRevokeDebtRepository _rpRevokeDebt;
        protected readonly ITailieuRepository _rpTailieu;
        protected readonly ISystemconfigRepository _rpConfig;
        protected readonly INoteRepository _rpNote;
        public RevokeDebtBusiness(IRevokeDebtRepository revokeDebtRepository
            , ITailieuRepository tailieuRepository
            , ISystemconfigRepository systemconfigRepository
            ,INoteRepository noteRepository)
        {
            _rpTailieu = tailieuRepository;
            _rpRevokeDebt = revokeDebtRepository;
            _rpConfig = systemconfigRepository;
            _rpNote = noteRepository;
        }
        public async Task<DataPaging<List<RevokeDebtSearch>>> SearchAsync(int userId, string freeText, string status, int page, int limit, int groupId = 0, int assigneeId = 0, DateTime? fromDate = null, DateTime? toDate = null, int loaiNgay = 1)
        {
            var data = await _rpRevokeDebt.SearchAsync(userId, freeText, status, page, limit, groupId,assigneeId,fromDate,toDate,loaiNgay);
            if (data == null || !data.Any())
            {
                return DataPaging.Create(null as List<RevokeDebtSearch>, 0);
            }
            var result = DataPaging.Create(data, data[0].TotalRecord);
            return result;
        }
        public async Task<BaseResponse<bool>> InsertFromFileAsync(MemoryStream stream, int userId)
        {
            var result = await ReadXlsxFile(stream);
            if (result.IsSuccess == false || result.Data == null)
                return new BaseResponse<bool>(result.Message, false, false);
            if (result.Data == null || !result.Data.Any())
                return new BaseResponse<bool>("Không có dữ liệu hoặc không thể import", false, false);
            foreach (var param in result.Data)
            {
                await _rpRevokeDebt.InsertManyByParameterAsync(param, userId);
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
                            bool isNullRow = row.Cells.Count < 20 ? true : false;
                            if (isNullRow)
                                continue;
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

        public async Task<RevokeDebtSearch> GetByIdAsync(int profileId, int userId)
        {
            var result = await _rpRevokeDebt.GetByIdAsync(profileId, userId);
            return result;
        }

        public async Task<bool> DeleteByIdAsync(int userId, int profileId)
        {
            return await _rpRevokeDebt.DeleteByIdAsync(userId, profileId);
        }
        public async Task<bool> UpdateStatusAsync(int userId, int profileId, int status)
        {
            return await _rpRevokeDebt.UpdateStatusAsync(userId, profileId, status);
        }
        public async Task<List<GhichuViewModel>> GetCommentsAsync(int profileId)
        {
            return await _rpNote.GetNoteByTypeAsync(profileId, (int)HosoType.RevokeDebt);
        }
        public async Task<BaseResponse<bool>> AddNoteAsync(int profileId , string content ,int userId)
        {
            if(string.IsNullOrWhiteSpace(content))
            {
                return new BaseResponse<bool>("Dữ liệu không hợp lệ", false, false);
            }
            await _rpNote.AddNoteAsync(new Entity.Model.GhichuModel
            {
                CommentTime = DateTime.Now,
                HosoId = profileId,
                Noidung = content,
                TypeId = (int)HosoType.RevokeDebt,
                UserId = userId
            });
            return new BaseResponse<bool>(true);
        }

        public async Task<bool> UpdateSimpleAsync(RevokeSimpleUpdate model, int updateBy, int profileId)
        {
            return await _rpRevokeDebt.UpdateSimpleAsync(model, updateBy, profileId);
        }
    }
}
