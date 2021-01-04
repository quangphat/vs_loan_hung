using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.MCreditModels.SqlModel
{
    public class MiraeDeferSearchModel : BaseSqlEntity
    {

        public int TotalRecord { get; set; }
        
        public string Client_name { get; set; }



        public string Defer_code { get; set; }
        public string Defer_note { get; set; }
        public DateTime Defer_time { get; set; }

        public string Id_f1 { get; set; }
        public int Id { get; set; }
        public string DeferName { get; set; }
        public string StatusName { get; set; }

        
        public MiraeDeferSearchModel()
        {
            this.CreatedTime = this.UpdatedTime = DateTime.Now;

            this.StatusName = "Chữa xác định";

            this.DeferName = "Chữa xác định";
        }

    }

}
