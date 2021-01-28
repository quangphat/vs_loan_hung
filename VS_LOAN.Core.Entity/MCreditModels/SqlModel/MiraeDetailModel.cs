using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.MCreditModels.SqlModel
{
    public class MiraeDetailModel : MiraeModel
    {
        public bool? IsQDEToDDE { get; set; }
        public bool? IsDDeCreate { get; set; }


        public bool? IsQDEToPor { get; set; }

        public bool? IsPushDoucment { get; set; }
    }


}
