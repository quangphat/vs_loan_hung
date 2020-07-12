using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity.MCreditModels;

namespace MCreditService.Interfaces
{
    public interface IMCreditService
    {
        Task<CheckSaleResponseModel> CheckSale(int userId, string salecode);
        Task<CheckCatResponseModel> CheckCat(int userId, string taxNumber);
        Task<CheckDupResponseModel> CheckDup(string value, int userId);
        Task<CheckCICResponseModel> CheckCIC(string value, int userId);
        Task<CheckStatusResponseModel> CheckStatus(string value, int userId);
        Task<ProfileSearchResponse> SearchProfiles(string freetext, string status, string type, int page, int userId);
        Task<ProfileAddResponse> CreateProfile(MCProfilePostModel obj, int userId);
        Task<string> AuthenByUserId(int userId,
            bool isUpdateToken = true,
            bool isUpdateProduct = false,
            bool isUpdateLoanPeriod = false,
            bool isUpdateLocation = false,
            bool isUpdateCity = false);
        Task<GetFileUploadResponse> GetFileUpload(GetFileUploadRequest model, int userId);
    }
}
