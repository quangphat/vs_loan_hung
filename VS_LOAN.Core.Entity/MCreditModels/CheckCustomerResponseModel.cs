﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.MCreditModels
{
    
    public class CheckCustomerRequestModel
    {

        public string searchVal { get; set; }

        public string partner { get; set; }
        public CheckCustomerRequestModel()
        {
            partner = "DIGI";
        }
    }
    public class CheckCustomerResponseModel
    {

        public bool Success { get; set; }

        public string Messsage { get; set; }
        public CheckCustomerDataReponseModel Data { get; set; }

    }

    public class CheckCustomerDataReponseModel
    {

        public string Id { get; set; }
        public string Phone { get; set; }
        public string Partner { get; set; }
        public string StatusNumber { get; set; }

    }

    public class MiraeCityRequest
    {

       public string msgName { get; set; }

      
    }
    public class MiraeCityResponseModel
    {

        public string Message { get; set; }
        public bool Success { get; set; }

        public List<MiraeCityItem> Data { get; set; }
    }
    public class MiraeCityItem
    {

        public string Stateid { get; set; }

        public string Statedesc { get; set; }

        public string Countryid { get; set; }
    }



    public class MiraeDistrictItemResponseModel
    {

        public string Message { get; set; }
        public bool Success { get; set; }

        public List<MiraeDistrictItem> Data { get; set; }
    }
    public class MiraeAllWardResponseModel
    {

        public string Message { get; set; }
        public bool Success { get; set; }

        public List<MiraeAllWardItem> Data { get; set; }
    }


    public class MiraeAllWardItem
    {

        public string Zipdesc { get; set; }

        public string City { get; set; }

        public string Zipcode { get; set; }
    }





    public class MiraeDistrictItem
    {

        public string Lmc_CITYNAME_C { get; set; }

        public string Lmc_CITYID_C { get; set; }

        public string Lmc_STATE_N { get; set; }
    }




    public class SchemeRequestModel
    {
        
        public string msgName { get; set; }
    }


    public class SchemeReponseModel
    {

        public bool Success { get; set; }

        public string Message { get; set; }
        
        public List<SchemeReponseItem> Data { get; set; }

        public SchemeReponseModel()
        {
            Data = new List<SchemeReponseItem>();
        }


    }

    public class SaleOfficeRequestModel
    {

        public string msgName { get; set; }
    }


    public class SaleOfficeReponseModel
    {

        public bool Success { get; set; }

        public string Message { get; set; }

        public List<SaleOfficeReponseItem> Data { get; set; }

        public SaleOfficeReponseModel()
        {
            Data = new List<SaleOfficeReponseItem>();
        }
    }

    public class BankRequestModel
    {

        public string msgName { get; set; }
        public BankRequestModel()
        {
            msgName = "getBank";
        }
    }


    public class BankReponseModel
    {

        public bool Success { get; set; }

        public string Message { get; set; }

        public List<BankItem> Data { get; set; }

        public BankReponseModel()
        {
            Data = new List<BankItem>();
        }


    }

    public class BankItem
    {
        public int Bankid  { get;set;}

        public string Bankdesc { get; set; }

    }

    public class SaleOfficeReponseItem
    {
        public int Inspectorid { get; set; }

        public string Inspectorname { get; set; }
    }
    public class SchemeReponseItem
    {

        public int Schemeid { get; set; }

        public string Schemename { get; set; }

        public string Schemegroup { get; set; }

        public string Product { get; set; }

        public double? Maxamtfin { get; set; }

        public double? Minamtfin { get; set; }

        public int Maxtenure { get; set; }

        public int Mintenure { get; set; } 



    }

    public class SelectUserRequestModel
    {

        public string msgName { get; set; }
    }

    public class SelectUserReponseModel
    {

        public bool Success { get; set; }

        public string Message { get; set; }

        public List<SelectItem> Data { get; set; }

        public SelectUserReponseModel()
        {
            Data = new List<SelectItem>();
        }


    }

    public class SelectItem
    {
        public string Lsu_USER_NAME_C { get; set; }

        public string Lsu_USER_ID_C { get; set; }

    }



    public class PushToUNDReponse 

    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public string Data { get; set; }

        public PushToUNDReponse()
        {
            Success = false;
        }

    }


    public class PushToHistoryReponse

    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public string Data { get; set; }

        public PushToHistoryReponse()
        {
            Success = false;
        }

    }

    public class PushToHistoryRequest

    {

        public int AppId { get; set; }

        public PushToHistoryRequest()
        {
           
        }

    }
    public class TaiLieuMirateModel
    {

        public int Id { get; set; }

        public string FileKey { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }

       
    }


}
