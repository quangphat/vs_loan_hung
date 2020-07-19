using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VietStar.Entities.ViewModels;

namespace VietStar.Business.Interfaces
{
    public interface IEmployeeBusiness
    {
        Task<Account> Login(LoginModel model);
    }
}
