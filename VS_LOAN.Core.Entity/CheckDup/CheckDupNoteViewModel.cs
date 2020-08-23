using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity.Model;

namespace VS_LOAN.Core.Entity.CheckDup
{
    public class CheckDupNote : SqlBaseModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string Note { get; set; }
    }
    public class CheckDupNoteViewModel : CheckDupNote
    {
        public string Commentator { get; set; }
    }
}
