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

namespace VS_LOAN.Core.Repository
{
   public class KhuVucBLL
    {
        public List<KhuVucModel> LayDSTinh()
        {
            try
            {
                using (var session = LOANSessionManager.OpenSession())
                {
                    IDbCommand command = new SqlCommand();
                    command.Connection = session.Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "sp_KHU_VUC_LayDSTinh";
                    var dt = new DataTable();
                    dt.Load(command.ExecuteReader());
                    if (dt == null)
                        return null;
                    List<KhuVucModel> rs = new List<KhuVucModel>();
                    foreach (DataRow item in dt.Rows)
                    {
                        KhuVucModel kv = new KhuVucModel();
                        kv.ID = Convert.ToInt32(item["ID"].ToString());
                        kv.Ten = item["Ten"].ToString();
                        rs.Add(kv);
                    }
                    return rs;
                }
            }
            catch (BusinessException ex)
            {
                throw ex;
            }
        }
        public List<KhuVucModel> LayDSHuyen(int maTinh)
        {
            try
            {
                using (var session = LOANSessionManager.OpenSession())
                {
                    IDbCommand command = new SqlCommand();
                    command.Connection = session.Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "sp_KHU_VUC_LayDSHuyen";
                    command.Parameters.Add(new SqlParameter("@MaTinh", maTinh));
                    var dt = new DataTable();
                    dt.Load(command.ExecuteReader());
                    if (dt == null)
                        return null;
                    List<KhuVucModel> rs = new List<KhuVucModel>();
                    foreach (DataRow item in dt.Rows)
                    {
                        KhuVucModel kv = new KhuVucModel();
                        kv.ID = Convert.ToInt32(item["ID"].ToString());
                        kv.Ten = item["Ten"].ToString();
                        rs.Add(kv);
                    }
                    return rs;
                }
            }
            catch (BusinessException ex)
            {
                throw ex;
            }
        }
        public int LayMaTinh(int maHuyen)
        {
            try
            {
                using (var session = LOANSessionManager.OpenSession())
                {
                    IDbCommand command = new SqlCommand();
                    command.Connection = session.Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "sp_KHU_VUC_LayMaTinhByMaHuyen";
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@MaHuyen", maHuyen));
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
