using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.MCreditModels
{
    public class NoteAddResponseModel:MCResponseModelBase
    {
        public int Total { get; set; }
        public List<NoteObj> objs { get; set; }
    }
}
