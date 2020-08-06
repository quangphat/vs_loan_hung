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
    public abstract class MCreditServiceBase
    {
        //protected static string _baseUrl = "http://hosoapi.taichinhtoancau.vn";
        protected static string _baseUrl = "http://api.taichinhtoancau.vn";
        protected static string _userName = "vietbankapi";
        //protected static string _password = "@vietb@pi@123";
        protected static string _password = "api@123";//test
        //protected static string _authenToken = "$2y$10$Eeh8kYRifE6Es1NU7UIqNOg6XGgfclFz0xgCObo2L4du8t.5SJVx6";
        protected static string _authenToken =  "$2y$10$ne/8QwsCG10c.5cVSUW6NO7L3..lUEFItM4ccV0usJ3cAbqEjLywG"; //test
        //protected static string _xdnCode = "TWpBeU1FUjFibWRBVG1Wdk1qQXlNQT09";//"TWpBeU1HUjFibWR1Wlc4eU1ESXc=";
        protected static string _xdnCode = "TWpBeU1HUjFibWR1Wlc4eU1ESXc="; //test
        protected static string _contentType = "application/json";
        protected static string _authenApi = "api/act/authen.html";
        protected static string _checkCATApi = "api/act/checkcat.html";
        protected static string _checkDupApi = "api/act/checkdup.html";
        protected static string _checkCICApi = "api/act/checkcic.html";
        protected static string _checkStatusApi = "api/act/checkstatus.html";
        protected static string _checkSaleApi = "api/act/checksale.html";
        protected static string _searchProfilesApi = "api/act/profiles.html";
        protected static string _getProfileByIdApi = "api/act/profile.html";
        protected static string _create_profile_Api = "api/act/profileadd.html";
        protected static string _get_file_upload_Api = "api/act/getformupload.html";
        protected static string _upload_file_Api = "api/act/profiledoc.html";
        protected static string _get_notes_Api = "api/act/notes.html";
        protected static string _add_notes_Api = "api/act/noteadd.html";


        protected readonly HttpClient _httpClient;
        protected HttpRequestMessage _requestMessage;
        protected readonly IMCeditRepository _bizMcredit;
        protected readonly ILogRepository _rpLog;
        protected int _userId;
        protected string _userToken;
        protected readonly IMapper _mapper;
        protected MCreditServiceBase(IMCeditRepository mCeditBusiness, ILogRepository logRepository)
        {
            _httpClient = new HttpClient();
            _requestMessage = new HttpRequestMessage();
            _requestMessage.Headers.Add("xdncode", _xdnCode);
            _userToken = string.Empty;
            _bizMcredit = mCeditBusiness;
            _rpLog = logRepository;
            //var config = new MapperConfiguration(x =>
            //{
            //    x.CreateMap<OptionSimple, ProfileAddObj>()
            //    .ForMember(a => a.name, b => b.MapFrom(c => c.TenKhachHang))
            //    .ForMember(a => a.cityId, b => b.MapFrom(c => c.MaKhuVuc))
            //    .ForMember(a => a.bod, b => b.MapFrom(c => c.BirthDay.ToShortDateString()))
            //    .ForMember(a => a.phone, b => b.MapFrom(c => c.SDT))
            //    .ForMember(a => a.productCode, b => b.MapFrom(c => c.ProductCode))
            //    .ForMember(a => a.idNumber, b => b.MapFrom(c => c.CMND))
            //    .ForMember(a => a.idNumberDate, b => b.MapFrom(c => c.CmndDay.ToShortDateString()))
            //    //.ForMember(a => a.loanPeriodCode, b => b.MapFrom(c => c.CMND))
            //    .ForMember(a => a.loanMoney, b => b.MapFrom(c => c.SoTienVay.ToString()))
            //    //.ForMember(a => a.saleID, b => b.MapFrom(c => c.CMND))
            //    ;

            //});

            //_mapper = config.CreateMapper();
        }
        public async Task<string> AuthenByUserId(int userId, int[] tableToUpdateIds = null)
        {
            if (userId <= 0)
                return "Vui lòng nhập userId";
            var authen = await Authen();
            if (authen == null || authen.Obj == null || string.IsNullOrWhiteSpace(authen.Obj.Token))
                return "Không thể authen";
            string token = authen.Obj.Token;

            _bizMcredit.InsertUserToken(new MCreditUserToken { Token = token, UserId = userId });

            if (tableToUpdateIds == null || !tableToUpdateIds.Any())
                return token;
            if (tableToUpdateIds.Contains((int)MCTableType.MCreditProduct) && authen.Products != null && authen.Products.Any())
            {
                await _bizMcredit.DeleteMCTableDatas((int)MCTableType.MCreditProduct);
                await _bizMcredit.InsertProducts(authen.Products);
                await _rpLog.InsertLog("mcredit-update-product", authen.Products.Dump());
            }
            if (tableToUpdateIds.Contains((int)MCTableType.MCreditCity) && authen.Cities != null && authen.Cities.Any())
            {
                await _bizMcredit.DeleteMCTableDatas((int)MCTableType.MCreditCity);
                await _bizMcredit.InsertCities(authen.Cities);
                await _rpLog.InsertLog("mcredit-update-cities", authen.Cities.Dump());
            }
            if (tableToUpdateIds.Contains((int)MCTableType.MCreditLoanPeriod) && authen.LoanPeriods != null && authen.LoanPeriods.Any())
            {
                await _bizMcredit.DeleteMCTableDatas((int)MCTableType.MCreditLoanPeriod);
                await _bizMcredit.InsertLoanPeriods(authen.LoanPeriods);
                await _rpLog.InsertLog("mcredit-update-loanperiod", authen.LoanPeriods.Dump());
            }
            if (tableToUpdateIds.Contains((int)MCTableType.MCreditlocations) && authen.Locations != null && authen.Locations.Any())
            {
                await _bizMcredit.DeleteMCTableDatas((int)MCTableType.MCreditlocations);
                await _bizMcredit.InsertLocations(authen.Locations);
                await _rpLog.InsertLog("mcredit-update-locations", authen.Locations.Dump());
            }
            if (tableToUpdateIds.Contains((int)MCTableType.MCreditProfileStatus) && authen.ProfileStatus != null && authen.ProfileStatus.Any())
            {
                var list = new List<OptionSimple>();
                foreach (var item in authen.ProfileStatus)
                {
                    list.Add(new OptionSimple { Code = item.Key, Name = item.Value });
                }
                await _bizMcredit.DeleteMCTableDatas((int)MCTableType.MCreditProfileStatus);
                await _bizMcredit.InsertProfileStatus(list);
            }
            return token;
        }
        protected async Task<AuthenResponse> Authen()
        {
            var result = await _httpClient.PostAsync<AuthenResponse>(_requestMessage, _baseUrl, _authenApi, _contentType, null, new AuthenRequestModel
            {
                UserName = _userName,
                UserPass = _password,
                token = _authenToken
            });
            return result.Data;
        }
        protected async Task<T> BeforeSendRequest<T, TInput>(string apiPath, TInput model, int userId = 0)
            where T : MCResponseModelBase
            where TInput : MCreditRequestModelBase
        {
            _requestMessage = new HttpRequestMessage();
            _requestMessage.Headers.Add("xdncode", _xdnCode);
            model.token = await GetUserToken(userId);
            await _rpLog.InsertLog($"mcredit-{apiPath}", model.Dump());
            var result = await _httpClient.PostAsync<T>(_requestMessage, _baseUrl, apiPath, _contentType, null, model, rpLog: _rpLog);
            if (result != null)
            {
               await _rpLog.InsertLog($"mcredit-{apiPath}", result.Dump());
            }
            if (result == null || result.Data == null)
                return null;
            if (result.Data is T)
            {
                var data = (T)result.Data;
                data.message = JsonConvert.SerializeObject(data.msg);
                return data;
            }
            return result.Data;
        }
        protected async Task<T> BeforeSendRequestUploadFile<T, TInput>(string apiPath, Dictionary<string, object> postParameters, int userId)
           where T : MCResponseModelBase
        {
            _requestMessage = new HttpRequestMessage();
            _requestMessage.Headers.Add("xdncode", _xdnCode);
            var token = await GetUserToken(userId);
            try
            {
                string requestURL = $"{_baseUrl}/{apiPath}";

                string userAgent = "vietbank";
                postParameters.Add("token", token);
                var response = await FormUpload.MultipartFormPost<T>(requestURL, userAgent, postParameters, "xdncode", _xdnCode, token);
                await _rpLog.InsertLog($"BeforeSendRequestUploadFile - {apiPath}", response == null ? "response = null" : response.Dump());
                return response.Data;
            }
            catch (Exception exp)
            {
                await _rpLog.InsertLog($"BeforeSendRequestUploadFile - {apiPath}", exp.InnerException == null ? exp.Dump() : exp.InnerException.Dump());
                return null;
            }
        }
        protected async Task<string> GetUserToken(int userId)
        {
            _userId = userId;
            if (!string.IsNullOrWhiteSpace(_userToken))
            {
                return _userToken;
            }
            var tokenFromDb = await _bizMcredit.GetUserTokenByIdAsync(_userId);
            //tokenFromDb = null;
            if (tokenFromDb != null)
            {
                _userToken = tokenFromDb.Token;
            }
            else
            {
                var tokenFromMCApi = await Authen();
                if (tokenFromMCApi == null)
                    return null;
                _userToken = tokenFromMCApi.Obj.Token;
                await _bizMcredit.InsertUserToken(new MCreditUserToken { UserId = _userId, Token = _userToken });
            }
            return _userToken;
        }
        public byte[] FileToByteArray(string fileName)
        {
            byte[] fileData = null;
            if (!File.Exists(fileName))
                return fileData;
            using (FileStream fs = File.OpenRead(fileName))
            {
                var binaryReader = new BinaryReader(fs);
                fileData = binaryReader.ReadBytes((int)fs.Length);
            }
            return fileData;
        }
    }
}
