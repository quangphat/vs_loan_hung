﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.MCreditModels
{
    public class CheckCICRequestModel : MCreditRequestModelBase
    {
        public string IdNumber { get; set; }
        public string name { get; set; }
    }
}
