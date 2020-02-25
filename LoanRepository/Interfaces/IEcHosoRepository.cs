using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Entity.EasyCredit;

namespace LoanRepository.Interfaces
{
    public interface IEcHosoRepository
    {
        Task<int> Insert(EcHoso model);
        Task<bool> InsertHosoRequest(int hosoId, string requestId);
    }
}

