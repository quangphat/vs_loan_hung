using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Entity.Employee;
using VS_LOAN.Core.Entity.Model;

namespace VS_LOAN.Core.Repository.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<bool> ResetPassord(int id, string password, int updatedBy);
        Task<OptionSimple> GetEmployeeByCode(string code);
        Task<bool> CheckIsAdmin(int userId);
        Task<List<int>> GetPeopleIdCanViewMyProfile(int userId);
        Task<bool> ResetPassord(string userName, string password);
        Task<List<NhanVienInfoModel>> GetCourierList();
        Task<List<OptionSimple>> GetByProvinceId(int provinceId);
        Task<List<IDictionary<string, object>>> QuerySQLAsync(string sql);
        Task<List<OptionSimple>> GetByDistrictId(int districtId);
        Task<Nhanvien> GetByUserName(string userName, int id = 0);
        Task<Nhanvien> GetByCode(string code, int id = 0);
        Task<int> Count(
            DateTime workFromDate,
            DateTime workToDate,
            int roleId,
            string freeText);
        Task<List<EmployeeViewModel>> Gets(
            DateTime workFromDate,
            DateTime workToDate,
            int roleId,
            string freeText,
            int page,
            int limit);
        Task<Nhanvien> GetById(int id);
        Task<List<OptionSimple>> GetRoleList();
        Task<bool> Update(EmployeeEditModel entity);
        Task<int> Create(UserCreateModel entity);
    }
}
