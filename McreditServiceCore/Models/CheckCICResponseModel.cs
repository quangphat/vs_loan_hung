using System;
using System.Collections.Generic;
using System.Text;

namespace McreditServiceCore.Models
{
    public class CheckCICResponseModel : MCResponseModelBase
    {
        public string Result { get; set; }
        public string Relate { get; set; }
    }
}
