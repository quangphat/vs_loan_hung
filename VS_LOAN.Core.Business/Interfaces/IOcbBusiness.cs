using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Business.Infrastuctures;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Entity.RevokeDebt;

namespace VS_LOAN.Core.Business.Interfaces
{
    public interface IOcbBusiness
    {
        Task<bool> HandleFileImport(Stream stream, int userId);
    }
}
