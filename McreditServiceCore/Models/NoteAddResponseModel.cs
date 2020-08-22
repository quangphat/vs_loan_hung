using System;
using System.Collections.Generic;
using System.Text;

namespace McreditServiceCore.Models
{
    public class NoteAddResponseModel : MCResponseModelBase
    {
        public int Total { get; set; }
        public List<NoteObj> objs { get; set; }
    }
}
