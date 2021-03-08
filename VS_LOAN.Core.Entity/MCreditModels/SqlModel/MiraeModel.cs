using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.MCreditModels.SqlModel
{
    public class MiraeModel : BaseSqlEntity
    {
        public string Mobile { get; set; }
        public string Fixphone { get; set; }

        public string AppId { get; set; }
        public int Id { get; set; }
        public string Channel { get; set; }
        public string Schemeid { get; set; }
        public decimal? Downpayment { get; set; }
        public decimal? Totalloanamountreq { get; set; }
        public int Tenure { get; set; }
        public string Sourcechannel { get; set; }
        public string Salesofficer { get; set; }
        public string Loanpurpose { get; set; }
        public string Creditofficercode { get; set; }
        public string Bankbranchcode { get; set; }
        public string Laa_app_ins_applicable { get; set; }
        public string Possipbranch { get; set; }
        public string Priority_c { get; set; }
        public string Userid { get; set; }
        public string Fname { get; set; }
        public string Mname { get; set; }
        public string Lname { get; set; }
        public string Nationalid { get; set; }
        public string Title { get; set; }
        public string Gender { get; set; }

        public DateTime Dob { get; set; }

       




        public int Constid { get; set; }

        public string Tax_code { get; set; }

        public int Presentjobyear { get; set; }

        public int Presentjobmth { get; set; }

        public int Previousjobyear { get; set; }

        public int Previousjobmth { get; set; }

        public string Natureofbuss { get; set; }

        public string Referalgroup { get; set; }

        public string Addresstype { get; set; }

        public string Addressline { get; set; }

        public string  Phone { get; set; }

        public string Others { get; set; }

        public string Position { get; set; }

        public string Head { get; set; }


        public string Frequency { get; set; }

        public string Amount { get; set; }

        public string Accountbank { get; set; }

        public string Debit_credit { get; set; }

        public string In_per_cont { get; set; }     
        public string AddressCur_in_propertystatus { get; set; }

        public string AddressCur_address1stline { get; set; }


        public int  AddressCur_Country { get; set; }

        public int AddressCur_City { get; set; }

        public int AddressCur_District { get; set; }

        public int AddressCur_Ward { get; set; }


        public string AddressCur_roomno { get; set; }

        public string AddressCur_stayduratcuradd_y { get; set; }

        public string AddressCur_stayduratcuradd_m { get; set; }
        public string AddressCur_mailingaddress { get; set; }
        public string AddressCur_mobile { get; set; }

        public string AddressCur_landlord { get; set; }

        public string AddressCur_landmark { get; set; }

        public string Refferee1_in_title { get; set; }


        public string Refferee1_Refereename { get; set; }

        public string Refferee1_Refereerelation { get; set; }

        public string Refferee1_Phone1 { get; set; }

        public string Refferee1_Phone2 { get; set; }



        public string Refferee2_in_title { get; set; }


        public string Refferee2_Refereename { get; set; }

        public string Refferee2_Refereerelation { get; set; }

        public string Refferee2_Phone1 { get; set; }

        public string Refferee2_Phone2 { get; set; }



        public string Refferee3_in_title { get; set; }


        public string Refferee3_Refereename { get; set; }

        public string Refferee3_Refereerelation { get; set; }

        public string Refferee3_Phone1 { get; set; }

        public string Refferee3_Phone2 { get; set; }



        public string AddressPer_in_propertystatus { get; set; }

        public string AddressPer_address1stline { get; set; }


        public int AddressPer_Country { get; set; }

        public int AddressPer_City { get; set; }

        public int AddressPer_District { get; set; }

        public int AddressPer_Ward { get; set; }


        public string AddressPer_roomno { get; set; }

        public string AddressPer_stayduratPeradd_y { get; set; }

        public string AddressPer_stayduratPeradd_m { get; set; }
        public string AddressPer_mailingaddress { get; set; }
        public string AddressPer_mobile { get; set; }

        public string AddressPer_landlord { get; set; }

        public string AddressPer_landmark { get; set; }


        public int ContryCompany { get; set; }

        public int CityCompany { get; set; }

        public int DistrictCompany { get; set; }

        public int WardCompany { get; set; }

        public bool? IsDuplicateAdrees { get; set; }




        public string Maritalstatus { get; set; }
        public string Qualifyingyear { get; set; }

        public string Eduqualify { get; set; }
        public string Noofdependentin { get; set; }
        public string Paymentchannel { get; set; }

        public DateTime? Nationalidissuedate { get; set; }

        public string Familybooknumber { get; set; }

        public string Idissuer { get; set; }

        public string Spousename { get; set; }

        public string Spouse_id_c { get; set; }

        public string Categoryid { get; set; }

        public string Bankname { get; set; }
        public string Bankbranch { get; set; }

        public string Acctype { get; set; }
        public string Accno { get; set; }
        public string Dueday { get; set; }
        public string Notecode { get; set; }
        public string Notedetails { get; set; }


        public string PrivateInfo { get; set; }

        public string PrivateInfoOther { get; set; }

        public string NotedDetailPrivate { get; set; }

        public string Spouse_phoneNumber { get; set; }
        public string Spouse_companyName { get; set; }
        public string Spouse_addressName { get; set; }





        public int Status { get; set; }



        public MiraeModel()
        {
            this.AddressCur_Country = 189;
            this.AddressPer_Country = 189;
            this.Sourcechannel = "ADVT";
            this.ContryCompany = 189;
            this.Status = 0;
        }

    }



    public class ClientUpdateStatusRequest
    {
        public int AppId { get; set; }
        public string Status { get; set; }

        public DateTime BussinessTime { get; set; }








        public string Rejeccode { get; set; }

        public string Reason { get; set; }

    }

}
