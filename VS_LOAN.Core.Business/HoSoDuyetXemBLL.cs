using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Nhibernate;
using VS_LOAN.Core.Utility.Exceptions;

namespace VS_LOAN.Core.Business
{
   public class HoSoDuyetXemBLL
    {
        public bool DaXem(int maHS)
        {
            try
            {
                using (var session = LOANSessionManager.OpenSession())
                {
                    IDbCommand command = new SqlCommand();
                    command.Connection = session.Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "sp_HO_SO_DUYET_XEM_DaXem";
                    command.Parameters.Add(new SqlParameter("@ID", maHS));
                    command.ExecuteNonQuery();
                    return true;
                }
            }
            catch (BusinessException ex)
            {
                throw ex;
            }
        }
        public bool Them(int maHS)
        {
            try
            {
                using (var session = LOANSessionManager.OpenSession())
                {
                    IDbCommand command = new SqlCommand();
                    command.Connection = session.Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "sp_HO_SO_DUYET_XEM_Them";
                    command.Parameters.Add(new SqlParameter("@ID", maHS));
                    command.ExecuteNonQuery();
                    return true;
                }
            }
            catch (BusinessException ex)
            {
                throw ex;
            }
        }
    }
}
