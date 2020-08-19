using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VietStar.Entities.Courier;
using VietStar.Entities.ViewModels;
using VietStar.Utility;

namespace VietStar.Business.Interfaces
{
    public interface ICourierBusiness
    {
        Task<DataPaging<List<CourierIndexModel>>> GetsAsync(string freeText
            , int assigneeId
            , string status
            , int page
            , int limit
            , int groupId = 0
            , int provinceId = 0
            , string saleCode = null);
        Task<int> CreateAsync(CourierAddModel model);
        Task<bool> UpdateAsync(CourierAddModel model);
        Task<CourierIndexModel> GetByIdAsync(int id);
    }
}
