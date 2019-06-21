using NHibernate;
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

namespace VS_LOAN.Core.Business
{
    public class TrangThaiHoSoBLL
    {
        public List<TrangThaiHoSoModel> LayDSTrangThai()
        {
            try
            {
                using (ISession session = LOANSessionManager.OpenSession())
                {
                    IDbCommand command = new SqlCommand();
                    command.Connection = session.Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "sp_TRANG_THAI_HS_LayDSTrangThai";
                    DataTable dt = new DataTable();
                    dt.Load(command.ExecuteReader());
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            List<TrangThaiHoSoModel> result = new List<TrangThaiHoSoModel>();
                            foreach (DataRow item in dt.Rows)
                            {
                                TrangThaiHoSoModel tt = new TrangThaiHoSoModel();
                                tt.ID = Convert.ToInt32(item["ID"].ToString());
                                tt.Ten = item["Ten"].ToString();
                                result.Add(tt);
                            }
                            return result;
                        }
                    }
                    return null;
                }
            }
            catch (BusinessException ex)
            {
                throw ex;
            }
        }
    }
}
