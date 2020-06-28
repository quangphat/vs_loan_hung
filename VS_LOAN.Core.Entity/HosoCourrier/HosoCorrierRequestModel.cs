using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.HosoCourrier
{
    public class HosoCorrierRequestModel
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public string Cmnd { get; set; }
        public string ReceiveDate { get; set; }
        public int ProductId { get; set; }
        public int PartnerId { get; set; }
        //public int DistrictId { get; set; }
        //public int ProvinceId { get; set; }
        //public string Address { get; set; }
        public int Status { get; set; }
        public string LastNote { get; set; }
        public int AssignId { get; set; }
        public int GroupId { get; set; }
    }
}
