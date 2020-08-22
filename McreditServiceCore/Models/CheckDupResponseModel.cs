using System;
using System.Collections.Generic;
using System.Text;

namespace McreditServiceCore.Models
{
    public class CheckDupResponseModel : MCResponseModelBase
    {

        public string result { get; set; }
        public string desc { get; set; }
    }
}
