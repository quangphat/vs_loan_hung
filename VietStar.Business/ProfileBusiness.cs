using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using VietStar.Business.Interfaces;
using VietStar.Entities;
using VietStar.Entities.Infrastructures;
using VietStar.Entities.Messages;
using VietStar.Entities.Profile;
using VietStar.Entities.ViewModels;
using VietStar.Repository.Interfaces;
using VietStar.Utility;
using static VietStar.Entities.Commons.Enums;

namespace VietStar.Business
{
    public class ProfileBusiness : BaseBusiness, IProfileBusiness
    {
        protected readonly IProfileRepository _rpProfile;
        public ProfileBusiness(IProfileRepository profileRepository,
            IMapper mapper, CurrentProcess process) : base(mapper, process)
        {
            _rpProfile = profileRepository;
        }

        public async Task<int> CreateAsync(ProfileAdd model)
        {
            if(model==null)
            {
                AddError(Errors.invalid_data);
                return 0;
            }
            if (!string.IsNullOrWhiteSpace(model.Comment) && model.Comment.Length > 200)
            {
                AddError(Errors.note_length_cannot_more_than_200);
                return 0;
            }
            if (model.PartnerId <= 0)
            {
                AddError(Errors.missing_partner);
                return 0;
            }
            if (string.IsNullOrWhiteSpace(model.CustomerName))
            {
                AddError(Errors.customername_must_not_be_empty);
                return 0;
            }
            if (model.ProductId <= 0)
            {
                AddError(Errors.missing_product);
                return 0;
            }
            if (string.IsNullOrWhiteSpace(model.Phone))
            {
                AddError(Errors.missing_phone);
                return 0;
            }
            if (model.ReceiveDate == null)
            {
                AddError(Errors.missing_ngaynhandon);
                return 0;
                
            }
            if (string.IsNullOrWhiteSpace(model.Cmnd))
            {
                AddError(Errors.missing_cmnd);
                return 0;
                
            }
            if (model.DistrictId <= 0)
            {
                AddError(Errors.missing_district);
                return 0;
                
            }
            if (string.IsNullOrWhiteSpace(model.Address))
            {
                AddError(Errors.missing_diachi);
                return 0;
               
            }

            
            if (model.LoanAmount <= 0)
            {
                AddError(Errors.missing_money);
                return 0;
                
            }
            if (model.BirthDay == null)
            {
                AddError(Errors.missing_birthday);
                return 0;
                
            }
            if (model.CmndDay == null)
            {
                AddError(Errors.missing_cmnd_day);
                return 0;  
            }
            model.Status = (int)ProfileStatus.Draft;
            var sqlmodel = _mapper.Map<ProfileAddSql>(model);
            var result = await _rpProfile.CreateAsync(sqlmodel, _process.User.Id);
            return ToResponse(result);
        }

        public async Task<DataPaging<List<ProfileIndexModel>>> GetsAsync(DateTime? fromDate
            , DateTime? toDate
            , int dateType = 1
            , int groupId = 0
            , int memberId = 0
            , string status = null
            , string freeText = null
            , string sort = "desc"
            , string sortField = "updatedtime"
            , int page = 1
            , int limit = 20)
        {
            fromDate = fromDate.HasValue ? fromDate.Value.ToStartDateTime() : DateTime.Now.ToStartDateTime();
            toDate = toDate.HasValue ? toDate.Value.ToEndDateTime() : DateTime.Now.ToEndDateTime();
            var result = await _rpProfile.GetsAsync(_process.User.Id, fromDate.Value, toDate.Value, dateType, groupId, memberId, status, freeText,sort, sortField, page, limit);
            if(result==null || !result.Any())
            {
                return DataPaging.Create((List<ProfileIndexModel>)null, 0);
            }
            return DataPaging.Create(result, result[0].TotalRecord);
            
        }
       public async Task<ProfileAdd> GetByIdAsync(int profileId)
        {
            var data = await _rpProfile.GetByIdAsync(profileId);
            var profile = ToResponse<ProfileDetail>(data);
            var result = _mapper.Map<ProfileAdd>(profile);
            return result;
        }
    }
}
