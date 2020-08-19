using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VietStar.Entities;
using VietStar.Entities.Courier;
using VietStar.Entities.ViewModels;

namespace VietStar.Repository.Interfaces
{
    public interface ICourierRepository
    {
        Task<List<CourierIndexModel>> GetsAsync(string freeText
            , int assigneeId
            , int userId
            , string status
            , int page
            , int limit
            , int groupId = 0
            , int provinceId = 0
            , string saleCode = null);
        Task<RepoResponse<bool>> UpdateAsync(CourierSql model);
        Task<CourierIndexModel> GetByIdAsync(int id);
        Task<RepoResponse<int>> CreateAsync(CourierSql model, int groupId = 0);
        Task<bool> InsertCourierAssigneeAsync(int courierId, int assigneeId);
    }
}

