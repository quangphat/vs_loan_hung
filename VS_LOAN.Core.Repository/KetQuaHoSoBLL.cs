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

namespace VS_LOAN.Core.Repository
{
    public class KetQuaHoSoBLL
    {
        public List<KetQuaHoSoModel> LayDSKetQua()
        {
            try
            {
                using (ISession session = LOANSessionManager.OpenSession())
                {
                    IDbCommand command = new SqlCommand();
                    command.Connection = session.Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "sp_KET_QUA_HS_LayDSKetQua";
                    DataTable dt = new DataTable();
                    dt.Load(command.ExecuteReader());
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            List<KetQuaHoSoModel> result = new List<KetQuaHoSoModel>();
                            foreach (DataRow item in dt.Rows)
                            {
                                KetQuaHoSoModel kq = new KetQuaHoSoModel();
                                kq.ID = Convert.ToInt32(item["ID"].ToString());
                                kq.Ten = item["Ten"].ToString();
                                result.Add(kq);
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
