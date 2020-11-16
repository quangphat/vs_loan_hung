using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Repository.Interfaces;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Entity.MCreditModels;
using VS_LOAN.Core.Entity.MCreditModels.SqlModel;

namespace VS_LOAN.Core.Repository
{
    public class OcbRepository : BaseRepository, IOcbRepository
    {
        protected readonly INoteRepository _rpNote;
        public OcbRepository(INoteRepository rpNote) : base(typeof(OcbRepository))
        {
            _rpNote = rpNote;

        }

        public async Task<int> CreateDraftProfile(OcbProfile model)
        {
            try
            {
                model.Id = 0;
                var param = GetParams(model, "Id", ignoreKey: new string[] {
                nameof(model.CreatedTime),
                nameof(model.UpdatedTime),
                   nameof(model.CustomerId),
                      nameof(model.IsPushDocument)

            });

                using (var con = GetConnection())
                {
                    await con.ExecuteAsync("sp_insert_Ocb_Item", param, commandType: CommandType.StoredProcedure);
                    return param.Get<int>("Id");
                }
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public async Task<List<OcbProfileStatus>> GetAllStatus()
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<OcbProfileStatus>("sp_ocb_profileStatus", commandType: CommandType.StoredProcedure);
                return result.ToList();
            }

        }

        public async Task<List<GhichuViewModel>> GetCommentsAsync(int profileId)
        {
            return await _rpNote.GetNoteByTypeAsync(profileId, (int)HosoType.RevokeDebt);
        }

        public async Task<List<OcbProductLoan>> GetLoanProduct( int MaDoiTac)
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<OcbProductLoan>("sp_SAN_PHAM_VAYOcb_LayDSByID", new { MaDoiTac }, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }

        public async Task<BaseResponse<bool>> AddNoteAsync(int profileId, string content, int userId)
        {
            if (string.IsNullOrWhiteSpace(content))
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
        public Task<List<int>> GetPeopleCanViewMyProfile(int profileId)
        {
            throw new NotImplementedException();
        }

      

        public async Task<List<OcbSerarchSql>> GetTempProfiles(int page, int limit, string freeText, int userId,
         string status = null,
         DateTime? fromDate = null,
         DateTime? toDate = null,
         int dateType = 0,
         int maNhom = 0,
         int maThanhVien = 0)
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<OcbSerarchSql>("sp_ocb_Gets", new
                {
                    freeText,
                    userId,
                    page,
                    limit_tmp = limit,
                    status,
                    fromDate,
                    toDate,
                    dateType,
                    maNhom,
                    maThanhVien
                }, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }

        public async Task<OcbProfile> GetTemProfileByMcId(int id)
        {

            using (var con = GetConnection())
            {
                var result = await con.QueryFirstOrDefaultAsync<OcbProfile>("sp_ocb_GetById", new { id }, commandType: CommandType.StoredProcedure);
                return result;
            }

        }

        public Task<bool> InsertPeopleWhoCanViewProfile(int profileisUpdateMCIId, string peopleIds)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateDraftProfile(OcbProfile model)
        {
            var param = GetParams(model, ignoreKey: new string[]
            {
                nameof(model.CreatedTime),
                nameof(model.UpdatedTime),
                nameof(model.CreatedBy),
                nameof(model.CustomerId),
                nameof(model.IsPushDocument)
               
            });

            using (var con = GetConnection())
            {
                var storeExecute = "sp_update_ocb";
                await con.ExecuteAsync(storeExecute, param, commandType: CommandType.StoredProcedure);
                return true;
            }
        }

        public async Task<bool> UpdateOCBProileReport(OcbStatusImportModel model)
        {
            var param = GetParams(model, ignoreKey: new string[]
            {

            });
            using (var con = GetConnection())
            {
                var storeExecute = "sp_updateOCBStatus";
                await con.ExecuteAsync(storeExecute, param, commandType: CommandType.StoredProcedure);
                return true;
            }
        }

        public async Task<bool> UpdateStatusComplete(int customerId, int id)
        {
            using (var con = GetConnection())
            {
                var storeExecute = "sp_update_customerId";
                await con.ExecuteAsync(storeExecute, new { id,customerId }, commandType: CommandType.StoredProcedure);
                return true;
            }
        }
    }
}
