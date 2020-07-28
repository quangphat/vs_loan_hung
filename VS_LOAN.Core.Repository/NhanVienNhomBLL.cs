using NHibernate;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Entity.Model;
using VS_LOAN.Core.Nhibernate;
using VS_LOAN.Core.Utility.Exceptions;

namespace VS_LOAN.Core.Repository
{
    public class NhanVienNhomBLL
    {
        public List<UserPMModel> LayDSNhanVien(int userID, int quyen)
        {
            try
            {
                using (ISession session = LOANSessionManager.OpenSession())
                {
                    IDbCommand command = new SqlCommand();
                    command.Connection = session.Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "sp_Employee_LayDSByRule";
                    command.Parameters.Add(new SqlParameter("@UserID", userID));
                    command.Parameters.Add(new SqlParameter("@Rule", quyen));
                    DataTable dt = new DataTable();
                    dt.Load(command.ExecuteReader());
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            List<UserPMModel> result = new List<UserPMModel>();
                            foreach (DataRow item in dt.Rows)
                            {
                                UserPMModel nv = new UserPMModel();
                                nv.IDUser = Convert.ToInt32(item["ID"].ToString());
                                nv.Email = item["Email"].ToString();
                                nv.Code = item["Code"].ToString();
                                nv.UserName = item["UserName"].ToString();
                                nv.FullName = item["FullName"].ToString();
                                nv.Phone = item["Phone"].ToString();
                                result.Add(nv);
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
