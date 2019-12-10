using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity.Model;
using VS_LOAN.Core.Nhibernate;
using VS_LOAN.Core.Utility.Exceptions;

namespace VS_LOAN.Core.Business
{
   public class DoiTacBLL
    {
        public List<DoiTacModel> LayDS()
        {
            try
            {
                using (var session = LOANSessionManager.OpenSession())
                {
                    IDbCommand command = new SqlCommand();
                    command.Connection = session.Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "sp_DOI_TAC_LayDS";
                    var dt = new DataTable();
                    dt.Load(command.ExecuteReader());
                    if (dt == null)
                        return null;
                    List<DoiTacModel> rs = new List<DoiTacModel>();
                    foreach (DataRow item in dt.Rows)
                    {
                        DoiTacModel cm = new DoiTacModel();
                        cm.ID = Convert.ToInt32(item["ID"].ToString());
                        cm.Ten = item["Ten"].ToString();
                        cm.F88Value = item["F88Value"] !=null ? Convert.ToInt32(item["F88Value"].ToString()) : 0;
                        rs.Add(cm);
                    }
                    return rs;
                }
            }
            catch (BusinessException ex)
            {
                throw ex;
            }
        }
        public int LayMaDoiTac(int maSanPham)
        {
            try
            {
                using (var session = LOANSessionManager.OpenSession())
                {
                    IDbCommand command = new SqlCommand();
                    command.Connection = session.Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "sp_DOI_TAC_LayIDByMaSanPham";
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@MaSanPham", maSanPham));
                    var dt = new DataTable();
                    dt.Load(command.ExecuteReader());
                    if (dt == null)
                        return 0;
                    List<DoiTacModel> rs = new List<DoiTacModel>();
                    foreach (DataRow item in dt.Rows)
                    {
                        return Convert.ToInt32(item["ID"].ToString()); 
                    }
                    return 0;
                }
            }
            catch (BusinessException ex)
            {
                throw ex;
            }
        }
    }
}
