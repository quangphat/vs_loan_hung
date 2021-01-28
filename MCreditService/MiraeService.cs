﻿using System;
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
    public class MiraeService : MiraeServiceBase, IMiraeService
    {
    
        public static List<MiraeCityItem> AllProvince { get; set; }

        public static List<MiraeDistrictItem> AllDistrict { get; set; }
        public static List<MiraeAllWardItem> AllWard { get; set; }

        public static AuthenResponseModel _authenGlobal;

        private readonly IMiraeRepository _miraeRepository;

        public readonly IMiraeMaratialRepository _rpTailieu;

        public MiraeService(IMiraeRepository mCeditBusiness, ILogRepository logRepository, IMiraeMaratialRepository miraeMaratialRepository) : base(mCeditBusiness, logRepository)
        {


            _miraeRepository = mCeditBusiness;
            _rpTailieu = miraeMaratialRepository;

        }

        public async Task<CheckCustomerResponseModel> CheckCustomer(string searchVal, string partner)
        {

            var client = new HttpClient();
            client.BaseAddress = new Uri(_baseUrl);          
            client.DefaultRequestHeaders.Add("Authorization", "Basic M3AtY2hlY2tjdXN0b21lci1zYms6cFppYTBJWFJ0OUlaWjR2aGFTZXhFOXlCSGljdEQ5Vjc=");
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue(_contentType));
            var url = "/thirdparty/checkcustomer";
            var request = new CheckCustomerRequestModel() {
                partner = partner,
                searchVal =searchVal
            };
            var json = JsonConvert.SerializeObject(request);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, data);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                CheckCustomerResponseModel contributors = JsonConvert.DeserializeObject<CheckCustomerResponseModel>(content);
                return await Task.FromResult(contributors);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                CheckCustomerResponseModel contributors = JsonConvert.DeserializeObject<CheckCustomerResponseModel>(content);
                return contributors;
            }

        }
        public async Task<bool> CheckAuthen ()
        {
            if (AllProvince ==null)
            {    
                await GetAllProvince(new MiraeCityRequest());
                await GetAllWard(new MiraeCityRequest());
                await GetDistrict(new MiraeCityRequest());
                await GetAllProduct(new SchemeRequestModel());

                await GetAllSaleOffice(new SaleOfficeRequestModel());

                await GetAllUser(new SelectUserRequestModel());
                await GetAllBank(new BankRequestModel());

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
        public async Task<MiraeQDELeadRePonse> QDESubmit(MiraeQDELeadReQuest model)
        {

            var client = new HttpClient();
            model.in_channel = "SBK";
            model.msgName = "inputQDE";
            model.in_userid = "EXT_SBK";
            model.in_per_cont = "100";
            model.in_bankbranchcode = "01";
            model.in_head = "NETINCOM";
            model.in_frequency = "MONTHLY";
  
            model.in_possipbranch = "14";
            model.in_creditofficercode = "EXT_SBK";
            model.in_sourcechannel = "ADVT";
            model.in_possipbranch = "14";
            model.in_per_cont = "100";
            model.in_debit_credit = "P";
            model.in_referalgroup = "3";
            model.in_natureofbuss = "";
            model.in_previousjobmth = 0;
            model.in_previousjobyear = 0;
            model.in_natureofbuss = "hoat dong lam thue cac cong viec trong cac hgd,sx sp vat chat va dich vu tu tieu dung cua ho gia dinh";
            //model.in_tenure = 12;
            client.BaseAddress = new Uri(_baseUrl);
            if(string.IsNullOrEmpty(model.in_tax_code))
            {
                model.in_tax_code = "1111111111";
            }
            //;client.DefaultRequestHeaders.Add("Authorization", "Basic ZGF0YWVudHJ5bWNpOm1pcmFlNTIzNDUhQCMlJA==");
            client.DefaultRequestHeaders.Add("Authorization", "Basic M3AtZGF0YWVudHJ5LXNiazp1aDlObFVtTkR3anN1Y3BHaFF5d202YndrOXlobEdBaA==");

            client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue(_contentType));
            var url = _createLead;
            var json = JsonConvert.SerializeObject(model);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, data);
            if (response.IsSuccessStatusCode)
            {
            var content = await response.Content.ReadAsStringAsync();
            var resultReponse = JsonConvert.DeserializeObject<MiraeQDELeadRePonse>(content);
            var dataReponse = resultReponse.Data;
             return resultReponse;
            }
            return new MiraeQDELeadRePonse();

        }
        public async Task<MiraeDDELeadRePonse> DDESubmit(MiraeDDELeadReQuest model)
        {

            var client = new HttpClient();
          
            client.BaseAddress = new Uri(_baseUrl);

            //client.DefaultRequestHeaders.Add("Authorization", "Basic ZGF0YWVudHJ5bWNpOm1pcmFlNTIzNDUhQCMlJA==");
            client.DefaultRequestHeaders.Add("Authorization", "Basic M3AtZGF0YWVudHJ5LXNiazp1aDlObFVtTkR3anN1Y3BHaFF5d202YndrOXlobEdBaA==");
            
            client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue(_contentType));


            var url = _createLead;
            var json = JsonConvert.SerializeObject(model);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(url, data);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var resultReponse = JsonConvert.DeserializeObject<MiraeDDELeadRePonse>(content);
                var dataReponse = resultReponse.Data;



                return resultReponse;
            }
            return new MiraeDDELeadRePonse();

        }
        public async Task<QDEToDDERePonse> QDEToDDE(QDEToDDEReQuest model)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(_baseUrl);
               client.BaseAddress = new Uri(_baseUrl);
            //client.DefaultRequestHeaders.Add("Authorization", "Basic M3AtZGF0YWVudHJ5LXNiazp1aDlObFVtTkR3anN1Y3BHaFF5d202YndrOXlobEdBaA==");
            client.DefaultRequestHeaders.Add("Authorization", "Basic M3AtZGF0YWVudHJ5LXNiazp1aDlObFVtTkR3anN1Y3BHaFF5d202YndrOXlobEdBaA==");
            client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue(_contentType));
            var url = _createLead;
            var json = JsonConvert.SerializeObject(model);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, data);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var resultReponse = JsonConvert.DeserializeObject<QDEToDDERePonse>(content);
                var dataReponse = resultReponse.Data;
                return resultReponse;
            }
            else
            {
                return new QDEToDDERePonse();
            }
        }
        public async Task<DDEToPOReponse> DDEToPoR(DDEToPORReQuest model)
        {

            var client = new HttpClient();
            client.BaseAddress = new Uri(_baseUrl);
          
            //client.DefaultRequestHeaders.Add("Authorization", "Basic ZGF0YWVudHJ5bWNpOm1pcmFlNTIzNDUhQCMlJA==");
            client.DefaultRequestHeaders.Add("Authorization", "Basic M3AtZGF0YWVudHJ5LXNiazp1aDlObFVtTkR3anN1Y3BHaFF5d202YndrOXlobEdBaA==");
            client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue(_contentType));
            var url = _createLead;
            var json = JsonConvert.SerializeObject(model);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, data);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var resultReponse = JsonConvert.DeserializeObject<DDEToPOReponse>(content);
                var dataReponse = resultReponse.Data;
                return resultReponse;
            }
            else
            {
                return new DDEToPOReponse();
            }
        }
        private static async Task<T> GetMasterData<T>(string httpMethod,
            string route, string objectRequest)
        {
            using (var client = new HttpClient())
            {
                var userName = "EXT_SBK";
                var passwd = "mafc123!";
                var authToken = Encoding.ASCII.GetBytes($"{userName}:{passwd}");
                client.DefaultRequestHeaders.Add("Authorization", "Basic M3AtZGF0YWVudHJ5bWQtc2JrOkF4eGV2bjVFNUh5QjkyQ2wyYUJOR3RlbFlyblVkbWZr");
                var request = new MiraeCityRequest()
                {
                    msgName = objectRequest

                };
                var url = "/thirdparty/dataentryMD";
                //var url = "/masterdatamci";
                client.BaseAddress = new Uri(_baseUrl);
                var json = JsonConvert.SerializeObject(request);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, data);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var contributors = JsonConvert.DeserializeObject<T>(content);
                    return await Task.FromResult(contributors);
                }
                else
                {
                    return (T)Activator.CreateInstance(typeof(T));
                }
            }
        }
        public async Task<MiraeCityResponseModel> GetAllProvince(MiraeCityRequest model)
        {

            var result = await GetMasterData<MiraeCityResponseModel>("post", "masterdatamci", "getCity");

            AllProvince = result.Data;

            return result;

        }
        public async Task<MiraeDistrictItemResponseModel> GetDistrict(MiraeCityRequest model)
        {

            var result = await GetMasterData<MiraeDistrictItemResponseModel>("post", "masterdatamci", "getDistrict");

            AllDistrict = result.Data;

            return result;



        }
        public async Task<MiraeAllWardResponseModel> GetAllWard(MiraeCityRequest model)
        {
            var result = await GetMasterData<MiraeAllWardResponseModel>("post", "masterdatamci", "getWard");

            AllWard = result.Data;

            return result;

        }
        public Task<MiraeCityResponseModel> GetAllCity(MiraeCityRequest model)
        {
            throw new NotImplementedException();
        }
        public static List<SaleOfficeReponseItem> AllOfficeUser { get; set; }
        public static List<BankItem> AllBank { get; set; }
        public static List<SchemeReponseItem> Allproduct { get; set; }
        public static List<SelectItem> AllSelectUser { get; set; }
        public async Task<SchemeReponseModel> GetAllProduct(SchemeRequestModel model)
        {
            var result = await GetMasterData<SchemeReponseModel>("post", "masterdatamci", "getSchemes");

            Allproduct = result.Data;

            return result;


        }
        public async Task<SelectUserReponseModel> GetAllUser(SelectUserRequestModel model)
        {
            var result = await GetMasterData<SelectUserReponseModel>("post", "masterdatamci", "getSecUser");

            AllSelectUser = result.Data;

            return result;
        }
        public async Task<SaleOfficeReponseModel> GetAllSaleOffice(SaleOfficeRequestModel model)
        {
            var result = await GetMasterData<SaleOfficeReponseModel>("post", "masterdatamci", "getSaleOffice");

            AllOfficeUser = result.Data.Where(x => x.Inspectorname.Contains("SBK")).ToList();
     

            return result;
        }
        public async Task<BankReponseModel> GetAllBank(BankRequestModel model)
        {
            var result = await GetMasterData<BankReponseModel>("post", "masterdatamci", model.msgName);

            AllBank = result.Data;

            return result;
        }
        public async Task<PushToUNDReponse> PushToUND (MultipartFormDataContent request)
        {
           
               //gettailieu 
               var client = new HttpClient();
            client.BaseAddress = new Uri(_baseUrl);
            client.DefaultRequestHeaders.Add("Authorization", "Basic M3AtcHVzaHVuZGVyc3lzdGVtLXNiazpBcmE3cFQ1clFzM1R3RVc4WnR1dWMycTEzdmRUMkhCaQ==");
            var url = "/thirdparty/pushundersystem";
            //var  url = "/dataentry/openapi/pushUnderSystem";
            var json = JsonConvert.SerializeObject(request);
            var response = await client.PostAsync(url, request);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var resultReponse = JsonConvert.DeserializeObject<PushToUNDReponse>(content);
                return resultReponse;
            }
            else
            {

                var content = await response.Content.ReadAsStringAsync();
                var resultReponse = JsonConvert.DeserializeObject<PushToUNDReponse>(content);
                return resultReponse;
            }
         
        }
        public async Task<PushToHistoryReponse> PushToPendHistory(MultipartFormDataContent request)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(_baseUrl);
            client.DefaultRequestHeaders.Add("Authorization", "Basic M3AtcHVzaHBlbmhpc3Rvcnktc2JrOnRqSG9PTTZkRHN0eHU3VVNqeWFWQ1J2a243UThrQVI4");
            var url = "/thirdparty/pushpenhistory";
            var response = await client.PostAsync(url, request);
            if (response.IsSuccessStatusCode)
            {


                var content = await response.Content.ReadAsStringAsync();
                var resultReponse = JsonConvert.DeserializeObject<PushToHistoryReponse>(content);


                return resultReponse;
            }
            else
            {
                return new PushToHistoryReponse();
            }
        }
    }
}
