using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.MCreditModels
{
    public class GetFileUploadRequest:MCreditRequestModelBase
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Loccode { get; set; }
        public string Issl { get; set; }
        public string Money { get; set; }
    }
}
