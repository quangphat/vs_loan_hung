using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity.MCreditModels;
using VS_LOAN.Core.Entity.MCreditModels.SqlModel;

namespace MCreditService.Interfaces
{

    public interface IMiraeService
    {
        Task<CheckCustomerResponseModel> CheckCustomer(string searchVal, string partner);
        Task<GetpollingS37ResponseModel> GetpollingS37(GetpollingS37RequestModel request);

        Task<CheckSubmitS37ResponseModel> CheckSubmitS37( string idValue);
        Task<bool> Authen();
        Task<bool> CheckAuthen();

        Task<QDEToDDERePonse> QDEToDDE(QDEToDDEReQuest model);
        Task<DDEToPOReponse> DDEToPoR(DDEToPORReQuest model);

        Task<MiraeQDELeadRePonse> QDESubmit(MiraeQDELeadReQuest model);
        Task<MiraeDDELeadRePonse> DDESubmit(MiraeDDELeadReQuest model);
        Task<MiraeCityResponseModel> GetAllCity(MiraeCityRequest model);
        Task<MiraeCityResponseModel> GetAllProvince(MiraeCityRequest model);
        Task<MiraeDistrictItemResponseModel> GetDistrict(MiraeCityRequest model);
        Task<MiraeAllWardResponseModel> GetAllWard(MiraeCityRequest model);
        Task<SchemeReponseModel> GetAllProduct(SchemeRequestModel model);

        Task<SelectUserReponseModel> GetAllUser(SelectUserRequestModel model);


        Task<SaleOfficeReponseModel> GetAllSaleOffice(SaleOfficeRequestModel model);

        Task<BankReponseModel> GetAllBank(BankRequestModel model);

        Task<PushToUNDReponse> PushToUND(MultipartFormDataContent content);
        
        Task<PushToHistoryReponse> PushToPendHistory(MultipartFormDataContent request);


        Task<Mirae3PReponse> Update3p(Mirae3PRequest model);


    }
}
