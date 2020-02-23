using EasyCreditService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Business.Interfaces;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Entity.EasyCredit;
using VS_LOAN.Core.Entity.EasyCredit.PostModel;
using VS_LOAN.Core.Entity.Infrastructures;
using VS_LOAN.Core.Utility;

namespace VS_LOAN.Core.Business.EasyCredit
{
    public class ECLoanBusiness : BaseBusiness, IECLoanBusiness
    {
        public ECLoanBusiness(HttpClient httpClient, CurrentProcess currentProcess)
            : base(typeof(ECLoanBusiness),currentProcess, httpClient: httpClient)
        {
        }
        public async Task<bool> SaveEcHosoStep1(EcHosoPostModel model)
        {
            if(model==null)
            {
                _process.Error = errors.model_null;
                return false;
            }
            var customer = AutoMapper.Mapper.Map<CustomerModel>(model);
            return true;
        }
        public async Task<bool> SaveEcHoso(EcHoso model)
        {
            if (model == null)
            {
                _process.Error = "Dữ liệu không hợp lệ";
                return false;
            }
            return true;
        }
        public async Task<EcResponseModel<bool>> UploadFile(StringModel model)
        {
            var x = _process.UserName;
            var result = await _httpClient.Post<EcResponseModel<bool>>(EcApiPath.BasePathDev, "/api/ECCredit/sftp", model);
            return result.Data;
        }
        public async Task<EcResponseModel<EcDataResponse>> CreateLoanToEc(LoanInfoRequestModel model)
        {
            if (model == null)
                return null;
            var result = await _httpClient.Post<EcResponseModel<EcDataResponse>>(EcApiPath.BasePathDev, EcApiPath.Step2, model);
            if (result != null)
                return result.Data;
            return null;
        }

    }
}
