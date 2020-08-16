using System;
using System.Collections.Generic;
using System.Text;
using VietStar.Entities.Commons;

namespace VietStar.Entities.CheckDup
{
    public class CheckDupEditModel
    {
        public CheckDupAddModel CheckDup { get; set; }
        public List<OptionSimple> Partners { get; set; }
    }
}
