﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.MCreditModels.SqlModel
{
    public class MCreditlocations
    {
        public string Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }
        public string Addr { get; set; }

        public DateTime UpdatedTime { get; set; }

        public string McId { get; set; }

    }
}
