using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.MCreditModels.SqlModel
{
    public class MiraeEditModel :MiraeModel
    {

        public string DobStr { get; set; }

        public string NationalidissuedateStr { get; set; }
        public string SellerNote { get; set; }
        public string TotalloanamountreqText { get; set; }
        public string ReasonReject { get; set; }
        public string Email { get; set; }
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
        public MiraeEditModel()
        {

        }

    }
}
