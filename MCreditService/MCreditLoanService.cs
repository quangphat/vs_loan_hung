using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity.MCreditModels;
using HttpClientService;
using System.Net.Http.Headers;
using VS_LOAN.Core.Business;
using MCreditService.Interfaces;
using VS_LOAN.Core.Business.Interfaces;
using VS_LOAN.Core.Entity.HosoCourrier;

namespace MCreditService
{
    public class MCreditLoanService : MCreditServiceBase, IMCreditService
    {
        public MCreditLoanService(IMCeditRepository mCeditBusiness) : base(mCeditBusiness)
        {

        }

        public async Task<CheckCatResponseModel> CheckCat(int userId, string taxNumber)
        {
            var model = new CheckCatRequestModel { taxNumber = taxNumber, UserId = userId };
            var result = await BeforeSendRequest<CheckCatResponseModel, CheckCatRequestModel>(_checkCATApi, model, userId);
            return result;
        }
        public async Task<CheckDupResponseModel> CheckDup(string value, int userId)
        {
            var model = new CheckDupRequestModel { IdNumber = value };
            var result = await BeforeSendRequest<CheckDupResponseModel, CheckDupRequestModel>(_checkDupApi, model,userId);
            return result;
        }
        public async Task<CheckCICResponseModel> CheckCIC(string value, int userId)
        {
            var model = new CheckCICRequestModel { IdNumber = value };
            var result = await BeforeSendRequest<CheckCICResponseModel, CheckCICRequestModel>(_checkCICApi, model, userId);
            return result;
        }
        public async Task<CheckStatusResponseModel> CheckStatus(string value, int userId)
        {
            var model = new CheckStatusRequestModel { IdNumber = value };
            var result = await BeforeSendRequest<CheckStatusResponseModel, CheckStatusRequestModel>(_checkStatusApi, model, userId);
            return result;
        }
        public async Task<ProfileSearchResponse> SearchProfiles(string freetext, string status, string type, int page, int userId)
        {
            var model = new ProfileSearchRequestModel {
                str = freetext,
                page = page,
                status= status,
                type = type
            };
            var result = await BeforeSendRequest<ProfileSearchResponse, ProfileSearchRequestModel>(_searchProfilesApi, model, userId);
            return result;
        }
        public async Task<ProfileAddResponse> CreateProfile(ProfileAddObj obj, int userId)
        {
            var model = new ProfileAddRequest
            {
                obj = obj
            };
            var result = await BeforeSendRequest<ProfileAddResponse, ProfileAddRequest>(_create_profile_Api, model, userId);
            return result;
        }
    }
}
