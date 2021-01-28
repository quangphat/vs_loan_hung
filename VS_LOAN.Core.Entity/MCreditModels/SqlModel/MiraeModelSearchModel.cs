using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.MCreditModels.SqlModel
{
    public class MiraeModelSearchModel : MiraeModel
    {
        public int TotalRecord { get; set; }    
        public string CreatedUser { get; set; }
        public string VbFUserUpdate { get; set; }
        public string UpdateUser { get; set; }
        public string Vbf { get; set; }
        public bool IsQDEToDDE { get; set; }
        public bool IsDDeCreate { get; set; }
        public bool IsPushDoucment { get; set; }
        public string StatusName { get; set; }
        public string Statusclient { get; set; }
        public string LastNoteVietBank { get; set; }
        public string LastDeffer { get; set; }

        public string Rejeccode { get; set; }

        public string Reason { get; set; }
        public MiraeModelSearchModel()
        {
            IsPushDoucment = false;
            IsDDeCreate = false;
            IsQDEToDDE = false;

        }
    }

}
