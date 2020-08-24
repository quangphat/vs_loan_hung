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
        Task<List<OptionSimple>> GetByProvinceIdAsync(int provinceId);
        Task<List<OptionSimple>> GetSalesAsync();
        Task<bool> GetStatusAsync(int userId);
        Task<Account> LoginAsync(LoginModel model);
        Task<List<OptionSimple>> GetMemberByGroupIdAsync(int groupId);
        Task<List<OptionSimple>> GetAllEmployeePagingAsync(int page, string freeText);
        Task<List<OptionSimple>> GetAllEmployeeAsync();
    }
}
