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

namespace MCreditService
{
    public abstract class MCreditServiceBase
    {
        protected static string _baseUrl = "http://api.taichinhtoancau.vn";
        protected static string _authenApi = "api/act/authen.html";
        protected static string _checkCATApi = "api/act/checkcat.html";
        protected static string _checkDupApi = "api/act/checkdup.html";
        protected static string _checkCICApi = "api/act/checkcic.html";
        protected static string _checkStatusApi = "api/act/checkstatus.html";
        protected static string _checkSaleApi = "api/act/checksale.html";
        protected static string _searchProfilesApi = "api/act/profiles.html";
        protected static string _create_profile_Api = "api/act/profileadd.html";
        protected static string _get_file_upload_Api = "api/act/getformupload.html";
        protected static string _upload_file_Api = "api/act/profiledoc.html";
        protected static string _userName = "vietbankapi";
        protected static string _password = "api@123";
        protected static string _authenToken = "$2y$10$ne/8QwsCG10c.5cVSUW6NO7L3..lUEFItM4ccV0usJ3cAbqEjLywG";
        protected static string _xdnCode = "TWpBeU1HUjFibWR1Wlc4eU1ESXc=";
        protected static string _contentType = "application/json";
        protected readonly HttpClient _httpClient;
        protected HttpRequestMessage _requestMessage;
        protected readonly IMCeditRepository _bizMcredit;
        protected int _userId;
        protected string _userToken;
        protected readonly IMapper _mapper;
        protected MCreditServiceBase(IMCeditRepository mCeditBusiness)
        {
            _httpClient = new HttpClient();
            _requestMessage = new HttpRequestMessage();
            _requestMessage.Headers.Add("xdncode", _xdnCode);
            _userToken = string.Empty;
            _bizMcredit = mCeditBusiness;
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
        public async Task<string> AuthenByUserId(int userId, 
            bool isUpdateToken = true,
            bool isUpdateProduct = false, 
            bool isUpdateLoanPeriod = false,
            bool isUpdateLocation =false, 
            bool isUpdateCity = false)
        {
            if (userId <= 0)
                return "Vui lòng nhập userId";
            var authen = await Authen();
            if (authen == null || authen.Obj == null || string.IsNullOrWhiteSpace(authen.Obj.Token))
                return "Không thể authen";
            string token = authen.Obj.Token;
            if(isUpdateToken)
            {
                _bizMcredit.InsertUserToken(new MCreditUserToken { Token = token, UserId = userId });
            }
            if(isUpdateProduct && authen.Products!=null &&authen.Products.Any())
            {
                await _bizMcredit.DeleteMCTableDatas((int)MCTableType.MCreditProduct);
                await _bizMcredit.InsertProducts(authen.Products);
            }
            if (isUpdateCity && authen.Cities != null && authen.Cities.Any())
            {
                await _bizMcredit.DeleteMCTableDatas((int)MCTableType.MCreditCity);
                await _bizMcredit.InsertCities(authen.Cities);
            }
            if (isUpdateLoanPeriod && authen.LoanPeriods != null && authen.LoanPeriods.Any())
            {
                await _bizMcredit.DeleteMCTableDatas((int)MCTableType.MCreditLoanPeriod);
                await _bizMcredit.InsertLoanPeriods(authen.LoanPeriods);
            }
            if (isUpdateLocation && authen.Locations != null && authen.Locations.Any())
            {
                await _bizMcredit.DeleteMCTableDatas((int)MCTableType.MCreditlocations);
                await _bizMcredit.InsertLocations(authen.Locations);
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
            var result = await _httpClient.PostAsync<T>(_requestMessage, _baseUrl, apiPath, _contentType, null, model);
            if (result == null || result.Data == null)
                return null;
           if(result.Data is T)
            {
                var data = (T)result.Data;
                data.message = JsonConvert.SerializeObject(data.msg);
                return data;
            }
            return result.Data;
        }
        protected async Task<T> BeforeSendRequestFormData<T, TInput>(string apiPath, MultipartFormDataContent formData, int userId)
           where T : MCResponseModelBase
           where TInput : MCreditRequestModelBase
        {
            _requestMessage = new HttpRequestMessage();
            _requestMessage.RequestUri = new Uri($"{_baseUrl}/{_upload_file_Api}");
            _requestMessage.Headers.Add("xdncode", _xdnCode);
            var token = await GetUserToken(userId);
            HttpContent stringContent = new StringContent(token);
            formData.Add(stringContent, "token");
            _requestMessage.Content = formData;
            _requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            _requestMessage.Method = HttpMethod.Post;
           
            var newRequest = await _requestMessage.CloneAsync();
            //newRequest.Content = formData;
            try
            {
                
                using (var response = await _httpClient.SendAsync(newRequest).ConfigureAwait(false))
                {
                    var result = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception e)
            {

            }
                return null;
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
                _bizMcredit.InsertUserToken(new MCreditUserToken { UserId = _userId, Token = _userToken });
            }
            return _userToken;
        }
    }
}
