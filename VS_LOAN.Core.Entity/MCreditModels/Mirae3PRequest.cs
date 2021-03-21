using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.MCreditModels
{


    public class Mirae3PRequest : MiraeQDELeadReQuest
    {
      
        public string in_appid { get; set; }
  

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

       

        public Mirae3PRequest  () :base()
        {
            msgName = "inputDDE";
            in_channel = "VFS";
            in_userid = "EXT_VFS";

        }


    }
   
    public class Mirae3PReponse
    {

        public bool Success { get; set; }
        public string Message { get; set; }

        public object Data { get; set; }

        public Mirae3PReponse()
        {
            Success = false;
        }

    }

   
}



