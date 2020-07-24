using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity.HosoCourrier;
using VS_LOAN.Core.Entity.Model;

namespace VS_LOAN.Core.Repository.Interfaces
{
    public interface IHosoCourrierRepository
    {
        Task<UserPMModel> GetEmployeeById(int id);
        Task<HosoCourierViewModel> GetById(int id);
        Task<int> GetGroupIdByNguoiQuanLyId(int leaderId);
        Task<bool> Update(int id, HosoCourier hoso);
        Task<int> Create(HosoCourier hoso, int groupId = 0);
        Task<bool> InsertCourierAssignee(int courierId, int assigneeId);
        Task<int> CountHosoCourrier(string freeText, int courierId, int userId, string status, int groupId = 0, int provinceId = 0,string saleCode= null);
        Task<List<HosoCourierViewModel>> GetHosoCourrier(string freeText, int courierId, int userId, string status, int page, int limit, int groupId = 0, int provinceId = 0,string saleCode = null);
    }
}
