using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.MCreditModels.SqlModel
{
    public class OcbProfile : BaseSqlEntity
    {
        public DateTime IdIssueDate { get; set; }
        

        public int Id { get; set; }

        public string TraceCode { get; set; }

        public string FullNamme { get; set; }

        public bool? Gender { get; set; }

        public string IdCard { get; set; }
        public string idIssuePlaceId { get; set; }
        public DateTime BirthDay { get; set; }

        public string Mobilephone { get; set; }

        public decimal? InCome { get; set; }

        public string CurAddressWardId { get; set; }

        public string RurAddressDistId { get; set; }

        public string CurAddressProvinceId { get; set; }

        public string RegAddressWardId { get; set; }

        public string RegAddressDistId { get; set; }

        public string RegAddressProvinceId { get; set; }
        public string CurAddressDistId { get; set; }

        public string ProductId { get; set; }

        public string SellerNote { get; set; }

        public string CustomerId { get; set; }

        public bool? IsPushDocument { get; set; }

        public decimal? RequestLoanAmount { get; set; }

        public int? RequestLoanTerm { get; set; }
        public string RegAddressNumber { get; set; }

        public string RegAddressStreet { get; set; }
        public string RegAddressRegion { get; set; }
        public string CurAddressNumber { get; set; }

        public string CurAddressStreet { get; set; }

        public string CurAddressRegion { get; set; }

        public string IncomeType { get; set; }
        public string Email { get; set; }
        public int? AssigneeId { get; set; }

        public bool? IsDuplicateAdrees { get; set; }

        public int Status { get; set; }

        public string ReferenceFullName1 { get; set; }
        public string ReferenceRelationship1 { get; set; }
        public string ReferencePhone1 { get; set; }
        public int? Reference1Gender { get; set; }


        public string ReferenceFullName2 { get; set; }
        public string ReferenceRelationship2 { get; set; }
        public string ReferencePhone2 { get; set; }
        public int? Reference2Gender { get; set; }

        public string ReferenceFullName3 { get; set; }
        public string ReferenceRelationship3{ get; set; }
        public string ReferencePhone3 { get; set; }
        public int? Reference3Gender { get; set; }


    }

}
