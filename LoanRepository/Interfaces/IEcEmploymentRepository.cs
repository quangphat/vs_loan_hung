using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity;

namespace LoanRepository.Interfaces
{
    public interface IEcEmploymentRepository
    {
        Task<List<StringOptionSimple>> GetEmployment(string type);
    }
}

