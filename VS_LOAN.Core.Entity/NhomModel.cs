﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity
{
    public class NhomModel
    {
        public int ID { get; set; }
        public string Ten { get; set; }
        public string TenNgan { get; set; }
        public int MaNhomCha { get; set; }
        public int MaNguoiQL { get; set; }

        public string ChuoiMaCha { get; set; }
        public int CreatedBy { get; set; }
    }
}
