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
  public  class SanPhamBLL
    {
        public List<SanPhamModel> LaySanPhamByID(int id)
        {
            try
            {
                using (var session = LOANSessionManager.OpenSession())
                {
                    IDbCommand command = new SqlCommand();
                    command.Connection = session.Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "sp_SAN_PHAM_VAY_LayDSByID";
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@MaDoiTac", id));
                    var dt = new DataTable();
                    dt.Load(command.ExecuteReader());
                    if (dt == null)
                        return null;
                    List<SanPhamModel> rs = new List<SanPhamModel>();
                    foreach (DataRow item in dt.Rows)
                    {
                        SanPhamModel cm = new SanPhamModel();
                        cm.ID = Convert.ToInt32(item["ID"].ToString());
                        cm.Ten = item["Ten"].ToString();
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
        public List<SanPhamModel> LaySanPhamByID(int id, int maHS)
        {
            try
            {
                using (var session = LOANSessionManager.OpenSession())
                {
                    IDbCommand command = new SqlCommand();
                    command.Connection = session.Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "sp_SAN_PHAM_VAY_LayDSByIDAndMaHS";
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@MaDoiTac", id));
                    command.Parameters.Add(new SqlParameter("@MaHS", maHS));
                    var dt = new DataTable();
                    dt.Load(command.ExecuteReader());
                    if (dt == null)
                        return null;
                    List<SanPhamModel> rs = new List<SanPhamModel>();
                    foreach (DataRow item in dt.Rows)
                    {
                        SanPhamModel cm = new SanPhamModel();
                        cm.ID = Convert.ToInt32(item["ID"].ToString());
                        cm.Ten = item["Ten"].ToString();
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

        public int Them(SanPhamVayModel sanPham)
        {
            using (var session = LOANSessionManager.OpenSession())
            using (var transaction = session.BeginTransaction(IsolationLevel.RepeatableRead))
            {
                try
                {
                    IDbCommand commandNhom = new SqlCommand();
                    commandNhom.Connection = session.Connection;
                    commandNhom.CommandType = CommandType.StoredProcedure;
                    commandNhom.CommandText = "sp_SAN_PHAM_VAY_Them";
                    session.Transaction.Enlist(commandNhom);
                    commandNhom.Parameters.Add(new SqlParameter("@ID", SqlDbType.BigInt) { Direction = ParameterDirection.Output });
                    commandNhom.Parameters.Add(new SqlParameter("@MaDoiTac", sanPham.MaDoiTac));
                    commandNhom.Parameters.Add(new SqlParameter("@Ma", sanPham.Ma));
                    commandNhom.Parameters.Add(new SqlParameter("@Ten", sanPham.Ten));
                    if(sanPham.NgayTao == DateTime.MinValue)
                        commandNhom.Parameters.Add(new SqlParameter("@NgayTao", DBNull.Value));
                    else
                        commandNhom.Parameters.Add(new SqlParameter("@NgayTao", sanPham.NgayTao));
                    commandNhom.Parameters.Add(new SqlParameter("@MaNguoiTao", sanPham.MaNguoiTao));
                    commandNhom.Parameters.Add(new SqlParameter("@Loai", sanPham.Loai));
                    commandNhom.ExecuteNonQuery();
                    int maNhom = Convert.ToInt32((((SqlParameter)commandNhom.Parameters["@ID"]).Value).ToString());
                    transaction.Commit();
                    return maNhom;
                }
                catch (BusinessException ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        public List<ThongTinSanPhamVayModel> LayThongTinSanPhamByID(int id, DateTime ngayTao)
        {
            try
            {
                using (var session = LOANSessionManager.OpenSession())
                {
                    IDbCommand command = new SqlCommand();
                    command.Connection = session.Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "sp_SAN_PHAM_VAY_LayDSThongTinByID";
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@MaDoiTac", id));
                    command.Parameters.Add(new SqlParameter("@NgayTao", ngayTao));
                    
                    var dt = new DataTable();
                    dt.Load(command.ExecuteReader());
                    if (dt == null)
                        return null;
                    List<ThongTinSanPhamVayModel> rs = new List<ThongTinSanPhamVayModel>();
                    foreach (DataRow item in dt.Rows)
                    {
                        ThongTinSanPhamVayModel sp = new ThongTinSanPhamVayModel();
                        sp.ID = Convert.ToInt32(item["ID"].ToString());
                        sp.Ten = item["Ten"].ToString();
                        sp.Ma = item["Ma"].ToString();
                        try
                        {
                            sp.NgayTao = Convert.ToDateTime(item["NgayTao"].ToString());
                        }
                        catch (Exception)
                        {
                        }
                        sp.NguoiTao = item["NguoiTao"].ToString();
                        rs.Add(sp);
                    }
                    return rs;
                }
            }
            catch (BusinessException ex)
            {
                throw ex;
            }
        }
        public bool Xoa(int id)
        {
            using (var session = LOANSessionManager.OpenSession())
            using (var transaction = session.BeginTransaction(IsolationLevel.RepeatableRead))
            {
                try
                {
                    IDbCommand command = new SqlCommand();
                    command.Connection = session.Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "sp_SAN_PHAM_VAY_Xoa";
                    session.Transaction.Enlist(command);
                    command.Parameters.Add(new SqlParameter("@ID", id));
                    command.ExecuteNonQuery();
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

        public bool Trung(string ma)
        {
            try
            {
                using (var session = LOANSessionManager.OpenSession())
                {
                    IDbCommand command = new SqlCommand();
                    command.Connection = session.Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "sp_SAN_PHAM_VAY_DemTrungMa";
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@Ma", ma));
                    int rs = Convert.ToInt32(command.ExecuteScalar());
                    if (rs > 0)
                        return true;
                    return false;
                }
            }
            catch (BusinessException ex)
            {
                throw ex;
            }
        }
    }
}
