using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.Model
{
    public class GhichuModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int HosoId { get; set; }
        public string Noidung {get;set;}
        public DateTime CommentTime { get; set; }
    }
}
