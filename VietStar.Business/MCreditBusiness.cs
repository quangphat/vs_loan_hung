using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using McreditServiceCore.Interfaces;
using McreditServiceCore.Models;
using VietStar.Business.Interfaces;
using VietStar.Entities.Commons;
using VietStar.Entities.Infrastructures;
using VietStar.Entities.Mcredit;
using VietStar.Entities.Messages;
using VietStar.Entities.ViewModels;
using VietStar.Repository.Interfaces;
using VietStar.Utility;

namespace VietStar.Business
{
    public class MCreditBusiness : BaseBusiness, IMCreditBusiness
    {
        protected readonly IMCreditRepository _rpMCredit;
        protected readonly IMCreditService _svMcredit;
        public MCreditBusiness(IMCreditRepository mcreditRepository,
            IMCreditService mCreditService,
            IMapper mapper, CurrentProcess process) : base(mapper, process)
        {
            _rpMCredit = mcreditRepository;
            _svMcredit = mCreditService;
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
            if(!result.success)
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
    }
}
