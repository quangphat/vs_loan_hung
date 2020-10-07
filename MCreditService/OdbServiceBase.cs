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
    public abstract class OdbServiceBase
    {
        //pro
        protected static string _baseUrl = "https://prepro.m-ocb.com.vn:8889";
        protected static string _userName = "vietbank_test";
        protected static string _password = "Abc@1234!";
        protected static string _authenToken = "bearer 7V83nVoWNIfgRiv250kuu_7VhjS_5PSovGX3a1mgaJs9ip-K5K_o1u5tLYUsg8MMeH_Z5gFb01aCTWEiNW2uoiVz9IPaAiXtB1LrLFkyf3axQFVczmb8MaIMc5oHiiktPruyjArdxXDBfCfXMFWsBx0emU_20GNaKsDpcKOvpzIVui-ab3TTbMRrwgFs06K93daQVo2kV5c_6RVkVy_1K0D6LHYNvUf12Nb988IQvjVi_kJVFeCMcQCJ_YB4ABAqn4-XlmrqomuGKjSnMTecmGHsHsiNbQjfJEXIXsmqaRx67zvwj0SGjOGSF-gE4lIHijtvFxY8LySZTheCPdKqvE1wKXRDKHqJAFPHn2ttHrWwLexSA9jyHKc0zblBEIi9xPcTLoWIE_l3YVSg9XTxdMn7BcdZnOT0lh9ZY9LbJbteKtc1I7wSlLYD019M_0dpkeWR8Zvg0_R8Bx0FbYZBLXkE8JmsFX4ZG01RnuCIZCQm5jbc1Y8i0zKOHB7xF7Dw";
        protected static string _xdnCode = "TWpBeU1FUjFibWRBVG1Wdk1qQXlNQT09";

   
       
        protected static string _contentType = "application/json";
        protected static string _checkValidData = "api/VietBank/CheckValidData";
        protected static string _checkCATApi = "api/act/checkcat.html";
        protected static string _createLead = "api/VietBank/CreateNewLead";
        protected static string _getAllDictionary = "api/MasterData/GetDictionaryList";
        protected static string _getAllProvince = "api/MasterData/GetAllProvince";
        protected static string _getallcity = "api/MasterData/GetAllCity";
        protected static string _getallWard = "api/MasterData/GetAllWard";


        protected readonly HttpClient _httpClient;
        protected HttpRequestMessage _requestMessage;
        protected readonly IOcbRepository _bizMcredit;
        protected readonly ILogRepository _rpLog;
        protected int _userId;
        protected string _userToken;
        protected readonly IMapper _mapper;
        protected OdbServiceBase(IOcbRepository mCeditBusiness, ILogRepository logRepository)
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
