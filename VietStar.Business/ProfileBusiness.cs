using System;
using System.Collections.Generic;
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

       
    }
}
