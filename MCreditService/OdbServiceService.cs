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
        public OdbServiceService(IOcbRepository mCeditBusiness, ILogRepository logRepository) : base(mCeditBusiness, logRepository)
        {

        }

        public Task<CheckVavlidOdcResponseModel> CheckValidData(string mobilePhone, string idNo)
        {
            throw new NotImplementedException();


        }
        public Task<OcbLeadResponseModel> CreateLead(OcbProfile model)
        {
            throw new NotImplementedException();
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
    }
}
