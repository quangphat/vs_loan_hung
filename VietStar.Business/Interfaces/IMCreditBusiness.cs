using McreditServiceCore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VietStar.Entities.Commons;
using VietStar.Entities.Mcredit;
using VietStar.Entities.ViewModels;
using VietStar.Utility;

namespace VietStar.Business.Interfaces
{
    public interface IMCreditBusiness
    {
        Task<string> CheckSaleAsync(CheckSaleModel model);
        Task<CheckCatResponseModel> CheckCatAsync(StringModel model);
        Task<bool> IsCheckCatAsync(StringModel model);
        Task<CheckCICResponseModel> CheckCICAsync(CheckCICModel model);
        Task<CheckDupResponseModel> CheckDupAsync(StringModel model);
        Task<CheckStatusResponseModel> CheckStatusAsync(StringModel model);
        Task<DataPaging<List<ProfileSearchSql>>> SearchsTemsAsync(string freeText, string status, int page = 1, int limit = 20);
    }
}
