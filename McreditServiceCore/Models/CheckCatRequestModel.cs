using System;
using System.Collections.Generic;
using System.Text;

namespace McreditServiceCore.Models
{
    public class CheckCatRequestModel : MCreditRequestModelBase
    {
        public string taxNumber { get; set; }
    }
}
