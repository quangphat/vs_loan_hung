using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using VietStar.Entities;
using VietStar.Entities.CheckDup;
using VietStar.Entities.ViewModels;
using VietStar.Repository.Interfaces;


namespace VietStar.Repository
{
    public class CheckDupRepository : RepositoryBase, ICheckDupRepository
    {
        protected readonly ILogRepository _rpLog;
        public CheckDupRepository(IConfiguration configuration, ILogRepository logRepository) : base(configuration)
        {
            _rpLog = logRepository;
        }
        public async Task<RepoResponse<int>> CreateAsync(CheckDupAddSql model, int createdBy)
        {
            var par = GetParams(model, new string[] {
                nameof(model.UpdatedBy),
                nameof(model.UpdatedTime),
                nameof(model.CreatedTime),
                nameof(model.CreatedBy)

            }, "Id");
            par.Add("CreatedBy", createdBy);
            try
            {
                using (var _con = GetConnection())
                {
                    await _con.ExecuteAsync("sp_HO_SO_Them_v2", par, commandType: CommandType.StoredProcedure);
                    var id = par.Get<int>(nameof(model.Id));
                    return RepoResponse<int>.Create(id);
                }
            }
            catch(Exception e)
            {
                return RepoResponse<int>.Create(0, GetException(e));
            }
        }
    }
}

