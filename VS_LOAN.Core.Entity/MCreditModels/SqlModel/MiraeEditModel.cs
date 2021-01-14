using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.MCreditModels.SqlModel
{
    public class MiraeEditModel
    {

        public int Id { get; set; }
        public string Channel { get; set; }
        public string Schemeid { get; set; }

        public decimal? Downpayment { get; set; }
        public decimal? Totalloanamountreq { get; set; }
        public string TotalloanamountreqText { get; set; }

        public int Tenure { get; set; }

        public string Sourcechannel { get; set; }

        public string Salesofficer { get; set; }

        public string Loanpurpose { get; set; }

        public string Creditofficercode { get; set; }

        public string Bankbranchcode { get; set; }

        public string Laa_app_ins_applicable { get; set; }
        public string Possipbranch { get; set; }

        public string ReasonReject { get; set; }



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


        public int ContryCompany { get; set; }

        public int CityCompany { get; set; }

        public int DistrictCompany { get; set; }

        public int WardCompany { get; set; }

        public string Phone { get; set; }


        public string Email { get; set; }

        public string Others { get; set; }

        public string Position { get; set; }

        public string Head { get; set; }


        public string Frequency { get; set; }

        public string Amount { get; set; }

        public string Accountbank { get; set; }

        public string Debit_credit { get; set; }

        public string Per_cont { get; set; }

        public string MsgName { get; set; }




        public string PhoneCompany { get; set; }

        public int DdlTinhCur { get; set; }


        public int DdlHuyenCur { get; set; }

        public int DdlRewardCur { get; set; }

        public string Stayduratcuradd_yCur { get; set; }
        public string Stayduratcuradd_mCur { get; set; }

        public string MobileCur { get; set; }

        public string PropertystatusCur { get; set; }


        public string LandlordCur { get; set; }
        public string LandmarkCur { get; set; }
        public string RoomnoCur { get; set; }



        public bool? IsDuplicateAdrees { get; set; }

        public string Address1stlineCur { get; set; }
        public int DdlTinhPer { get; set; }
        public int DdlHuyenPer { get; set; }
        public int DdlRewardPer { get; set; }
        public string StayduratPeradd_yPer { get; set; }
        public string StayduratPeradd_mPer { get; set; }
        public string MobilePer { get; set; }
        public string PropertystatusPer { get; set; }
        public string LandlordPer { get; set; }
        public string RoomnoPer { get; set; }
        public string Landmarkper { get; set; }
        public string Address1stlinePer { get; set; }



        public string Refferee3_in_title { get; set; }


        public string Refferee3_Refereename { get; set; }

        public string Refferee3_Refereerelation { get; set; }

        public string Refferee3_Phone1 { get; set; }

        public string Refferee3_Phone2 { get; set; }






        public string Refferee2_in_title { get; set; }


        public string Refferee2_Refereename { get; set; }

        public string Refferee2_Refereerelation { get; set; }

        public string Refferee2_Phone1 { get; set; }

        public string Refferee2_Phone2 { get; set; }

        public string Refferee1_in_title { get; set; }

        public string Refferee1_Refereename { get; set; }

        public string Refferee1_Refereerelation { get; set; }

        public string Refferee1_Phone1 { get; set; }

        public string Refferee1_Phone2 { get; set; }
        public string Fixphone { get; set; }
        public string Mobile { get; set; }
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


        public MiraeEditModel()
        {
          


        }



    }
}
