using VS_LOAN.Core.Nhibernate;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Utility.Exceptions;

namespace VS_LOAN.Core.Repository
{
   public class GrantRightBLL
    {

        public string GetListRule(string id)
        {
            try
            {
                using (var session = LOANSessionManager.OpenSession())
                {
                    IDbCommand command = new SqlCommand();
                    command.Connection = session.Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "sp_RULE_GetWidthID";
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@ID", id));
                    DataTable dt = new DataTable();
                    dt.Load(command.ExecuteReader());
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow item in dt.Rows)
                            {
                                return item["Rule"].ToString();
                            }
                        }
                    } 
                }
                return "";
            }
            catch (BusinessException ex)
            {
                throw ex;
            }
        }
    }
}
