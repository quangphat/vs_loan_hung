using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using VietStar.Business.Interfaces;
using VietStar.Entities.Commons;
using VietStar.Entities.Infrastructures;
using static VietStar.Entities.Commons.Enums;

namespace VietStar.Business
{
    public class CommonBusiness : BaseBusiness, ICommonBusiness
    {
        public CommonBusiness(IMapper mapper, CurrentProcess process) : base(mapper, process)
        {
        }

        public Task<List<OptionSimple>> GetStatusList()
        {
            var result = new List<OptionSimple>() {
                new OptionSimple{Id =1, Name = "Mới" },

                new OptionSimple{ Id = 2,Name = "Nhập liệu" },
                 new OptionSimple{ Id = 3,Name = "Thẩm định" },
                 new OptionSimple{ Id = 4,Name = "Từ chối" },
                
            };
            return Task.FromResult(result);
        }
    }
}
