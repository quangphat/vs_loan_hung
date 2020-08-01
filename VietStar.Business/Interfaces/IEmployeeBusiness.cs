using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VietStar.Entities.Commons;
using VietStar.Entities.ViewModels;

namespace VietStar.Business.Interfaces
{
    public interface IEmployeeBusiness
    {
        Task<bool> GetStatus(int userId);
        Task<Account> Login(LoginModel model);
        Task<List<OptionSimple>> GetMemberByGroupId(int groupId);
    }
}
