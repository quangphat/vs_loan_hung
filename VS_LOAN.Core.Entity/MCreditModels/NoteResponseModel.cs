using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.MCreditModels
{
    public class NoteResponseModel:MCResponseModelBase
    {
        public int Total { get; set; }
        public List<NoteObj> objs { get; set; }

    }
    public class NoteObj
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string CreateUserName { get; set; }
        public string CreateUserAvatar { get; set; }
        public string CreateDated { get; set; }
    }
}
