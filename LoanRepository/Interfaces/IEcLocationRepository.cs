using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity;

namespace LoanRepository.Interfaces
{
    public interface IEcLocationRepository
    {
        Task<List<StringOptionSimple>> GetIssuePlace();
        Task<List<OptionSimple>> GetProvinces();
        Task<List<OptionSimple>> GetDistricts(int provinceId);
        Task<List<OptionSimple>> GetWards(int districtId);
    }
}
