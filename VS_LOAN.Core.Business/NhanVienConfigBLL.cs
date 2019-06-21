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
    public class NhanVienConfigBLL
    {
        public bool CapNhat(int maNhanVien, List<int> lstIDNhom)
        {
            using (var session = LOANSessionManager.OpenSession())
            using (var transaction = session.BeginTransaction(IsolationLevel.RepeatableRead))
            {
                try
                {
                    IDbCommand commandXoaNhanVienNhom = new SqlCommand();
                    commandXoaNhanVienNhom.Connection = session.Connection;
                    commandXoaNhanVienNhom.CommandType = CommandType.StoredProcedure;
                    commandXoaNhanVienNhom.CommandText = "sp_NHAN_VIEN_CF_Xoa";
                    session.Transaction.Enlist(commandXoaNhanVienNhom);
                    commandXoaNhanVienNhom.Parameters.Add(new SqlParameter("@MaNhanVien", maNhanVien));
                    commandXoaNhanVienNhom.ExecuteNonQuery();

                    IDbCommand commandNhanVienNhom = new SqlCommand();
                    commandNhanVienNhom.Connection = session.Connection;
                    commandNhanVienNhom.CommandType = CommandType.StoredProcedure;
                    commandNhanVienNhom.CommandText = "sp_NHAN_VIEN_CF_Them";
                    session.Transaction.Enlist(commandNhanVienNhom);
                    if (lstIDNhom != null)
                    {
                        for (int i = 0; i < lstIDNhom.Count; i++)
                        {
                            commandNhanVienNhom.Parameters.Clear();
                            commandNhanVienNhom.Parameters.Add(new SqlParameter("@MaNhom", lstIDNhom[i]));
                            commandNhanVienNhom.Parameters.Add(new SqlParameter("@MaNhanVien", maNhanVien));
                            commandNhanVienNhom.ExecuteNonQuery();
                        }
                    }
                    transaction.Commit();
                    return true;
                }
                catch (BusinessException ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }
    }
}
