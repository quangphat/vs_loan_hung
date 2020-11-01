using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.MCreditModels
{
    public class OcbStatusImportModel
    {
        public DateTime? ImportDate { get; set; }
        public DateTime? FirstCallDate { get; set; }
        public DateTime? LastCallDate { get; set; }
        public int? UpdatedBy { get; set; }
        public string MonthImport { get; set; }
        public string FirstCallStatus { get; set; }
        public string LastCallStatus { get; set; }
        public string lastCallNote { get; set; }
        public string AppProcessStatus { get; set; }
        public string AppStatusForSale { get; set; }
        public string DisbureseMonth { get; set; }
        public string CancelCode { get; set; }
        public string RejectCode { get; set; }
        public DateTime? DisbureseDate { get; set; }
        public int CustomerId { get; set; }

        public int Volumn { get; set; }
        public int AppCreate { get; set; }
        public int AppLoan { get; set; }
        public int AppAprove { get; set; }
        public int AppCancel { get; set; }
        public int AppReject { get; set; }

    }

    public class OcbProfileStatus
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    
    }

    public class OcbProductLoan
    {
        public int Id { get; set; }
        public string Ma_Doi_Tac { get; set; }
        public string Ma { get; set; }
        public string Ten { get; set; }

    }


}
