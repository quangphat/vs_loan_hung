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
    public class CheckDupBusiness : BaseBusiness, ICheckDupBusiness
    {
        protected readonly ICheckDupRepository _rpCheckDup;
        public CheckDupBusiness(ICheckDupRepository checkdupRepository,
            IMapper mapper, CurrentProcess process) : base(mapper, process)
        {
            _rpCheckDup = checkdupRepository;
        }

       
    }
}
