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
    public class NhanVienBLL
    {
        public List<UserPMModel> LayDSByMaQL(int maQL)
        {
            try
            {
                using (var session = LOANSessionManager.OpenSession())
                {
                    IDbCommand command = new SqlCommand();
                    command.Connection = session.Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "sp_NHAN_VIEN_LayDSByMaQL";
                    command.Parameters.Add(new SqlParameter("@MaQL", maQL));
                    var dt = new DataTable();
                    dt.Load(command.ExecuteReader());
                    if (dt == null)
                        return null;
                    List<UserPMModel> rs = new List<UserPMModel>();
                    foreach (DataRow item in dt.Rows)
                    {
                        UserPMModel us = new UserPMModel();
                        us.IDUser = Convert.ToInt32(item["ID"].ToString());
                        us.Code = item["Code"].ToString();
                        us.FullName = item["HoTen"].ToString();
                        rs.Add(us);
                    }
                    return rs;
                }
            }
            catch (BusinessException ex)
            {
                throw ex;
            }
        }

      
    }
}
