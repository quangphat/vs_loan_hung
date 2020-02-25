using EasyCreditService.Interfaces;
using LoanRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Business.Interfaces;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Entity.EasyCredit;
using VS_LOAN.Core.Entity.Infrastructures;
using VS_LOAN.Core.Utility;

namespace VS_LOAN.Core.Business.EasyCredit
{
    public class ECLoanBusiness : BaseBusiness, IECLoanBusiness
    {
        protected readonly IEcHosoRepository _rpEcHoso;
        public ECLoanBusiness(HttpClient httpClient,
            CurrentProcess currentProcess,
            IEcHosoRepository ecHosoRepository)
            : base(typeof(ECLoanBusiness), currentProcess, httpClient: httpClient)
        {
            _rpEcHoso = ecHosoRepository;
        }
        public async Task<int> SaveEcHosoStep1(EcHoso model)
        {
            if (model == null)
            {
                _process.Error = errors.model_null;
                return 0;
            }
            if(model.FileCount <2)
            {
                _process.Error = errors.image_selfie_missing;
                return 0;
            }
            if (string.IsNullOrWhiteSpace(model.FullName))
            {
                _process.Error = errors.customer_name_must_not_be_emty;
                return 0;
            }
            if (!Utility.Utility.IsValidPhone(model.Phone))
            {
                _process.Error = errors.phone_must_not_be_emty;
                return 0;
            }
            if (!Utility.Utility.IsValidIdentityCard(model.Cmnd))
            {
                _process.Error = errors.identity_number_must_not_be_emty;
                return 0;
            }
            if (string.IsNullOrWhiteSpace(model.IssueDate))
            {
                _process.Error = errors.identity_date_invalid;
                return 0;
            }
            if (string.IsNullOrWhiteSpace(model.BirthDay))
            {
                _process.Error = errors.customer_birthday_invalid;
                return 0;
            }
            if (!string.IsNullOrWhiteSpace(model.Email) && !Utility.Utility.IsValidEmail(model.Email))
            {
                _process.Error = "Email không hợp lệ";
                return 0;
            }
            model.Status = (int)EcHosoStatus.WatingOtp;
            model.EcResult = (int)EcHosoResult.Waiting;
            model.RequestId = ModelExtensions.GenEcRequestId();
            var id = await _rpEcHoso.Insert(model);
            if (id > 0)
            {
                await _rpEcHoso.InsertHosoRequest(id, model.RequestId);
            }
            _process.Clear();
            return id;
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
