using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.MCreditModels.SqlModel
{
    public class MiraeDDEEditModel:BaseSqlEntity
    {

        public int Id { get; set; }
        public string Maritalstatus { get; set; }
        public string Qualifyingyear { get; set; }

        public string Eduqualify { get; set; }
        public string Noofdependentin { get; set; }
        public string Paymentchannel { get; set; }

        public DateTime? Nationalidissuedate { get; set; }

        public string Familybooknumber { get; set; }

        public string Idissuer { get; set; }

        public string Spousename { get; set; }

        public string Spouse_id_c { get; set; }

        public string Categoryid { get; set; }

        public string Bankname { get; set; }
        public string Bankbranch { get; set; }

        public string Acctype { get; set; }



        public string Accno { get; set; }

        public string Dueday { get; set; }

        public string Notecode { get; set; }

        public string Notedetails { get; set; }

    }
}
