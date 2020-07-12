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

namespace MCreditService
{
    public class MCreditLoanService : MCreditServiceBase, IMCreditService
    {
        public MCreditLoanService(IMCeditRepository mCeditBusiness) : base(mCeditBusiness)
        {

        }

        public async Task<CheckCatResponseModel> CheckCat(int userId, string taxNumber)
        {
            var model = new CheckCatRequestModel { taxNumber = taxNumber, UserId = userId };
            var result = await BeforeSendRequest<CheckCatResponseModel, CheckCatRequestModel>(_checkCATApi, model, userId);
            return result;
        }
        public async Task<CheckSaleResponseModel> CheckSale(int userId, string salecode)
        {
            var model = new CheckSaleRequestModel { idCode = salecode, UserId = userId };
            var result = await BeforeSendRequest<CheckSaleResponseModel, CheckSaleRequestModel>(_checkSaleApi, model, userId);
            return result;
        }
        public async Task<CheckDupResponseModel> CheckDup(string value, int userId)
        {
            var model = new CheckDupRequestModel { IdNumber = value };
            var result = await BeforeSendRequest<CheckDupResponseModel, CheckDupRequestModel>(_checkDupApi, model,userId);
            return result;
        }
        public async Task<CheckCICResponseModel> CheckCIC(string value, int userId)
        {
            var model = new CheckCICRequestModel { IdNumber = value };
            var result = await BeforeSendRequest<CheckCICResponseModel, CheckCICRequestModel>(_checkCICApi, model, userId);
            return result;
        }
        public async Task<CheckStatusResponseModel> CheckStatus(string value, int userId)
        {
            var model = new CheckStatusRequestModel { IdNumber = value };
            var result = await BeforeSendRequest<CheckStatusResponseModel, CheckStatusRequestModel>(_checkStatusApi, model, userId);
            return result;
        }
        public async Task<ProfileSearchResponse> SearchProfiles(string freetext, string status, string type, int page, int userId)
        {
            var model = new ProfileSearchRequestModel {
                str = freetext,
                page = page,
                status= status,
                type = type
            };
            var result = await BeforeSendRequest<ProfileSearchResponse, ProfileSearchRequestModel>(_searchProfilesApi, model, userId);
            return result;
        }
        public async Task<ProfileAddResponse> CreateProfile(MCProfilePostModel obj, int userId)
        {
            var model = new ProfileAddRequest
            {
                Obj = obj
            };
            var result = await BeforeSendRequest<ProfileAddResponse, ProfileAddRequest>(_create_profile_Api, model, userId);
            return result;
        }
        public async Task<GetFileUploadResponse> GetFileUpload(GetFileUploadRequest model, int userId)
        {
            var result = await BeforeSendRequest<GetFileUploadResponse, GetFileUploadRequest>(_get_file_upload_Api, model, userId);
            return result;
        }
        public async Task<bool> SendFiles(int userId)
        {
            byte[] file = File.ReadAllBytes(System.IO.Path.Combine("D:\\Development", "ziiiiiiiiiip.zip"));
            MemoryStream ms = new MemoryStream();
            using (FileStream fs = new FileStream(System.IO.Path.Combine("D:\\Development", "ziiiiiiiiiip.zip"), FileMode.Open, FileAccess.Read))
            {
                fs.CopyTo(ms);
            }
               
            HttpContent stringContent = new StringContent("99999");
            //HttpContent fileStreamContent = new StreamContent(ms);
            HttpContent bytesContent = new ByteArrayContent(file);
            //var url = $"{_baseUrl}/{_upload_file_Api}";
            using (var client = new HttpClient())
            {
                //client.DefaultRequestHeaders.Add("xdncode", _xdnCode);
                //client.ContentType = new MediaTypeHeaderValue("application/json");
                using (var formData = new MultipartFormDataContent())
                {
                    formData.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                    formData.Add(stringContent, "id");
                    formData.Add(bytesContent, "file");
                    await BeforeSendRequestFormData<MCResponseModelBase, MCreditRequestModelBase>(_upload_file_Api, formData, userId);
                    return true;
                }
            }
           
        }
    }
}
