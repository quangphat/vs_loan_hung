using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using VietStar.Entities.Collection;
using VietStar.Entities.ViewModels;
using VietStar.Repository.Interfaces;


namespace VietStar.Repository
{
    public class RevokeDebtRepository : RepositoryBase, IRevokeDebtRepository
    {
        protected readonly ILogRepository _rpLog;
        public RevokeDebtRepository(IConfiguration configuration, ILogRepository logRepository) : base(configuration)
        {
            _rpLog = logRepository;
        }

        public async Task<List<RevokeDebtSearch>> SearchAsync(int userId, 
            string freeText, 
            string status, 
            int page, 
            int limit, 
            int groupId = 0, 
            int assigneeId = 0,
            DateTime? fromDate = null, 
            DateTime? toDate = null, 
            int dateType = 1, 
            int processStatus = -1)
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<RevokeDebtSearch>("sp_RevokeDebt_Search",
                    new { freeText, page, limit_tmp = limit, status, groupId, userId, assigneeId, fromDate, toDate, dateType, processStatus }
                    , commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }
    }
}

