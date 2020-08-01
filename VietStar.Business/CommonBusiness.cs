using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using VietStar.Business.Interfaces;
using VietStar.Entities.Commons;
using VietStar.Entities.Infrastructures;
using VietStar.Repository.Interfaces;
using static VietStar.Entities.Commons.Enums;

namespace VietStar.Business
{
    public class CommonBusiness : BaseBusiness, ICommonBusiness
    {
        protected readonly ICommonRepository _rpCommon;
        public CommonBusiness(ICommonRepository commonRepository,IMapper mapper, CurrentProcess process) : base(mapper, process)
        {
            _rpCommon = commonRepository;
        }

        public async Task<List<OptionSimple>> GetStatusList(string profileType)
        {
            var result = await _rpCommon.GetProfileStatusByRoleCode(profileType, _process.User.OrgId, _process.User.Rolecode);
            return result;
        }
    }
}
