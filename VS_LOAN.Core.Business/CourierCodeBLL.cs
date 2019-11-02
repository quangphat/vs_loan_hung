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
    public class CourierCodeBLL
    {

        public List<NhanVienInfoModel> LayDS()
        {
            try
            {
                using (var session = LOANSessionManager.OpenSession())
                {
                    IDbCommand command = new SqlCommand();
                    command.Connection = session.Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "sp_NHAN_VIEN_LayDSCourierCode";
                    var dt = new DataTable();
                    dt.Load(command.ExecuteReader());
                    if (dt == null)
                        return null;
                    List<NhanVienInfoModel> rs = new List<NhanVienInfoModel>();
                    foreach (DataRow item in dt.Rows)
                    {
                        NhanVienInfoModel us = new NhanVienInfoModel();
                        us.ID = Convert.ToInt32(item["ID"].ToString());
                        us.FullText = item["FullText"].ToString();

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
