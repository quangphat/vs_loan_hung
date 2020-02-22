using EasyCreditService.Infrastructure;
using EasyCreditService.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Entity.EasyCredit;
using VS_LOAN.Core.Utility;

namespace EasyCreditService.Classes
{
    public class LoanRequestService : BaseService, ILoanRequestService
    {
        public LoanRequestService(HttpClient httpClient) : base(httpClient, typeof(LoanRequestService))
        {

        }
        public async Task<List<string>> TestVietbankApi()
        {
            ////http://test.smartbank.com.vn/
            //var response = await _httpClient.Get<List<string>>("http://112.213.89.5/plesk-site-preview/vietbankfc.api/api/values");
            ////var result = await _httpClient.GetAsync("http://localhost:5000/api/values");
            //return response;
            ////return content;
            return null;
        }
        public async Task<EcResponseModel<bool>> UploadFile(StringModel model)
        {
            var response = await _httpClient.Post<EcResponseModel<bool>>(ECApiPath.BasePath, "/api/ECCredit/sftp", model);
            return response.Data;
        }
        public async Task<EcResponseModel<EcDataResponse>> CreateLoan(LoanInfoRequestModel model)
        {
            try
            {
                
                _log.InfoFormat("start send loan request at {0}", DateTime.Now);
                
                var response = await _httpClient.Post<EcResponseModel<EcDataResponse>>(ECApiPath.BasePathDev, ECApiPath.Step2, model);
                
                return response.Data;
            }
            catch (Exception e)
            {
                _log.Error("exception while sending loan request");
                _log.ErrorFormat("Message: {0}", e.Message);
                _log.ErrorFormat("All exception content :{0}", e);
                return null;
            }
        }

    }
}
