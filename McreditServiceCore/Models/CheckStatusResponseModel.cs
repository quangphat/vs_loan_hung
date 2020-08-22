﻿using System;
using System.Collections.Generic;
using System.Text;

namespace McreditServiceCore.Models
{
    public class CheckStatusResponseModel : MCResponseModelBase
    {
        public List<CheckStatusResponseObject> Objs { get; set; }
    }
    public class CheckStatusResponseObject
    {
        public string CaseNumber { get; set; }
        public string CustName { get; set; }
        public string Status { get; set; }
    }
}
