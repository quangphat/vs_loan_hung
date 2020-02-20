using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity.EasyCredit;

namespace VietBankApi.Business.Interfaces
{
    public interface IMediaBusiness
    {
        Task<EcResponseModel<bool>> UploadSFtp(string fileName);
    }
}
