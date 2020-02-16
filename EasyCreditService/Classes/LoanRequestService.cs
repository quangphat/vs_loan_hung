﻿using EasyCreditService.Infrastructure;
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
            //http://test.smartbank.com.vn/
            var response = await _httpClient.Get<List<string>>("http://112.213.89.5/plesk-site-preview/vietbankfc.api/api/values");
            //var result = await _httpClient.GetAsync("http://localhost:5000/api/values");
            return response;
            //return content;
        }
        public async Task<EcResponseModel<EcDataResponse>> CreateLoan(LoanInfoRequestModel model, int type=0)
        {
            try
            {
                _log.Info("getting ip addrress");
                var ip = GetPublicIP();
                _log.InfoFormat("the ip address is: {0}", ip);

                _log.InfoFormat("start send loan request at {0}", DateTime.Now);
                var response = await _httpClient.Post<EcResponseModel<EcDataResponse>>(ECApiPath.ECBasePathTest, ECApiPath.LoanRequest, null, model, type);
                
                _log.Info(response);
                _log.Info("send loan request success");
                return response;
            }
            catch (Exception e)
            {
                _log.Error("exception while sending loan request");
                _log.ErrorFormat("Message: {0}", e.Message);
                _log.ErrorFormat("All exception content :{0}", e);
                return null;
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
