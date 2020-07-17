using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.MCreditModels
{
    public class NoteAddRequestModel:MCreditRequestModelBase
    {
        public string Id { get; set; }
        public string Content { get; set; }
    }
}
