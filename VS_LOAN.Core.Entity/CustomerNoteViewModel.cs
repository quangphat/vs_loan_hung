using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity.Model;

namespace VS_LOAN.Core.Entity
{
    public class CustomerNoteViewModel:CustomerNote
    {
        public string Commentator { get; set; }
    }
}
