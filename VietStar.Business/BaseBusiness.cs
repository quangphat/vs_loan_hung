using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using VietStar.Entities.Infrastructures;

namespace VietStar.Business
{
    public abstract class BaseBusiness
    {
        public readonly CurrentProcess _process;
        protected IMapper _mapper;
        public BaseBusiness(IMapper mapper, CurrentProcess process)
        {
            _process = process;
            _mapper = mapper;
        }
        public void AddError(string errorMessage, params object[] traceKeys)
        {
            _process.AddError(errorMessage, traceKeys);
        }
        public bool CheckIsNotLogin()
        {
            if (string.IsNullOrWhiteSpace(_process.User.UserName))
            {
                //AddError(errors.error_login_expected);
                return true;
            }
            return false;
        }
    }
}
