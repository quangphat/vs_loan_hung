using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VietStar.Entities.Commons;
using VietStar.Entities.ViewModels;

namespace VietStar.Repository.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<OptionSimple> GetEmployeeByCode(string code, int userId);
        Task<List<OptionSimple>> GetByProvinceId(int provinceId);
        Task<List<OptionSimple>> GetCouriers(int orgId);
        Task<bool> GetStatus(int userId);
        Task<Account> Login(string userName, string password);
        Task<List<string>> GetPermissions(string  roleCode);
        Task<List<OptionSimple>> GetMemberByGroupIdIncludeChild(int groupId, int userId);
        Task<List<OptionSimple>> GetMemberByGroupIdAsync(int groupId, int userId);
        Task<List<int>> GetPeopleCanViewMyProfile(int profileId);
        Task<List<OptionSimple>> GetAllEmployee(int orgId);
        Task<List<OptionSimple>> GetAllEmployeePagingAsync(int orgId, int page, string freeText);
    }
}
