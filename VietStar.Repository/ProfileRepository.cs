using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using VietStar.Entities;
using VietStar.Entities.Profile;
using VietStar.Entities.ViewModels;
using VietStar.Repository.Interfaces;
using VietStar.Utility;

namespace VietStar.Repository
{
    public class ProfileRepository : RepositoryBase, IProfileRepository
    {
        public ProfileRepository(IConfiguration configuration) : base(configuration)
        {
        }
        public async Task<List<ProfileIndexModel>> GetsAsync(int userId
            , DateTime fromDate
            , DateTime toDate
            ,int dateType = 1 
            , int groupId = 0
            , int memberId =0
            ,string status = null
            , string freeText = null
            ,string sort = "desc"
            ,string sortField ="updatedtime"
            , int page = 1
            , int limit = 20)
        
        {
            ProcessInputPaging(page, ref limit, out offset);
            try
            {
                using (var con = GetConnection())
                {
                    var result = await con.QueryAsync<ProfileIndexModel>("sp_Profile_Search", new
                    {
                        userId,
                        groupId,
                        memberId,
                        fromDate,
                        toDate,
                        dateType,
                        status,
                        freeText,
                        sort,
                        sortField,
                        offset,
                        limit
                    }, commandType: CommandType.StoredProcedure);
                    return result.ToList();
                }
            }
            catch(Exception e)
            {
                return null;
            }
           
        }
        public async Task<RepoResponse<int>> CreateAsync(ProfileAddSql model, int createdBy)
        {
            var par = GetParams(model, new string[] {
                nameof(model.UpdatedBy),
                nameof(model.UpdatedTime),
                nameof(model.CreatedTime),
                nameof(model.Ho_So_Cua_Ai),
                nameof(model.CreatedBy),
                nameof(model.Ma_Ho_So)
          
            }, "id");
            par.Add("CreatedBy", createdBy);
            par.Add("Ho_So_Cua_Ai", createdBy);
            try
            {
                using (var _con = GetConnection())
                {
                    await _con.ExecuteAsync("sp_HO_SO_Them_v2", par, commandType: CommandType.StoredProcedure);
                    var id = par.Get<int>(nameof(model.ID));
                    return RepoResponse<int>.Create(id);
                }
            }
            catch(Exception e)
            {
                return RepoResponse<int>.Create(0, GetException(e));
            }

        }
        public async Task<RepoResponse<ProfileDetail>> GetByIdAsync(int profileId)
        {
            try
            {
                using (var con = GetConnection())
                {
                    var result = await con.QueryFirstOrDefaultAsync<ProfileDetail>("sp_HO_SO_LayChiTiet_v2", new { profileId }, commandType: CommandType.StoredProcedure);
                    return RepoResponse<ProfileDetail>.Create(result);
                }
            }
            catch(Exception e)
            {
                return RepoResponse<ProfileDetail>.Create(null, GetException(e));
            }
        }
    }
}
