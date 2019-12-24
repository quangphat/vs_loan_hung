using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity.HosoCourrier;

namespace VS_LOAN.Core.Business
{
    public class HosoCourrierBusiness:BaseBusiness
    {
        public async Task<int> Create(HosoCourrier hoso)
        {
            using (var con = GetConnection())
            {
                var p = AddOutputParam("id");
                p.Add("customerName", hoso.CustomerName);
                p.Add("receiveDate", hoso.ReceiveDate);
                p.Add("cmnd", hoso.Cmnd);
                p.Add("createddate", DateTime.Now);
                p.Add("status", hoso.Status);
                p.Add("note", hoso.LastNote);
                p.Add("createdby", hoso.CreatedBy);
                p.Add("phone", hoso.Phone);
                p.Add("address", hoso.Address);
                p.Add("districtId", hoso.DistrictId);
                p.Add("provinceId", hoso.ProvinceId);
                p.Add("partnerId", hoso.PartnerId);
                await con.ExecuteAsync("sp_InsertCustomer", p, commandType: CommandType.StoredProcedure);
                return p.Get<int>("id");
            }

        }
        
    }
}
