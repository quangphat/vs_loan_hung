using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Repository.Interfaces;
using VS_LOAN.Core.Entity.Model;
using VS_LOAN.Core.Nhibernate;
using VS_LOAN.Core.Utility.Exceptions;
using VS_LOAN.Core.Entity;

namespace VS_LOAN.Core.Repository
{
    public class PartnerRepository : BaseRepository, IPartnerRepository
    {
        public PartnerRepository() : base(typeof(PartnerRepository))
        {
        }

        public async Task<List<DoiTacModel>> LayDS()
        {
            try
            {
                using (var con = GetConnection())
                {
                    var result = await con.QueryAsync<DoiTacModel>("sp_DOI_TAC_LayDS", commandType: CommandType.StoredProcedure);
                    return result.ToList();
                }
            }
            catch (BusinessException ex)
            {
                return null;
            }
        }
        public async Task<List<OptionSimple>> GetListForCheckCustomerDuplicateAsync()
        {
            using (var con = GetConnection())
            {
                var result = await _connection.QueryAsync<OptionSimple>("sp_getListPartnerForCustomerCheck", commandType: CommandType.StoredProcedure);
                return result.ToList();
            }

        }
        public async Task<int> LayMaDoiTac(int maSanPham)
        {
            try
            {
                using (var con = GetConnection())
                {
                    var result = await con.ExecuteScalarAsync<int>("sp_DOI_TAC_LayIDByMaSanPham", new { MaSanPham = maSanPham }, commandType: CommandType.StoredProcedure);

                    return result;
                }
            }
            catch (BusinessException ex)
            {
                return 0;
            }
        }
    }
}
