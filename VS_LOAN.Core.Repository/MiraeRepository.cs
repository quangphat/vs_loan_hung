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
    public class MiraeRepository : BaseRepository, IMiraeRepository
    {
        protected readonly INoteRepository _rpNote;
        public MiraeRepository(INoteRepository rpNote) : base(typeof(OcbRepository))
        {
            _rpNote = rpNote;

        }
        
        public async Task<int> CreateS37Profile(S37profileModel model)
        {
            try
            {
                model.Id = 0;
                var param = GetParams(model, "Id", ignoreKey: new string[] {
                nameof(model.CreatedTime),
                nameof(model.IsDeleted),
                 nameof(model.Id)

                 });

                using (var con = GetConnection())
                {

                        var result =  await con.ExecuteAsync("sp_s37profile_Them", param, commandType: CommandType.StoredProcedure);
                        return result;
                }
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public async Task<List<S37profileSearchModel>> GetS37Profiles(int page, int limit, int userId       )
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<S37profileSearchModel>("sp_s37profile_Gets", new
                {
                    userId,
                    page,
                    limit
                }, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }

        }

        public async Task<int> CreateDraftProfile(MiraeModel model)
        {
            try
            {
                model.Id = 0;
                var param = GetParams(model, "Id", ignoreKey: new string[] {
                nameof(model.CreatedTime),
                nameof(model.UpdatedTime),
                 nameof(model.UpdatedBy),
                 nameof(model.AppId),
           

                nameof(model.Status )
             


            });

                using (var con = GetConnection())
                {
                  
                      await con.ExecuteAsync("sp_insert_Mirae_Item", param, commandType: CommandType.StoredProcedure);
                    return 1;
                }
            }
            catch (Exception e)
            {
                return 0;
            }
        }


       
    
        public async Task<List<GhichuViewModel>> GetCommentsAsync(int profileId)
        {
            return await _rpNote.GetNoteByTypeAsync(profileId, (int)HosoType.Mirae);
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
                TypeId = (int)HosoType.Mirae,
                UserId = userId
            });
            return new BaseResponse<bool>(true);
        }
        public Task<List<int>> GetPeopleCanViewMyProfile(int profileId)
        {
            throw new NotImplementedException();
        }

      

        public async Task<List<MiraeModelSearchModel>> GetTempProfiles(int page, int limit, string freeText, int userId,
         string status = null,
         DateTime? fromDate = null,
         DateTime? toDate = null,
         int dateType = 0,
         int maNhom = 0,
         int maThanhVien = 0)
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<MiraeModelSearchModel>("sp_Mirae_Gets", new
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




        public async Task<MiraeModel> GetByAppid(int appID)
        {

            using (var con = GetConnection())
            {
                var result = await con.QueryFirstOrDefaultAsync<MiraeModel>("sp_mirae_GetByAppId", new { appID }, commandType: CommandType.StoredProcedure);
                return result;
            }

        }

       
        public async Task<MiraeModel> GetTemProfileByMcId(int id)
        {

            using (var con = GetConnection())
            {
                var result = await con.QueryFirstOrDefaultAsync<MiraeModel>("sp_mirae_GetById", new { id }, commandType: CommandType.StoredProcedure);
                return result;
            }

        }
      
        public async Task<MiraeDetailModel> GetDetail(int id)
        {

            using (var con = GetConnection())
            {
                var result = await con.QueryFirstOrDefaultAsync<MiraeDetailModel>("sp_mirae_GetById", new { id }, commandType: CommandType.StoredProcedure);
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

 
      


        public async Task<bool> UpdatStatusClient(ClientUpdateStatusRequest request)
         {
            var param = GetParams(request, ignoreKey: new string[]
           {

           });
            using (var con = GetConnection())
            {
                var storeExecute = "sp_MiraeUpdateClientStatus";
                await con.ExecuteAsync(storeExecute,param, commandType: CommandType.StoredProcedure);
                return true;
            }
        }

        public async Task<bool> UpdateDraftProfile(MiraeModel model)
        {
            try
            {
                var param = GetParams(model, ignoreKey: new string[]
           {
                nameof(model.CreatedTime),
                nameof(model.UpdatedTime),
          
                nameof(model.AppId),
                nameof(model.CreatedBy),
           
                //nameof(model.Maritalstatus ),
                //nameof(model.Qualifyingyear ),
                //nameof(model.Noofdependentin  ),
                //nameof(model.Paymentchannel ),
                //nameof(model.Nationalidissuedate  ),
                //nameof(model.Familybooknumber ),
                //nameof(model.Idissuer ),
                //nameof(model.Spousename ),
                //nameof(model.Spouse_id_c ),
                //nameof(model.Categoryid ),
                //nameof(model.Bankname ),nameof(model.Bankbranch ),
                //nameof(model.Acctype ),
                //nameof(model.Accno ),
                //nameof(model.Dueday ),
                //nameof(model.Notecode ),
                //nameof(model.Eduqualify ),


                //nameof(model.Notedetails )

           }); 
                model.Status = 0;
                using (var con = GetConnection())
                {
                    var storeExecute = "sp_update_Mirae_Item";
                    await con.ExecuteAsync(storeExecute, param, commandType: CommandType.StoredProcedure);
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
            
           
        }

        public async Task<List<OcbProfileStatus>> GetAllStatus()
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<OcbProfileStatus>("sp_Mirae_profileStatus", commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }

        public Task<List<OcbProductLoan>> GetLoanProduct(int MaDoiTac)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateStatus(int id, int status, int appid, int userid)
        {
            using (var con = GetConnection())
            {
                var storeExecute = "sp_updateMiraeStatus";
                await con.ExecuteAsync(storeExecute, new { id, status,appid,userid }, commandType: CommandType.StoredProcedure);
                return true;
            }
        }

        public async Task<bool> UpdateStatusMAFC(int id, int status, int appid, int userid)
        {
            using (var con = GetConnection())
            {
                var storeExecute = "sp_updateMiraeStatusMafc";
                await con.ExecuteAsync(storeExecute, new { id, status, appid, userid }, commandType: CommandType.StoredProcedure);
                return true;
            }
        }

        public async Task<bool> SetAppidProfile(int id,int appId)
        {
            using (var con = GetConnection())
            {
                var storeExecute = "setAppidProfile";
                await con.ExecuteAsync(storeExecute, new { id, appId }, commandType: CommandType.StoredProcedure);
                return true;
            }
        }

        public async Task<bool> UpdateDDE(MiraeDDEEditModel model)
        {
            

            var param = GetParams(model, ignoreKey: new string[]
           {
                nameof(model.CreatedTime),
                nameof(model.UpdatedTime),
                nameof(model.CreatedBy)
              

           });

            using (var con = GetConnection())
            {
                var storeExecute = "sp_update_MiraeDDE_Item";
                await con.ExecuteAsync(storeExecute, param, commandType: CommandType.StoredProcedure);
                return true;
            }
        }

        public async Task<bool> UpdateS37(int id, string requestedId)
        {

            using (var con = GetConnection())
            {
                var storeExecute = "sp_updateS37Requested";
                await con.ExecuteAsync(storeExecute, new { id, requestedId }, commandType: CommandType.StoredProcedure);
                return true;
            }
        }
    }
}
