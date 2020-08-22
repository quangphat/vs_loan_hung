using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using McreditServiceCore.Interfaces;
using VietStar.Entities.Mcredit;
using VietStar.Business.Interfaces;
using VietStar.Entities.Commons;
using VietStar.Entities.Infrastructures;
using VietStar.Entities.Messages;
using VietStar.Entities.ViewModels;
using VietStar.Repository.Interfaces;
using VietStar.Utility;
using VietStar.Entities.Note;
using static VietStar.Entities.Commons.Enums;
using Newtonsoft.Json;

namespace VietStar.Business
{
    public class MCreditBusiness : BaseBusiness, IMCreditBusiness
    {
        protected readonly IMCreditRepository _rpMCredit;
        protected readonly IMCreditService _svMcredit;
        protected readonly INoteRepository _rpNote;
        protected readonly ILogRepository _rpLog;
        public MCreditBusiness(IMCreditRepository mcreditRepository,
            IMCreditService mCreditService,
            INoteRepository noteRepository,
            ILogRepository logRepository,
            IMapper mapper, CurrentProcess process) : base(mapper, process)
        {
            _rpMCredit = mcreditRepository;
            _svMcredit = mCreditService;
            _rpNote = noteRepository;
            _rpLog = logRepository;
        }


        public async Task<string> CheckSaleAsync(CheckSaleModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.SaleCode))
                return ToResponse<string>(null, "Dữ liệu không hợp lệ");
            var result = await _svMcredit.CheckSale(model.SaleCode);
            if (!result.success)
                return null;
            if (model.ProfileId > 0)
            {

                if (result.success && result.data.obj != null)
                {
                    var sale = _mapper.Map<UpdateSaleModel>(result.data.obj);
                    await _rpMCredit.UpdateSale(sale, model.ProfileId);
                }
            }

            return result.data.msg.ToString();
        }

        public async Task<CheckCatResponseModel> CheckCatAsync(StringModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Value))
                return ToResponse<CheckCatResponseModel>(null, Errors.invalid_data);
            var result = await _svMcredit.CheckCat(model.Value);
            if (!result.success)
                return ToResponse(result);
            return result.data;
        }

        public async Task<bool> IsCheckCatAsync(StringModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Value))
                return ToResponse(false, Errors.invalid_data);
            var result = await _rpMCredit.IsCheckCat(model.Value);
            return result;
        }

        public async Task<CheckCICResponseModel> CheckCICAsync(CheckCICModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.IdNumber))
                return ToResponse<CheckCICResponseModel>(null, Errors.invalid_data);
            var result = await _svMcredit.CheckCIC(model.IdNumber, model.Name);
            if (!result.success)
                return ToResponse(result);
            return result.data;
        }

        public async Task<CheckDupResponseModel> CheckDupAsync(StringModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Value))
                return ToResponse<CheckDupResponseModel>(null, Errors.invalid_data);
            var result = await _svMcredit.CheckDup(model.Value);
            if (!result.success)
                return ToResponse(result);
            return result.data;
        }

        public async Task<CheckStatusResponseModel> CheckStatusAsync(StringModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Value))
                return ToResponse<CheckStatusResponseModel>(null, Errors.invalid_data);
            var result = await _svMcredit.CheckStatus(model.Value);
            if (!result.success)
                return ToResponse(result);
            return result.data;
        }

        public async Task<DataPaging<List<ProfileSearchSql>>> SearchsTemsAsync(string freeText, string status, int page = 1, int limit = 20)
        {
            page = page <= 0 ? 1 : page;
            var profiles = await _rpMCredit.GetTempProfiles(page, limit, freeText, _process.User.Id, status);
            if (profiles == null || !profiles.Any())
            {
                return DataPaging.Create(null as List<ProfileSearchSql>, 0);
            }
            return DataPaging.Create(profiles, profiles[0].TotalRecord);
        }

        public async Task<ProfileSearchResponse> SearchsAsync(string freeText, string status, string type, int page = 1)
        {
            var result = await _svMcredit.SearchProfiles(freeText, status, type, page);
            if (!result.success)
                return ToResponse(result);
            return result.data;
        }

        public async Task<int> CreateDraftAsync(MCredit_TempProfileAddModel model)
        {
            if (model == null)
                return ToResponse(0, Errors.invalid_data);
            var profile = _mapper.Map<MCredit_TempProfile>(model);
            profile.CreatedBy = _process.User.Id;
            var response = await _rpMCredit.CreateDraftProfile(profile);
            if (!response.success)
                return ToResponse(response);
            if (response.data > 0)
            {
                if (!string.IsNullOrWhiteSpace(model.LastNote))
                {
                    var note = new NoteAddModel
                    {
                        CommentTime = DateTime.Now,
                        Content = model.LastNote,
                        ProfileId = response.data,
                        ProfileTypeId = (int)NoteType.MCreditTemp,
                        UserId = _process.User.Id
                    };
                    await _rpNote.AddNoteAsync(note);
                }

            }
            return response.data;
        }

        public async Task<bool> AddRefuseReasonToNoteAsync(int profileId, string type)
        {
            if (profileId <= 0 || string.IsNullOrWhiteSpace(type))
                return ToResponse(false, Errors.invalid_data);
            var response = await _svMcredit.GetProfileById(profileId.ToString());

            if (!response.success)
            {
                return ToResponse<bool>(false, response.error);
            }
            if (response.data.obj == null)
            {
                return ToResponse(false, "Không tìm thấy hồ sơ");
            }
            var profileInPortal = await _rpMCredit.GetTemProfileByMcId(response.data.obj.Id);
            if (!profileInPortal.success)
            {
                return ToResponse(false, "Không tìm thấy hồ sơ trong portal");
            }
            if (type == "refuse")
            {
                return await AddRefuseToNote(profileInPortal.data.Id, response.data.obj);
            }
            if (type == "reason")
            {
                return await AddReasonToNote(profileInPortal.data.Id, response.data.obj);
            }
            return true;

        }

        public async Task<bool> UpdateTempProfileStatusAsync(int profileId)
        {
            var temp = await _rpMCredit.GetTemProfileById(profileId);
            if (!temp.success)
            {
                return ToResponse(false, "Không tìm thây hồ sơ trong portal");
            }
            if (string.IsNullOrWhiteSpace(temp.data.MCId))
            {
                return ToResponse(false, "Hồ sơ chưa được đẩy qua mc");
            }
            var mcProfile = await _svMcredit.GetProfileById(temp.data.MCId);
            if (!mcProfile.success)
            {
                return ToResponse(false, mcProfile.error);
            }
            int status = 0;
            try
            {
                status = Convert.ToInt32(mcProfile.data.obj.Status);
            }
            catch
            {
                status = temp.data.Status;
            }
            if (temp.data.Status != status)
            {
                var result = await _rpMCredit.UpdateTempProfileStatusAsync(temp.data.Id, status);
                return ToResponse(result);
            }
            return true;
        }

        public async Task<bool> UpdateDraftAsync(MCredit_TempProfileAddModel model)
        {
            if (model == null || model.Id <= 0)
                return ToResponse(false, Errors.invalid_data);
            var profile = _mapper.Map<MCredit_TempProfile>(model);
            profile.UpdatedBy = _process.User.Id;
            
            profile.Status = _process.User.isAdmin ? model.Status : (int)MCreditProfileStatus.Submit;
            await _rpLog.InsertLog("mcredit-UpdateDraft", model.Dump());
            var result = await _rpMCredit.UpdateDraftProfile(profile);
            if (!result.success)
            {
                return ToResponse(result);
            }

            if (!string.IsNullOrWhiteSpace(model.LastNote))
            {
                var note = new NoteAddModel
                {
                    UserId = _process.User.Id,
                    ProfileId = model.Id,
                    Content = model.LastNote,
                    CommentTime = DateTime.Now,
                    ProfileTypeId = (int)NoteType.MCreditTemp
                };
                await _rpNote.AddNoteAsync(note);
            }
            
            return true;
        }

        public async Task<MCredit_TempProfile> GetTempProfileByIdAsync(int id)
        {
            var result = await _rpMCredit.GetTemProfileById(id);
            return ToResponse(result);
        }




        protected async Task<bool> AddRefuseToNote(int profileId, ProfileGetByIdResponseObj profile)
        {

            if (profile != null && !string.IsNullOrWhiteSpace(profile.Refuse))
            {
                await _rpNote.AddNoteAsync(new NoteAddModel
                {
                    ProfileId = profileId,
                    CommentTime = DateTime.Now,
                    Content = profile.Refuse,
                    ProfileTypeId = (int)NoteType.MCreditTemp,
                    UserId = _process.User.Id
                });
                await _rpLog.InsertLog($"AddRefuseToNote-{profileId}", profile.Refuse);
            }
            return true;

        }
        protected async Task<bool> AddReasonToNote(int profileId, ProfileGetByIdResponseObj profile)
        {

            if (profile != null && profile.Reason != null)
            {
                var reasonName = JsonConvert.SerializeObject(profile.Reason);
                if (!string.IsNullOrWhiteSpace(reasonName))
                {
                    await _rpNote.AddNoteAsync(new NoteAddModel
                    {
                        ProfileId = profileId,
                        CommentTime = DateTime.Now,
                        Content = reasonName,
                        ProfileTypeId = (int)NoteType.MCreditTemp,
                        UserId = _process.User.Id
                    });
                }
                await _rpLog.InsertLog($"AddReasonToNote-{profileId}", reasonName);
            }


            return true;

        }

        public async Task<List<OptionSimple>> GetSimpleListByTypeAsync(string type)
        {
            var result = new List<OptionSimple>();
            switch (type)
            {
                case "city":
                    result = await _rpMCredit.GetMCCitiesSimpleList();
                    break;
                case "loanperiod":
                    result = await _rpMCredit.GetMCLoanPerodSimpleList();
                    break;
                case "location":
                    result = await _rpMCredit.GetMCLocationSimpleList();
                    break;
                case "product":
                    result = await _rpMCredit.GetMCProductSimpleList();
                    break;
                default:break;
            }
            return result;
        }


    }
}
