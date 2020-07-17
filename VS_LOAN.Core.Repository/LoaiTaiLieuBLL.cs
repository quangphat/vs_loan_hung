using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Nhibernate;
using VS_LOAN.Core.Utility.Exceptions;

namespace VS_LOAN.Core.Repository
{
   public class LoaiTaiLieuBLL
    {
        public List<LoaiTaiLieuModel> LayDS()
        {
            try
            {
                using (var session = LOANSessionManager.OpenSession())
                {
                    IDbCommand command = new SqlCommand();
                    command.Connection = session.Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "sp_LOAI_TAI_LIEU_LayDS";
                    var dt = new DataTable();
                    dt.Load(command.ExecuteReader());
                    if (dt == null)
                        return null;
                    List<LoaiTaiLieuModel> rs = new List<LoaiTaiLieuModel>();
                    foreach (DataRow item in dt.Rows)
                    {
                        LoaiTaiLieuModel cm = new LoaiTaiLieuModel();
                        cm.ID = Convert.ToInt32(item["ID"].ToString());
                        cm.Ten = item["Ten"].ToString();
                        cm.BatBuoc = Convert.ToInt32(item["BatBuoc"].ToString());
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
    }
}
