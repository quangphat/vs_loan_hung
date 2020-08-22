using System;
using System.Collections.Generic;
using System.Text;

namespace McreditServiceCore.Models
{
    public class CheckStatusRequestModel : MCreditRequestModelBase
    {
        public string IdNumber { get; set; }
    }
}
