using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity.MCreditModels;
using HttpClientService;
using VS_LOAN.Core.Repository.Interfaces;
using AutoMapper;
using VS_LOAN.Core.Entity;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net;
using System.IO;
using VS_LOAN.Core.Utility;

namespace MCreditService
{
    public abstract class MiraeServiceBase
    {
        //pro
        //protected static string _baseUrl1 = "https://apigw.mafc.vn";
         protected static string _createLead = "/thirdparty/dataentryi";
        protected static string _baseUrl = "https://apigw.mafc.vn";

        protected static string _baseUrls37 = "https://apigw-staging.mafc.vn/cic";

        //protected static string _createLead = "/dataentry-mci";


        protected static string _userName = "vietbank_test";
        protected static string _password = "Abc@1234!";
        protected static string _authenToken = "Basic bWFzdGVyZGF0YW1jaTptYWZjMzI0MTJeJiVeJA==";
        protected static string _xdnCode = "TWpBeU1FUjFibWRBVG1Wdk1qQXlNQT09";
        
        protected static string _contentType = "application/json";
        protected static string _checkValidData = "public/api/v1/customer/CheckCustomer";
        protected static string _checkCATApi = "api/act/checkcat.html";
  

        protected static string _addDocument = "api/VietBank/AddDocument";
        protected static string _getAllDictionary = "api/MasterData/GetDictionaryList";
        protected static string _getAllProvince = "api/MasterData/GetAllProvince";
        protected static string _getallcity = "masterdatamci";
        protected static string _getallWard = "api/MasterData/GetAllWard";

        

        protected static string _token = "/token";
        protected readonly HttpClient _httpClient;
        protected HttpRequestMessage _requestMessage;
        protected readonly IMiraeRepository _bizMcredit;
        protected readonly ILogRepository _rpLog;
        protected int _userId;
        protected string _userToken;
        protected readonly IMapper _mapper;
        protected MiraeServiceBase(IMiraeRepository mCeditBusiness, ILogRepository logRepository)
        {
            _httpClient = new HttpClient();
            _requestMessage = new HttpRequestMessage();
            _requestMessage.Headers.Add("xdncode", _xdnCode);
            _userToken = string.Empty;
            _bizMcredit = mCeditBusiness;
            _rpLog = logRepository;
        }
    }
}
