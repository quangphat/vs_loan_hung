using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Entity.EasyCredit;
using VS_LOAN.Core.Entity.EasyCredit.PostModel;

namespace VS_LOAN.Core.Business.Interfaces
{
    public interface IECLoanBusiness
    {
        Task<bool> SaveEcHosoStep1(EcHosoPostModel model);
        Task<bool> SaveEcHoso(EcHoso model);
        Task<EcResponseModel<bool>> UploadFile(StringModel model);
        Task<EcResponseModel<EcDataResponse>> CreateLoanToEc(LoanInfoRequestModel model);
    }
}
