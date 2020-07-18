using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Entity.Model;

namespace VS_LOAN.Core.Repository.Interfaces
{
    public interface IPartnerRepository
    {
        Task<List<DoiTacModel>> LayDS();
        Task<int> LayMaDoiTac(int maSanPham);
        Task<List<OptionSimple>> GetListForCheckCustomerDuplicateAsync();
    }
}
