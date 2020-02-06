using EasyCreditService.Infrastructure;
using EasyCreditService.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity.EasyCredit;
using VS_LOAN.Core.Utility;

namespace EasyCreditService.Classes
{
    public class LoanRequest : BaseService, ILoanRequest
    {
        public LoanRequest(HttpClient httpClient) : base(httpClient, typeof(LoanRequest))
        {

        }
        public async Task<string> CreateLoan(LoanInfoRequestModel model, int type=0)
        {
            try
            {
                _log.Info("getting ip addrress");
                var ip = GetPublicIP();
                _log.InfoFormat("the ip address is: {0}", ip);

                _log.InfoFormat("start send loan request at {0}", DateTime.Now);
                var response = await _httpClient.Post(ECApiPath.ECBasePathTest, ECApiPath.LoanRequest, null, model, type);
                
                _log.Info(response);
                _log.Info("send loan request success");
                return response.StatusCode.ToString();
            }
            catch (Exception e)
            {
                _log.Error("exception while sending loan request");
                _log.ErrorFormat("Message: {0}", e.Message);
                _log.ErrorFormat("All exception content :{0}", e);
                return e.InnerException.ToString();
            }
        }
        public string GetPublicIP()
        {
            String direction = "";
            WebRequest request = WebRequest.Create("http://checkip.dyndns.org/");
            using (WebResponse response = request.GetResponse())
            using (StreamReader stream = new StreamReader(response.GetResponseStream()))
            {
                direction = stream.ReadToEnd();
            }

            //Search for the ip in the html
            int first = direction.IndexOf("Address: ") + 9;
            int last = direction.LastIndexOf("</body>");
            direction = direction.Substring(first, last - first);

            return direction;
        }
    }
}
