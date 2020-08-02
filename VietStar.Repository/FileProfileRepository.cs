using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using VietStar.Entities.FileProfile;
using VietStar.Entities.ViewModels;
using VietStar.Repository.Interfaces;


namespace VietStar.Repository
{
    public class FileProfileRepository : RepositoryBase, IFileProfileRepository
    {
        protected readonly ILogRepository _rpLog;
        public FileProfileRepository(IConfiguration configuration, ILogRepository logRepository) : base(configuration)
        {
            _rpLog = logRepository;
        }
        public async Task<List<FileProfileType>> GetByType(int profileType)
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<FileProfileType>("sp_LOAI_TAI_LIEU_GetsByType", new {profileType }, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }
    }
}

