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

namespace MCreditService
{
    public class MCreditLoanService : MCreditServiceBase, IMCreditService
    {
        public MCreditLoanService(IMCeditRepository mCeditBusiness, ILogRepository logRepository) : base(mCeditBusiness, logRepository)
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
        public async Task<CheckCICResponseModel> CheckCIC(string idNumber, string name, int userId)
        {
            var model = new CheckCICRequestModel { IdNumber = idNumber, name = name };
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
        public async Task<ProfileGetByIdResponse> GetProfileById(string profileId ,int userId)
        {
            var model = new ProfileGetByIdRequest
            {
                Id = profileId
            };
            var result = await BeforeSendRequest<ProfileGetByIdResponse, ProfileGetByIdRequest>(_getProfileByIdApi, model, userId);
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
        public async Task<NoteResponseModel> GetNotes(string profileId, int userId)
        {
            var model = new NoteRequestModel
            {
                Id = profileId
            };
            await _rpLog.InsertLog("mcredit-GetNote-request", model.Dump());

            var result = await BeforeSendRequest<NoteResponseModel, NoteRequestModel>(_get_notes_Api, model, userId);
            await _rpLog.InsertLog("mcredit-GetNote-result", result.Dump());
            return result;
        }
        public async Task<NoteAddResponseModel> AddNote(NoteAddRequestModel model, int userId)
        {
            var result = await BeforeSendRequest<NoteAddResponseModel, NoteAddRequestModel>(_add_notes_Api, model, userId);
            await _rpLog.InsertLog("mcredit-AddNote", result.Dump());
            return result;
        }
        public async Task<MCResponseModelBase> SendFiles(int userId, string fileName, string profileId)
        {
            // You need to do this download if your file is on any other server otherwise you can convert that file directly to bytes  
            //WebClient wc = new WebClient();
            //byte[] bytes = wc.DownloadData(fileName); 
            
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            byte[] data = new byte[fs.Length];
            fs.Read(data,0 , data.Length);
            fs.Close();
            Dictionary<string, object> postParameters = new Dictionary<string, object>();
            postParameters.Add("file", new FileParameter(data,"18.zip", "application/zip"));
            postParameters.Add("id", profileId);
            return await BeforeSendRequestUploadFile<MCResponseModelBase, MCreditRequestModelBase>(_upload_file_Api, postParameters, userId);
        }
    }
}
