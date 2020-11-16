using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.MCreditModels
{
    public class OcbLeadResponseModel
    {

        public string Status { get; set; }


        public string Message { get; set; }

        public string ErrorCode { get; set; }
        
        public OcbLeadResponseData Data
        { get; set; }

    }

    public class OcbSendfileReponseModel
    {

        public string Status { get; set; }


        public string Message { get; set; }

    }

    public class OcbSendfileReQuestModel
    {

        public string CustomerId { get; set; }
        public string Fieldname { get; set; }
        public string FileContent { get; set; }
        public string FileType { get; set; }
    }



    public class OcbLeadResponseData
    {
        public string CustomerId { get; set; }
    }
    public class DictionaryResponseModel
    {

        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public string ResponseData { get; set; }
        public List<DictionaryResponseItem> listDictionary { get; set; }
    }

    public class DictionaryResponseItem
    {

        public string GroupId { get; set; }
        public string Code { get; set; }
        public string DisplayName { get; set; }
    }

    public class ProvinceResponseModel
    {

        public int ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
      
        public string ResponseData { get; set; }

        public List<ProvinceItem> listProvinceItem { get; set; }

        public ProvinceResponseModel()
        {
            listProvinceItem = new List<ProvinceItem>();
        }

    }


    public class AuthenResponseModel
    {

        public string Access_token { get; set; }
        public string Token_type { get; set; }
        public DateTime? ValidTo { get; set; }
        public string Expires_in { get; set; }
        public string UserName { get; set; }

    }






    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 




    public class ProvinceItem
    {
        public string ProvinceId { get; set; }
        public string ProvinceName { get; set; }

        public string ProvinceType
        {
            get; set;
        }
    } 
        public class ALlCityResponseModel
        {

            public string ResponseCode { get; set; }
            public string ResponseMessage { get; set; }
            public string  ResponseData { get; set; }

            public List<CityItem> listCity { get; set; }

        }

        public class CityItem
        {
        public string CityId { get; set; }
        public string CityName { get; set; }
        public string ProvinceId { get; set; }
        public string CityType { get; set; }

        }



        public class CityRequestModel
        {

            public string ProvinceId { get; set; }


        }


        public class WardResponseModel
        {
            public string ResponseCode { get; set; }
            public string ResponseMessage { get; set; }
            public string ResponseData { get; set; }

            public List<WardResponseItem> listWardResponse { get; set; }

            

            //public object DigitalSignature { get; set; }
    }

            
        public class WardResponseItem
        {
        public string WardId { get; set; }
        public string WardName { get; set; }
        public string CityId { get; set; }
        public string WardType { get; set; }
        }
        public class WardRequestModel
        {
        public string CityId { get; set; }

        }


    public class CreateLeadRequest
    {
        public string TraceCode { get; set; }

        public string FullName { get; set; }

        public int? Gender { get; set; }

        public string IdCard { get; set; }

        public DateTime? IdIssueDate { get; set; }

        public string IdIssuePlaceId { get; set; }

        public DateTime Birthday { get; set; }
        public string Mobilephone { get; set; }
        public string RegAddressWardId { get; set; }

        public string RegAddressDistId { get; set; }

        public string RegAddressProvinceId { get; set; } 

        public string CurAddressWardId { get; set; }

        public string CurAddressDistId { get; set; }

        public string CurAddressProvinceId { get; set; }

        public decimal Income { get; set; }

        public decimal RequestLoanAmount { get; set; }

        public int RequestLoanTerm { get; set; }

        public string ProductId { get; set; }

        public string SellerNote { get; set; }

        public string MetaData { get; set; }


        public string ReferenceFullName1 { get; set; }
        public string ReferenceRelationship1 { get; set; }
        public string ReferencePhone1 { get; set; }
        public int? Reference1Gender { get; set; }


        public string ReferenceFullName2 { get; set; }
        public string ReferenceRelationship2 { get; set; }
        public string ReferencePhone2 { get; set; }
        public int? Reference2Gender { get; set; }

        public string ReferenceFullName3 { get; set; }
        public string ReferenceRelationship3 { get; set; }
        public string ReferencePhone3 { get; set; }
        public int? Reference3Gender { get; set; }


    }


    public class MetaData
    {

        public string RegAddressNumber { get; set; }
        public string RegAddressStreet { get; set; }
        public string RegAddressRegion { get; set; }
        public string CurAddressNumber { get; set; }

        public string CurAddressStreet { get; set; }

        public string CurAddressRegion { get; set; }
        public string IncomeType { get; set; }

        public string Email { get; set; }

    }
    
}



