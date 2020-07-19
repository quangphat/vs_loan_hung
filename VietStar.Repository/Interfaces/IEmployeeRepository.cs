using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VietStar.Entities.ViewModels;

namespace VietStar.Repository.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<Account> Login(string userName, string password);
    }
}
