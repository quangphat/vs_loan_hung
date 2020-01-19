using EasyCreditService.Infrastructure;
using EasyCreditService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity.EasyCredit;
using VS_LOAN.Core.Utility;

namespace EasyCreditService.Classes
{
    public class LoanRequest : BaseService, ILoanRequest
    {
        public LoanRequest(HttpClient httpClient):base(httpClient,typeof(LoanRequest))
        {

        }
        public async Task CreateLoan(LoanInfoRequestModel model)
        {
            try
            {
                _log.Info("start send loan request");
                var response = await _httpClient.Post(ECApiPath.ECBasePathTest, ECApiPath.LoanRequest, null, model);
                
                _log.Info(response);
            }
            catch(Exception e)
            {
                _log.Error("exception while sending loan request");
                _log.ErrorFormat("Message: {0}", e.Message);
                _log.ErrorFormat("All exception content :{0}", e);
            }
        }
    }
}
