using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity;

namespace LoanRepository.Interfaces
{
    public interface IEcProductRepository
    {
        Task<List<OptionEcProductType>> GetSimples(string employmentType);
    }
}

