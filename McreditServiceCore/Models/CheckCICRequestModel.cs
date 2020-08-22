using System;
using System.Collections.Generic;
using System.Text;

namespace McreditServiceCore.Models
{
    public class CheckCICRequestModel : MCreditRequestModelBase
    {
        public string IdNumber { get; set; }
        public string name { get; set; }
    }
}
