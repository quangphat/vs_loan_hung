using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.MCreditModels
{

     public class MiraeQDELeadReQuest
    {
        public string in_channel { get; set; }
        public int in_schemeid { get; set; }
        public decimal? in_downpayment { get; set; }
        public decimal? in_totalloanamountreq { get; set; }

        public string in_sourcechannel { get; set; }
        public int in_tenure { get; set; }
        public int in_salesofficer { get; set; }
        public string in_loanpurpose { get; set; }
        public string in_creditofficercode { get; set; }
        public string in_bankbranchcode { get; set; }
        public string in_laa_app_ins_applicable { get; set; }
        public string in_possipbranch { get; set; }
        public string in_priority_c { get; set; }
        public string in_userid { get; set; }
        public string in_fname { get; set; }
        public string in_mname { get; set; }

        public string in_lname { get; set; }
        public string in_nationalid { get; set; }
        public string in_title { get; set; }
        public string in_gender { get; set; }
        public string in_dob { get; set; }
        public int in_constid { get; set; }
        public List<AddressItem> address { get; set; }
        public string in_tax_code { get; set; }
        public int in_presentjobyear { get; set; }

        public int in_presentjobmth { get; set; }

        public int in_previousjobyear { get; set; }

        public int in_previousjobmth { get; set; }

        public string in_natureofbuss { get; set; }

        public string in_referalgroup { get; set; }

        public string in_addresstype { get; set; }

        public string in_addressline { get; set; }

        public int in_country { get; set; }

        public int in_city { get; set; }
        public int in_district { get; set; }

        public int in_ward { get; set; }

        public string in_phone { get; set; }

        public string in_others { get; set; }

        public string in_position { get; set; }

        

        public List<ReferenceItem> reference { get; set; }
        public string msgName { get; set; }

        public string in_head { get; set; }

        public string in_frequency { get; set; }

        public string in_amount { get; set; }

        public string in_accountbank { get; set; }

        public string in_debit_credit { get; set; }
        public string in_per_cont { get; set; }

        public string in_mobile { get; set; }
        public string in_fixphone { get; set; }
        public MiraeQDELeadReQuest()
        {
       
            in_frequency = "MONTHLY";
            in_head = "NETINCOM";
            in_accountbank = "Y";
            in_debit_credit = "P";
            in_per_cont = "in_per_cont";

        }


      

    }

    
    public class ReferenceItem
    {
        public string in_title { get; set; }
        public string in_refereename { get; set; }
        public string in_refereerelation { get; set; }
        public string in_phone_1 { get; set; }
        public string in_phone_2 { get; set; }

      

    }
    public  class AddressItem
    {

        public string in_addresstype { get; set; }

        public string in_propertystatus { get; set; }
        public string in_address1stline { get; set; }

        public int in_country { get; set; }

        public int in_city { get; set; }
        public int in_district { get; set; }
        public int in_ward { get; set; }

        public string in_roomno { get; set; }

        public int in_stayduratcuradd_y { get; set; }

        public int in_stayduratcuradd_m { get; set; }

        public string in_mailingaddress { get; set; }

        public string in_mobile { get; set; }

        public string in_landlord { get; set; }

        public string in_landmark { get; set; }

        public string in_fixphone { get; set; }




        public AddressItem()
        {
        
            this.in_country = 189;
           
        }
    }

    public class MiraeQDELeadRePonse
    {

        public bool Success { get; set; }

        public string Message { get; set; }

        public string Data { get; set; }

        public string MsgName { get; set; }
        public string TrNo { get; set; }
    }


    public class QDEToDDEReQuest
    {

        public int p_appid { get; set; }

        public string in_userid { get; set; }

        public string in_channel { get; set; }

        public string msgName { get; set; }


        public QDEToDDEReQuest()
        {
            msgName = "procQDEChangeState";
            
            in_channel = "SBK";
            in_userid = "EXT_SBK";

     
        }

    }


   
    public class DDEToPORReQuest
    {

        public int p_appid { get; set; }

        public string in_userid { get; set; }

        public string in_channel { get; set; }

        public string msgName { get; set; }


        public DDEToPORReQuest()
        {
            msgName = "procDDEChangeState";

            in_channel = "SBK";
            in_userid = "EXT_SBK";
        }

    }
    
        public class DDEToPOReponse
    {

        public bool Success { get; set; }

        public string Message { get; set; }

        public string Data { get; set; }

        public string MsgName { get; set; }

        public string TrNo { get; set; }
        public DDEToPOReponse()
        {
            Success = false;

        }

    }

    public class QDEToDDERePonse
    {

        public bool Success { get; set; }

        public string Message { get; set; }

        public string Data { get; set; }

        public string MsgName { get; set; }

        public string TrNo { get; set; }
        public QDEToDDERePonse()
        {
            Success = false;

        }

    }

    public class MiraeDDELeadRePonse
    {
       public bool Success { get; set; }
        public string Message { get; set; }

        public string Data { get; set; }

        public string TrNo { get; set; }

        public MiraeDDELeadRePonse()
        {
            Success = true;

        }

    }

    public class MiraeDDELeadReQuest
    {
        public int in_appid { get; set; }

        public string in_userid { get; set; }

        public string in_channel { get; set; }

        public string in_maritalstatus { get; set; }
        public string in_qualifyingyear { get; set; }

        public string in_eduqualify { get; set; }
        public string in_noofdependentin { get; set; }
        public string in_paymentchannel { get; set; }

        public string in_nationalidissuedate { get; set; }

        public string in_familybooknumber { get; set; }

        public string in_idissuer { get; set; }

        public string in_spousename { get; set; }

        public string in_spouse_id_c { get; set; }

        public string in_categoryid { get; set; }

        public string in_bankname { get; set; }
        public string in_bankbranch { get; set; }

        public string in_acctype { get; set; }



        public string in_accno { get; set; }

        public string in_dueday { get; set; }

        public string in_notecode { get; set; }

        public string in_notedetails { get; set; }

        public string msgName { get; set; }

        public MiraeDDELeadReQuest()
        {
            msgName = "inputDDE";
            in_channel = "VFS";        
           in_userid = "EXT_VFS";

        }

    }
   
}



