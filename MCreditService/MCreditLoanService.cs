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
        public MCreditLoanService(IMCeditBusiness mCeditBusiness) : base(mCeditBusiness)
        {

        }

        public async Task<CheckCatResponseModel> CheckCat(int userId, string taxNumber)
        {
            var model = new CheckCatRequestModel { taxNumber = taxNumber, UserId = userId };
            var result = await BeforeSendRequest<CheckCatResponseModel, CheckCatRequestModel>(_checkCATApi, model, userId);
            return result;
        }
        public async Task<CheckDupResponseModel> CheckDup(string value)
        {
            var model = new CheckDupRequestModel { IdNumber = value };
            var result = await BeforeSendRequest<CheckDupResponseModel, CheckDupRequestModel>(_checkDupApi, model);
            return result;
        }
        public async Task<CheckCICResponseModel> CheckCIC(string value)
        {
            var model = new CheckCICRequestModel { IdNumber = value };
            var result = await BeforeSendRequest<CheckCICResponseModel, CheckCICRequestModel>(_checkCICApi, model);
            return result;
        }
        public async Task<CheckStatusResponseModel> CheckStatus(string value)
        {
            var model = new CheckStatusRequestModel { IdNumber = value };
            var result = await BeforeSendRequest<CheckStatusResponseModel, CheckStatusRequestModel>(_checkStatusApi, model);
            return result;
        }
    }
}
