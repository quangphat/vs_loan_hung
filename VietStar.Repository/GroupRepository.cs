using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using VietStar.Entities.GroupModels;
using VietStar.Entities.ViewModels;
using VietStar.Repository.Interfaces;


namespace VietStar.Repository
{
    public class GroupRepository : RepositoryBase, IGroupRepository
    {
        protected readonly ILogRepository _rpLog;
        public GroupRepository(IConfiguration configuration, ILogRepository logRepository) : base(configuration)
        {
            _rpLog = logRepository;
        }

        public async Task<List<GroupModel>> GetChildGroupByParentId(int parentGroupId)
        {
            try
            {
                using (var _con = GetConnection())
                {
                    var result = await _con.QueryAsync<GroupModel>("sp_NHOM_LayCayNhomCon_v3", new { parentGroupId }, commandType: CommandType.StoredProcedure);
                    return result.ToList();
                }
            }
            catch (Exception e)
            {
                await _rpLog.InsertLogFromException(nameof(GetChildGroupByParentId), e);
                return null;
            }
        }

        public async Task<List<GroupModel>> GetGroupByUserId(int userId)
        {
            try
            {
                using (var _con = GetConnection())
                {
                    var result = await _con.QueryAsync<GroupModel>("sp_NHOM_LayDSChonTheoNhanVien_v3", new { userId }, commandType: CommandType.StoredProcedure);
                    return result.ToList();
                }
            }
            catch(Exception e)
            {
                await _rpLog.InsertLogFromException(nameof(GetGroupByUserId), e);
                return null;
            }
        }
    }
}

