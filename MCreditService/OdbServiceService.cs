using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity.MCreditModels;
using HttpClientService;
using System.Net.Http.Headers;
using VS_LOAN.Core.Repository;
using MCreditService.Interfaces;
using VS_LOAN.Core.Repository.Interfaces;
using VS_LOAN.Core.Entity.HosoCourrier;
using System.IO;
using System.Net;
using VS_LOAN.Core.Utility;
using VS_LOAN.Core.Entity.MCreditModels.SqlModel;
using Newtonsoft.Json;

namespace MCreditService
{
    public class OdbServiceService : OdbServiceBase, IOdcService
    {

        public static List<ProvinceItem> AllProvice { get; set; }
        public static List<DictionaryResponseItem> AllDictionary { get; set; }
        public static List<CityItem> AllCity { get; set; }
        public static List<WardResponseItem> AllWard { get; set; }

        public static AuthenResponseModel _authenGlobal;       
        public OdbServiceService(IOcbRepository mCeditBusiness, ILogRepository logRepository) : base(mCeditBusiness, logRepository)
        {


        }

        public async Task<CheckVavlidOdcResponseModel> CheckValidData(string mobilePhone, string idNo)
        {

            var client = new HttpClient();
            client.BaseAddress = new Uri(_baseUrl);
            client.DefaultRequestHeaders.Add("Authorization", _authenToken);
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue(_contentType));
            var url = _checkValidData;
            var request = new CheckValidOdcRequestModel() { IdNo = idNo, MobilePhone = mobilePhone };
            var json = JsonConvert.SerializeObject(request);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(url, data);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                CheckVavlidOdcResponseModel contributors = JsonConvert.DeserializeObject<CheckVavlidOdcResponseModel>(content);
           

                return await Task.FromResult(contributors);
            }
            else
            {
                return new CheckVavlidOdcResponseModel()
                {
                    Data = new ValidDataOdbResponse()
                    {
                        ValidData = false
                    }
                };
            }
        }
        public async Task<OcbLeadResponseModel> CreateLead(OcbProfile model)
        {

            var client = new HttpClient();
            client.BaseAddress = new Uri(_baseUrl);
            client.DefaultRequestHeaders.Add("Authorization", _authenToken);
            client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue(_contentType));
            var url = _createLead;

            var validResponse = await CheckValidData(model.Mobilephone, model.IdCard);

            if (validResponse.Status != "200")
            {
                return new OcbLeadResponseModel() { Status = "400", ErrorCode = "", Message = "Bị trùng số điện thoại" };
            }
            else
            {

               
            }

            var request = new CreateLeadRequest()
            {
                Birthday = model.BirthDay,
                CurAddressDistId = model.CurAddressDistId,
                CurAddressProvinceId = model.CurAddressProvinceId,
                CurAddressWardId = model.CurAddressWardId,
                FullName = model.FullNamme,
                Gender = model.Gender == true ? 1 : 0,
                IdCard = model.IdCard,
                IdIssueDate = model.IdIssueDate,
                Income = model.InCome != null ? model.InCome.Value : 0,
                Mobilephone = model.Mobilephone,
                RegAddressDistId = model.RegAddressDistId,
                RequestLoanTerm = model.RequestLoanTerm != null ? model.RequestLoanTerm.Value : 0,
                SellerNote = model.SellerNote,
                TraceCode = model.TraceCode,
                ProductId = model.ProductId,
                RegAddressProvinceId = model.RegAddressProvinceId,
                RegAddressWardId = model.RegAddressWardId,
                RequestLoanAmount = model.RequestLoanAmount != null ? model.RequestLoanAmount.Value : 0,
                ReferenceFullName1 =model.ReferenceFullName1,
                ReferencePhone1 = model.ReferencePhone1,
                ReferenceRelationship1 = model.ReferenceRelationship1,
                Reference1Gender = model.Reference1Gender,

                ReferenceFullName2 = model.ReferenceFullName2,
                ReferencePhone2 = model.ReferencePhone2,
                ReferenceRelationship2 = model.ReferenceRelationship2,
                Reference2Gender = model.Reference2Gender,
                ReferenceFullName3 = model.ReferenceFullName3,
                ReferencePhone3 = model.ReferencePhone3,
                ReferenceRelationship3 = model.ReferenceRelationship3,
                Reference3Gender = model.Reference3Gender,
            };

            string intcomtypeitem = "1";
            if (model.IncomeType != "9")
            {

                intcomtypeitem = "2";
            }
           
            var metadata = new MetaData()
            {
                CurAddressNumber = model.CurAddressNumber,
                CurAddressRegion = model.CurAddressRegion,
                CurAddressStreet = model.CurAddressStreet,
                Email = model.Email,
                IncomeType = intcomtypeitem,
                RegAddressNumber = model.RegAddressNumber,
                RegAddressRegion = model.RegAddressRegion,
                RegAddressStreet = model.RegAddressStreet
                
            };


            request.MetaData = JsonConvert.SerializeObject(metadata); 
            var json = JsonConvert.SerializeObject(request);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
        
            var response = await client.PostAsync(url, data);

           
            if(response.IsSuccessStatusCode)
            {

                 var content = await response.Content.ReadAsStringAsync();
                var resultReponse = JsonConvert.DeserializeObject<OcbLeadResponseModel>(content);

                if(resultReponse.Status=="200")
                {

                    var dataReponse = resultReponse.Data;

                    var customenrId = dataReponse.CustomerId;
                    if (customenrId != null || customenrId !="")
                    {
                        var updateCustomerId = await _bizMcredit.UpdateStatusComplete(Convert.ToInt32(customenrId), model.Id);
                    }

                }
                else
                {

                }
                return resultReponse;
            }
            return new OcbLeadResponseModel();


       

        }
        public async Task<ALlCityResponseModel> GetAllCity(CityRequestModel model)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(_baseUrl);
            client.DefaultRequestHeaders.Add("Authorization", _authenToken);
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue(_contentType));
            var url = _getallcity;
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                ALlCityResponseModel contributors = JsonConvert.DeserializeObject<ALlCityResponseModel>(content);
                var allProvicec = JsonConvert.DeserializeObject<List<CityItem>>(contributors.ResponseData);

                contributors.listCity = allProvicec;
                AllCity = allProvicec;
                return await Task.FromResult(contributors);
            }
            else
            {

            }
            return await Task.FromResult(new ALlCityResponseModel());
        }

        public async Task<DictionaryResponseModel> GetAllDictionary()
        {
            var client = new HttpClient();

            client.BaseAddress = new Uri(_baseUrl);
            client.DefaultRequestHeaders.Add("Authorization", _authenToken);
            client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue(_contentType));
            var url = _getAllDictionary;
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                DictionaryResponseModel contributors = JsonConvert.DeserializeObject<DictionaryResponseModel>(content);
                var allProvicec = JsonConvert.DeserializeObject<List<DictionaryResponseItem>>(contributors.ResponseData);

                contributors.listDictionary = allProvicec;
                AllDictionary = allProvicec;
                return await Task.FromResult(contributors);
            }
            else
            {

            }
            return await Task.FromResult(new DictionaryResponseModel());
        }

        public async Task<ProvinceResponseModel> GetAllProvince()
        {
            var client = new HttpClient();

            client.BaseAddress = new Uri(_baseUrl);
            client.DefaultRequestHeaders.Add("Authorization", _authenToken);
            client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue(_contentType));
            var url = _getAllProvince;
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                ProvinceResponseModel contributors = JsonConvert.DeserializeObject<ProvinceResponseModel>(content);
                var allProvicec = JsonConvert.DeserializeObject<List<ProvinceItem>>(contributors.ResponseData);

                contributors.listProvinceItem = allProvicec;
                AllProvice = allProvicec;
                return await Task.FromResult(contributors);
            }
            else
            {
              
            }

            return await Task.FromResult(new ProvinceResponseModel());
        }

        public async Task<WardResponseModel> GetAllWard(WardRequestModel model)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(_baseUrl);
            client.DefaultRequestHeaders.Add("Authorization", _authenToken);
            client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue(_contentType));
            var url = _getallWard;
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                WardResponseModel contributors = JsonConvert.DeserializeObject<WardResponseModel>(content);
                var allProvicec = JsonConvert.DeserializeObject<List<WardResponseItem>>(contributors.ResponseData);
                contributors.listWardResponse = allProvicec;
                AllWard  = allProvicec;
                return await Task.FromResult(contributors);
            }
            else
            {

            }

            return await Task.FromResult(new WardResponseModel());
        }
        public async Task<bool> CheckAuthen ()
        {

            if(_authenGlobal ==null || _authenGlobal.ValidTo ==null ||  _authenGlobal.ValidTo.Value.AddSeconds(3600) >DateTime.Now)
            {
                await Authen();
                await GetAllCity(new CityRequestModel());
                await GetAllProvince();
                await GetAllWard(new WardRequestModel());
                await GetAllDictionary();
            }
            return true;
             
        }
        public async Task<bool> Authen()
        {
            var nvc = new List<KeyValuePair<string, string>>();
            nvc.Add(new KeyValuePair<string, string>("grant_type", "password"));
            nvc.Add(new KeyValuePair<string, string>("username",_userName));
            nvc.Add(new KeyValuePair<string, string>("password", _password));
            var client = new HttpClient();
            client.BaseAddress = new Uri(_baseUrl);
            var req = new HttpRequestMessage(HttpMethod.Post,_token ) { Content = new FormUrlEncodedContent(nvc) };
            var res = await client.SendAsync(req);
            if (res.IsSuccessStatusCode)
            {
                var content = await res.Content.ReadAsStringAsync();
                AuthenResponseModel contributors = JsonConvert.DeserializeObject<AuthenResponseModel>(content);
                int expries_in = 0;
                try
                {
                    expries_in = int.Parse(contributors.Expires_in);
                }
                catch (Exception)
                {


                }
                contributors.ValidTo = DateTime.Now.AddSeconds(expries_in);
                _authenToken = "bearer " + contributors.Access_token;
                _authenGlobal = contributors;

            }
            else
            {

            }

            return true;

        }

        public async Task<OcbSendfileReponseModel> SendFile(OcbSendfileReQuestModel model)
        {

            var client = new HttpClient();
            client.BaseAddress = new Uri(_baseUrl);
            client.DefaultRequestHeaders.Add("Authorization", _authenToken);
            client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue(_contentType));
            var url = _addDocument;
            var request = new OcbSendfileReQuestModel()
            {
               CustomerId  = model.CustomerId,
               Fieldname = model.Fieldname,
               FileType = model.FileType,
               FileContent= model.FileContent,
            };
            var json = JsonConvert.SerializeObject(request);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, data);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var resultReponse = JsonConvert.DeserializeObject<OcbSendfileReponseModel>(content);
                if (resultReponse.Status == "200")
                {
                    return new OcbSendfileReponseModel()
                    {
                        Message = "Đẩy file thành công",
                        Status = resultReponse.Status
                    };
                }
                else
                {
                    return new OcbSendfileReponseModel()
                    {
                        Message = "Bị lỗi",
                        Status = resultReponse.Status
                    };
                }
               
            }
            return new OcbSendfileReponseModel();
        }
    }
}
