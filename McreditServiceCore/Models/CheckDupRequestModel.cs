using System;
using System.Collections.Generic;
using System.Text;

namespace McreditServiceCore.Models
{
    public class CheckDupRequestModel : MCreditRequestModelBase
    {
        public string IdNumber { get; set; }
    }
}
