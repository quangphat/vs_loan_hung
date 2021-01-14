using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.MCreditModels.SqlModel
{
    public class MiraeDeferModel : BaseSqlEntity
    {
        public string Client_name { get; set; }
        public string Defer_code { get; set; }
        public string Defer_note { get; set; }
        public DateTime Defer_time { get; set; }

        public string Id_f1 { get; set; } 

        
        public int Id { get; set; }
    
        public string StatusDefer { get; set; }

        public MiraeDeferModel()
        {
            this.CreatedTime = this.UpdatedTime = DateTime.Now;

            
                 
        }

    }


    public class PushPundHistoryModel : BaseSqlEntity
    {
        public int Id { get; set; }
        public string Appid{ get; set; }
        public string DocCode { get; set; }


        public string Defercode { get; set; }
        public string Deferstatus { get; set; }

        public string StatusVietBank { get; set; }

        public PushPundHistoryModel()
        {
            this.CreatedTime = this.UpdatedTime = DateTime.Now;
        }

    }

}
