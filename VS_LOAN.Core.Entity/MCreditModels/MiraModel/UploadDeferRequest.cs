using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.MCreditModels.SqlModel
{
    public class UploadDeferRequest
    {
        public string id_f1 { get; set; }

        public string client_name { get; set; }
        public string defer_code { get; set; }

        public string defer_note { get; set; }

        public DateTime defer_time { get; set; }

    }

    public class UploadDeferReponseItem
    {

        public string client_name { get; set; }
        public string defer_code { get; set; }

        public string defer_note { get; set; }

        public DateTime defer_time { get; set; }

        public string id_f1 { get; set; }

    }

    public class UploadDeferReponse
    {

        public string status { get; set; }

        public UploadDeferReponseItem data { get; set; }

        public string message { get; set; }
    }


    public class UploadStatusRequest
    {
        public string id_f1 { get; set; }
        public string f1_no { get; set; }
        public string client_name { get; set; }
        public string status_f1 { get; set; }
        public string f1_time { get; set; }
        public string reject_code { get; set; }
        public string rejected_code { get; set; }
        public string reason { get; set; }
    }

    public class UploadStatusReponseItem
    {
        public string id_f1 { get; set; }

        public string client_name { get; set; }
        public string status_f1 { get; set; }
        public string f1_time { get; set; }

        public string f1_no { get; set; }

    }

    public class UploadStatusReponse
    {

        public string status { get; set; }

        public UploadStatusReponseItem data { get; set; }

        public string message { get; set; }
    }


    public class PushToUndRequest
    {

        public int appid { get; set; }

   
    }

}
