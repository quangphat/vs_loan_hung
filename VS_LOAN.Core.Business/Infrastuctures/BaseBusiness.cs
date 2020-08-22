using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Entity.CheckDup;

namespace VS_LOAN.Core.Business.Infrastuctures
{
    public abstract class BaseBusiness
    {
        protected IMapper _mapper;
        public BaseBusiness()
        {
            var config = new MapperConfiguration(x =>
            {
                x.CreateMap<CheckDupEditModel, CheckDupAddSql>();
                x.CreateMap<CheckDupAddModel, CheckDupAddSql>();

            });
            _mapper = config.CreateMapper();
        }

        protected BaseResponse<T> ToResponse<T>(RepoResponse<T> result)
        {
            if (!result.success)
            {
                return new BaseResponse<T>(result.error, result.data, result.success);
            }
            return new BaseResponse<T>(null, result.data, true);
        }
        protected BaseResponse<T> ToResponse<T>(T data, bool success = true, string error = null)
        {
            if (!success)
            {
                return new BaseResponse<T>(error, data, success);
            }
            return new BaseResponse<T>(null, data, success);
        }
    }
}
