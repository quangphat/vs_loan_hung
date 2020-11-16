using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity.MCreditModels;
using VS_LOAN.Core.Entity.MCreditModels.SqlModel;

namespace MCreditService.Interfaces
{
    public interface IOdcService
    {
      
        Task<CheckVavlidOdcResponseModel> CheckValidData(string mobilePhone, string idNo);
        Task<OcbLeadResponseModel> CreateLead(OcbProfile model);

        Task<OcbSendfileReponseModel> SendFile(OcbSendfileReQuestModel model);
        Task<DictionaryResponseModel> GetAllDictionary();

        Task<ProvinceResponseModel> GetAllProvince();

        Task<ALlCityResponseModel> GetAllCity(CityRequestModel model);
        Task<WardResponseModel> GetAllWard(WardRequestModel model);

        Task<bool> Authen();

        Task<bool> CheckAuthen();


    }
}
