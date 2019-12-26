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
        public async Task<HosoCourierViewModel> GetById(int id)
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryFirstOrDefaultAsync<HosoCourierViewModel>("sp_GetHosoCourierById", new { id = id}, 
                    commandType: CommandType.StoredProcedure);
                return result;
            }
        }
        public async Task<bool> Update(int id,HosoCourier hoso)
        {
            using (var con = GetConnection())
            {
                var p = new DynamicParameters();
                p.Add("id", id);
                p.Add("customerName", hoso.CustomerName);
                p.Add("cmnd", hoso.Cmnd);
                p.Add("updatedDate", DateTime.Now);
                p.Add("status", hoso.Status);
                p.Add("note", hoso.LastNote);
                p.Add("updatedBy", hoso.UpdatedBy);
                p.Add("phone", hoso.Phone);
                p.Add("assignId", hoso.AssignId);
                p.Add("partnerId", hoso.PartnerId);
                p.Add("productId", hoso.ProductId);
                await con.ExecuteAsync("sp_UpdateHosoCourier", p, commandType: CommandType.StoredProcedure);
                return true;
            }

        }
        public async Task<int> Create(HosoCourier hoso)
        {
            using (var con = GetConnection())
            {
                var p = AddOutputParam("id");
                p.Add("customerName", hoso.CustomerName);
                p.Add("receiveDate", DateTime.Now);
                p.Add("cmnd", hoso.Cmnd);
                p.Add("createddate", DateTime.Now);
                p.Add("status", hoso.Status);
                p.Add("note", hoso.LastNote);
                p.Add("createdby", hoso.CreatedBy);
                p.Add("phone", hoso.Phone);
                p.Add("assignId", hoso.AssignId);
                p.Add("partnerId", hoso.PartnerId);
                await con.ExecuteAsync("sp_InsertHosoCourrier", p, commandType: CommandType.StoredProcedure);
                return p.Get<int>("id");
            }

        }
        public async Task<int> CountHosoCourrier(string freeText, int courierId,string status)
        {
            status = string.IsNullOrWhiteSpace(status) ? string.Empty : status;
            using (var con = GetConnection())
            {
               return  await con.ExecuteScalarAsync<int>("sp_CountHosoCourier", new {freeText, courierId, status }, commandType: CommandType.StoredProcedure);
                
            }
        }
        public async Task<List<HosoCourierViewModel>> GetHosoCourrier(string freeText, int courierId,string status, int page, int limit)
        {
            status = string.IsNullOrWhiteSpace(status) ? string.Empty : status;
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<HosoCourierViewModel>("sp_GetHosoCourier",
                    new { freeText, courierId, page, limit_tmp = limit,status },
                    commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }
    }
}
