using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using VietStar.Business.Interfaces;
using VietStar.Entities.Infrastructures;
using VietStar.Entities.Messages;
using VietStar.Entities.ViewModels;
using VietStar.Repository.Interfaces;
using VietStar.Utility;

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
        public async Task<DataPaging<List<ProfileIndexModel>>> Gets(DateTime? fromDate, DateTime? toDate, int dateType = 1, int groupId = 0, int memberId = 0, string status = null, string freeText = null, int page = 1, int limit = 20)
        {
            fromDate = fromDate.HasValue ? fromDate.Value.ToStartDateTime() : DateTime.Now.ToStartDateTime();
            toDate = toDate.HasValue ? toDate.Value.ToEndDateTime() : DateTime.Now.ToEndDateTime();
            var result = await _rpProfile.Gets(_process.User.Id, fromDate.Value, toDate.Value, dateType, groupId, memberId, status, freeText, page, limit);
            if(result==null || !result.Any())
            {
                return DataPaging.Create((List<ProfileIndexModel>)null, 0);
            }
            return DataPaging.Create(result, result[0].TotalRecord);
            
        }
       
    }
}
