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
        Task<List<OptionSimple>> GetEmployeesByGroupId(int groupId, bool isLeader = false);
        Task<bool> ResetPassord(int id, string password, int updatedBy);
        Task<OptionSimple> GetEmployeeByCode(string code);
        Task<bool> CheckIsAdmin(int userId);
        Task<List<int>> GetPeopleIdCanViewMyProfile(int userId);
        Task<List<NhanVienInfoModel>> GetCourierList();
        Task<List<OptionSimple>> GetByProvinceId(int provinceId);
        Task<List<IDictionary<string, object>>> QuerySQLAsync(string sql);
        Task<List<OptionSimple>> GetByDistrictId(int districtId);
        Task<Nhanvien> GetByUserName(string userName, int userId);
        Task<Nhanvien> GetByCode(string code, int userId);
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
            int limit,
            int orgId,
            int currentUserId);
        Task<Nhanvien> GetByIdAsync(int id);
        Task<List<OptionSimple>> GetRoleList(int userId);
        Task<bool> Update(EmployeeEditModel entity);
        Task<bool> Delete(EmployeeEditModel entity);
        
        Task<int> Create(UserCreateModel entity);
        Task<List<OptionSimple>> GetAllEmployee(int orgId);
        Task<List<OptionSimple>> GetAllEmployeePaging(int orgId, int page, string freeText);
        bool CapNhat(int maNhanVien, List<int> lstIDNhom);
        Task<List<OptionSimple>> LayDSThanhVienNhomCaConAsync(int groupId, int userId);
    }
}
