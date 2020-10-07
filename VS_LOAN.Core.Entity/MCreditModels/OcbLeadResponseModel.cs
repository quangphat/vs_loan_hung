using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.MCreditModels
{
    public class OcbLeadResponseModel
    {

        public string status { get; set; }


        public string messsage { get; set; }

        public object data { get; set; }

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
    
}



