using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VietStar.Entities.ViewModels;

namespace VietStar.Repository.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<bool> GetStatus(int userId);
        Task<Account> Login(string userName, string password);
        Task<List<string>> GetPermissions(int userId);
    }
}
