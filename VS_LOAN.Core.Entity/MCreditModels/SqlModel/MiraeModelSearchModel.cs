using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.MCreditModels.SqlModel
{
    public class MiraeModelSearchModel : BaseSqlEntity
    {
        public int TotalRecord { get; set; }

        public string Appid { get; set; }
        public int Id { get; set; }
        public string Channel { get; set; }
        public string Schemeid { get; set; }

        public decimal? Downpayment        { get; set; }
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

        public DateTime? Dob { get; set; }

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

        public int Country { get; set; }

        public int City { get; set; }

        public int District { get; set; }

        public int Ward { get; set; }

        public string  Phone { get; set; }

        public string Others { get; set; }

        public string Position { get; set; }

        public string Head { get; set; }


        public string Frequency { get; set; }

        public string Amount { get; set; }

        public string Accountbank { get; set; }

        public string Debit_credit { get; set; }

        public string Per_cont { get; set; }

        public string MsgName { get; set; }

        public string Reference1title { get; set; }

        public string Reference1refereename { get; set; }
        public string Reference1refereerelation { get; set; }

        public string Reference1phone_1 { get; set; }

        public string Reference1phone_2 { get; set; }


        public string Reference2title { get; set; }

        public string Reference2refereename { get; set; }
        public string Reference2refereerelation { get; set; }

        public string Reference2phone_1 { get; set; }

        public string Reference2phone_2 { get; set; }

        public string Address1addresstype { get; set; }

        public string Address1propertystatus { get; set; }
        public string Address1address1stline { get; set; }

        public int Address1country { get; set; }
        public int Address1city { get; set; }
        public int Address1district { get; set; }
        public int Address1ward { get; set; }
        public string Address1roomno { get; set; }
        public int Address1stayduratcuradd_y { get; set; }
        public int Address1stayduratcuradd_m { get; set; }
        public string Address1mailingaddress { get; set; }
        public string Address1mobile { get; set; }
        public string Address1landlord { get; set; }
        public string Address1landmark { get; set; }

        public string Address2addresstype { get; set; }
        public string Address2propertystatus { get; set; }
        public string Address2Address2stline { get; set; }

        public int Address2country { get; set; }
        public int Address2city { get; set; }
        public int Address2district { get; set; }
        public int Address2ward { get; set; }
        public string Address2roomno { get; set; }
        public int Address2stayduratcuradd_y { get; set; }
        public int Address2stayduratcuradd_m { get; set; }
        public string Address2mailingaddress { get; set; }
        public string Address2mobile { get; set; }
        public string Address2landlord { get; set; }
        public string Address2landmark { get; set; }

    }

}
