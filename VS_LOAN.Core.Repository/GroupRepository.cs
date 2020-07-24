using Dapper;
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
    public class GroupRepository : BaseRepository
    {
        public GroupRepository() : base(typeof(GroupRepository))
        {

        }
        public bool CheckIsTeamlead(int nhanvienId)
        {
            return checkIsTeamLeadByUserId(nhanvienId);
            //List<NhomDropDownModel> groups = LayDSNhomByNhanvienQuanly(nhanvienId);
            //if (groups == null || !groups.Any())
            //    return false;
            //return true;
        }
        public bool CheckIsAdmin(int userId)
        {
            try
            {
                using (ISession session = LOANSessionManager.OpenSession())
                {
                    IDbCommand command = new SqlCommand();
                    command.Connection = session.Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "sp_CheckIsAdmin";
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@userId", userId));
                    var dt = new DataTable();
                    dt.Load(command.ExecuteReader());
                    if (dt == null || dt.Rows == null || dt.Rows.Count == 0)
                        return false;
                    return Convert.ToBoolean(dt.Rows[0]["isAdmin"]);
                }
            }
            catch (BusinessException ex)
            {
                return false;
            }
        }
        // Danh sách quản lý hồ sơ (lấy từ quản lý nhóm)
        public List<NhomDropDownModel> LayDSCuaNhanVien(int userID)
        {
            try
            {
                using (ISession session = LOANSessionManager.OpenSession())
                {
                    IDbCommand command = new SqlCommand();
                    command.Connection = session.Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "sp_NHOM_LayDSChonTheoNhanVien";
                    command.Parameters.Add(new SqlParameter("@UserID", userID));
                    DataTable dt = new DataTable();
                    dt.Load(command.ExecuteReader());
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            List<NhomDropDownModel> result = new List<NhomDropDownModel>();
                            List<NhomDropDownModel> resultTemp = new List<NhomDropDownModel>();
                            foreach (DataRow item in dt.Rows)
                            {
                                NhomDropDownModel nhom = new NhomDropDownModel();
                                nhom.ID = Convert.ToInt32(item["ID"].ToString());
                                nhom.Ten = item["Ten"].ToString();
                                nhom.ChuoiMaCha = item["ChuoiMaCha"].ToString();
                                nhom.TenQL = item["TenQL"].ToString();
                                // Nếu nhóm con
                                if (resultTemp.Find(x => nhom.ChuoiMaCha.Contains("." + x.ID.ToString() + ".") || nhom.ChuoiMaCha.EndsWith("." + x.ID.ToString())) != null)
                                    continue;
                                else
                                {
                                    // Nhóm cha
                                    resultTemp.RemoveAll(x => x.ChuoiMaCha.Contains("." + nhom.ID.ToString() + ".") || x.ChuoiMaCha.EndsWith("." + nhom.ID.ToString()));
                                    resultTemp.Add(nhom);
                                }
                                if (resultTemp.Count > 0)
                                {
                                    IDbCommand commandLayDSCon = new SqlCommand();
                                    commandLayDSCon.Connection = session.Connection;
                                    commandLayDSCon.CommandType = CommandType.StoredProcedure;
                                    commandLayDSCon.CommandText = "sp_NHOM_LayCayNhomCon";
                                    for (int i = 0; i < resultTemp.Count; i++)
                                    {
                                        commandLayDSCon.Parameters.Clear();
                                        commandLayDSCon.Parameters.Add(new SqlParameter("@MaNhomCha", resultTemp[i].ID));
                                        DataTable dt2 = new DataTable();
                                        dt2.Load(commandLayDSCon.ExecuteReader());
                                        if (dt2 != null && dt2.Rows.Count > 0)
                                        {
                                            List<NhomDropDownModel> lstTemp = new List<NhomDropDownModel>();
                                            foreach (DataRow item2 in dt2.Rows)
                                            {
                                                NhomDropDownModel nhom2 = new NhomDropDownModel();
                                                nhom2.ID = Convert.ToInt32(item2["ID"].ToString());
                                                nhom2.MaNguoiQL = Convert.ToInt32(item2["MaNguoiQL"].ToString());
                                                nhom2.Ten = item2["Ten"].ToString();
                                                nhom2.ChuoiMaCha = item2["ChuoiMaCha"].ToString();
                                                nhom2.TenQL = item2["TenQL"].ToString();
                                                lstTemp.Add(nhom2);
                                            }
                                            result.AddRange(TaoCayDSNhom(lstTemp, resultTemp[i].ChuoiMaCha + "." + resultTemp[i].ID.ToString(), userID));
                                        }
                                    }
                                }
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

        public async Task<List<NhomDropDownModel>> GetAll()
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<NhomDropDownModel>("sp_NHOM_LayDSNhom",
                    commandType: CommandType.StoredProcedure);
                return TaoCayDSNhom(result.ToList(), "0");
            }
            //try
            //{
            //    using (ISession session = LOANSessionManager.OpenSession())
            //    {
            //        IDbCommand command = new SqlCommand();
            //        command.Connection = session.Connection;
            //        command.CommandType = CommandType.StoredProcedure;
            //        command.CommandText = "sp_NHOM_LayDSNhom";
            //        DataTable dt = new DataTable();
            //        dt.Load(command.ExecuteReader());
            //        if (dt != null)
            //        {
            //            if (dt.Rows.Count > 0)
            //            {
            //                List<NhomDropDownModel> lstTemp = new List<NhomDropDownModel>();
            //                foreach (DataRow item in dt.Rows)
            //                {
            //                    NhomDropDownModel nhom = new NhomDropDownModel();
            //                    nhom.ID = Convert.ToInt32(item["ID"].ToString());
            //                    nhom.Ten = item["Ten"].ToString();
            //                    nhom.ChuoiMaCha = item["ChuoiMaCha"].ToString();
            //                    lstTemp.Add(nhom);
            //                }
            //                return TaoCayDSNhom(lstTemp, "0");
            //            }
            //        }
            //        return null;
            //    }
            //}
            //catch (BusinessException ex)
            //{
            //    throw ex;
            //}
        }

        public int Them(NhomModel nhom, List<int> lstThanhVien)
        {
            using (var session = LOANSessionManager.OpenSession())
            using (var transaction = session.BeginTransaction(IsolationLevel.RepeatableRead))
            {
                try
                {
                    IDbCommand commandNhom = new SqlCommand();
                    commandNhom.Connection = session.Connection;
                    commandNhom.CommandType = CommandType.StoredProcedure;
                    commandNhom.CommandText = "sp_NHOM_Them";
                    session.Transaction.Enlist(commandNhom);
                    commandNhom.Parameters.Add(new SqlParameter("@ID", SqlDbType.BigInt) { Direction = ParameterDirection.Output });
                    commandNhom.Parameters.Add(new SqlParameter("@MaNhomCha", nhom.MaNhomCha));
                    commandNhom.Parameters.Add(new SqlParameter("@MaNguoiQL", nhom.MaNguoiQL));
                    commandNhom.Parameters.Add(new SqlParameter("@TenVietTat", nhom.TenNgan));
                    commandNhom.Parameters.Add(new SqlParameter("@Ten", nhom.Ten));
                    commandNhom.Parameters.Add(new SqlParameter("@ChuoiMaCha", nhom.ChuoiMaCha));
                    commandNhom.ExecuteNonQuery();
                    int maNhom = Convert.ToInt32((((SqlParameter)commandNhom.Parameters["@ID"]).Value).ToString());
                    IDbCommand commandNhanVienNhom = new SqlCommand();
                    commandNhanVienNhom.Connection = session.Connection;
                    commandNhanVienNhom.CommandType = CommandType.StoredProcedure;
                    commandNhanVienNhom.CommandText = "sp_NHAN_VIEN_NHOM_Them";
                    session.Transaction.Enlist(commandNhanVienNhom);
                    if (lstThanhVien != null)
                    {
                        for (int i = 0; i < lstThanhVien.Count; i++)
                        {
                            commandNhanVienNhom.Parameters.Clear();
                            commandNhanVienNhom.Parameters.Add(new SqlParameter("@MaNhom", maNhom));
                            commandNhanVienNhom.Parameters.Add(new SqlParameter("@MaNhanVien", lstThanhVien[i]));
                            commandNhanVienNhom.ExecuteNonQuery();
                        }
                    }
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

        public List<NhomDropDownModel> LayDSNhom()
        {
            try
            {
                using (ISession session = LOANSessionManager.OpenSession())
                {
                    IDbCommand command = new SqlCommand();
                    command.Connection = session.Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "sp_NHOM_LayDSNhom";
                    DataTable dt = new DataTable();
                    dt.Load(command.ExecuteReader());
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            List<NhomDropDownModel> result = new List<NhomDropDownModel>();
                            foreach (DataRow item in dt.Rows)
                            {
                                NhomDropDownModel nhom = new NhomDropDownModel();
                                nhom.ID = Convert.ToInt32(item["ID"].ToString());
                                nhom.Ten = item["Ten"].ToString();
                                result.Add(nhom);
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
        public async Task<List<OptionSimple>> GetEmployeesByGroupId(int groupId, bool isLeader = false)
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<OptionSimple>("sp_NHOM_GetEmployeesByGroupId",
                    new { groupId, isGetLeader = isLeader },
                    commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }
        public List<ThongTinToNhomModel> LayDSNhomCon(int maNhomCha)
        {
            try
            {
                using (ISession session = LOANSessionManager.OpenSession())
                {
                    IDbCommand command = new SqlCommand();
                    command.Connection = session.Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "sp_Group_GetChildGroup";
                    command.Parameters.Add(new SqlParameter("@MaNhomCha", maNhomCha));
                    DataTable dt = new DataTable();
                    dt.Load(command.ExecuteReader());
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            List<ThongTinToNhomModel> result = new List<ThongTinToNhomModel>();
                            foreach (DataRow item in dt.Rows)
                            {
                                ThongTinToNhomModel nhom = new ThongTinToNhomModel();
                                nhom.ID = Convert.ToInt32(item["ID"].ToString());
                                nhom.Ten = item["Ten"].ToString();
                                nhom.TenNgan = item["TenNgan"].ToString();
                                nhom.NguoiQuanLy = item["NguoiQuanLy"].ToString();
                                result.Add(nhom);
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

        public ThongTinToNhomSuaModel LayTheoMa(int maNhom)
        {
            try
            {
                using (ISession session = LOANSessionManager.OpenSession())
                {
                    IDbCommand command = new SqlCommand();
                    command.Connection = session.Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "sp_NHOM_LayThongTinTheoMa";
                    command.Parameters.Add(new SqlParameter("@MaNhom", maNhom));
                    DataTable dt = new DataTable();
                    dt.Load(command.ExecuteReader());
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            ThongTinToNhomSuaModel nhom = new ThongTinToNhomSuaModel();
                            nhom.ID = Convert.ToInt32(dt.Rows[0]["ID"].ToString());
                            nhom.Ten = dt.Rows[0]["Ten"].ToString();
                            nhom.TenNgan = dt.Rows[0]["TenNgan"].ToString();
                            nhom.MaNguoiQuanLy = Convert.ToInt32(dt.Rows[0]["MaNguoiQuanLy"].ToString());
                            nhom.MaNhomCha = Convert.ToInt32(dt.Rows[0]["MaNhomCha"].ToString());
                            return nhom;
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

        public ThongTinChiTietToNhomModel LayChiTietTheoMa(int maNhom)
        {
            try
            {
                using (ISession session = LOANSessionManager.OpenSession())
                {
                    IDbCommand command = new SqlCommand();
                    command.Connection = session.Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "sp_Group_GetById";
                    command.Parameters.Add(new SqlParameter("@groupId", maNhom));
                    DataTable dt = new DataTable();
                    dt.Load(command.ExecuteReader());
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            ThongTinChiTietToNhomModel nhom = new ThongTinChiTietToNhomModel();
                            nhom.ID = Convert.ToInt32(dt.Rows[0]["ID"].ToString());
                            nhom.Ten = dt.Rows[0]["Ten"].ToString();
                            nhom.TenNgan = dt.Rows[0]["TenNgan"].ToString();
                            nhom.NguoiQuanLy = dt.Rows[0]["NguoiQuanLy"].ToString();
                            nhom.TenNhomCha = dt.Rows[0]["TenNhomCha"].ToString();
                            return nhom;
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

        public bool Sua(NhomModel nhom, List<int> lstThanhVien)
        {
            using (var session = LOANSessionManager.OpenSession())
            using (var transaction = session.BeginTransaction(IsolationLevel.RepeatableRead))
            {
                try
                {
                    IDbCommand commandNhom = new SqlCommand();
                    commandNhom.Connection = session.Connection;
                    commandNhom.CommandType = CommandType.StoredProcedure;
                    commandNhom.CommandText = "sp_NHOM_Sua";
                    session.Transaction.Enlist(commandNhom);
                    commandNhom.Parameters.Add(new SqlParameter("@ID", nhom.ID));
                    commandNhom.Parameters.Add(new SqlParameter("@MaNhomCha", nhom.MaNhomCha));
                    commandNhom.Parameters.Add(new SqlParameter("@MaNguoiQL", nhom.MaNguoiQL));
                    commandNhom.Parameters.Add(new SqlParameter("@TenVietTat", nhom.TenNgan));
                    commandNhom.Parameters.Add(new SqlParameter("@Ten", nhom.Ten));
                    commandNhom.Parameters.Add(new SqlParameter("@ChuoiMaCha", nhom.ChuoiMaCha));
                    commandNhom.ExecuteNonQuery();

                    IDbCommand commandXoaNhanVienNhom = new SqlCommand();
                    commandXoaNhanVienNhom.Connection = session.Connection;
                    commandXoaNhanVienNhom.CommandType = CommandType.StoredProcedure;
                    commandXoaNhanVienNhom.CommandText = "sp_NHAN_VIEN_NHOM_Xoa";
                    session.Transaction.Enlist(commandXoaNhanVienNhom);
                    commandXoaNhanVienNhom.Parameters.Add(new SqlParameter("@MaNhom", nhom.ID));
                    commandXoaNhanVienNhom.ExecuteNonQuery();

                    IDbCommand commandNhanVienNhom = new SqlCommand();
                    commandNhanVienNhom.Connection = session.Connection;
                    commandNhanVienNhom.CommandType = CommandType.StoredProcedure;
                    commandNhanVienNhom.CommandText = "sp_NHAN_VIEN_NHOM_Them";
                    session.Transaction.Enlist(commandNhanVienNhom);
                    if (lstThanhVien != null)
                    {
                        for (int i = 0; i < lstThanhVien.Count; i++)
                        {
                            commandNhanVienNhom.Parameters.Clear();
                            commandNhanVienNhom.Parameters.Add(new SqlParameter("@MaNhom", nhom.ID));
                            commandNhanVienNhom.Parameters.Add(new SqlParameter("@MaNhanVien", lstThanhVien[i]));
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

        private List<NhomDropDownModel> TaoCayDSNhom(List<NhomDropDownModel> lstData, string maCha)
        {
            try
            {
                if (lstData == null)
                    return null;
                List<NhomDropDownModel> lstResult = new List<NhomDropDownModel>();
                Stack<NhomDropDownModel> stack = new Stack<NhomDropDownModel>();
                List<NhomDropDownModel> lstFind = new List<NhomDropDownModel>();
                do
                {
                    if (stack.Count > 0)
                    {
                        NhomDropDownModel nhom = stack.Pop();
                        if (nhom != null)
                        {
                            string[] tempArray = nhom.ChuoiMaCha.Split('.');
                            if (tempArray.Length > 1)
                            {
                                for (int i = 0; i < tempArray.Length - 1; i++)
                                {
                                    nhom.Ten = "-" + nhom.Ten;
                                }
                            }
                            lstResult.Add(nhom);
                            maCha = nhom.ChuoiMaCha + "." + nhom.ID;
                        }
                    }
                    lstFind = lstData.FindAll(x => x.ChuoiMaCha.Equals(maCha));
                    if (lstFind != null)
                    {

                        for (int i = lstFind.Count - 1; i >= 0; i--)
                        {
                            stack.Push(lstFind[i]);
                            lstData.Remove(lstFind[i]);
                        }
                    }
                } while (stack.Count > 0);
                return lstResult;
            }
            catch (BusinessException ex)
            {
                throw ex;
            }
        }

        private List<NhomDropDownModel> TaoCayDSNhom(List<NhomDropDownModel> lstData, string maCha, int maNguoiQL)
        {
            try
            {
                if (lstData == null)
                    return null;
                List<NhomDropDownModel> lstResult = new List<NhomDropDownModel>();
                Stack<NhomDropDownModel> stack = new Stack<NhomDropDownModel>();
                List<NhomDropDownModel> lstFind = new List<NhomDropDownModel>();
                string maChaTemp = maCha;
                do
                {
                    if (stack.Count > 0)
                    {
                        NhomDropDownModel nhom = stack.Pop();
                        if (nhom != null)
                        {
                            string[] tempArray = nhom.ChuoiMaCha.Split('.');
                            if (tempArray.Length > 1)
                            {
                                for (int i = 0; i < tempArray.Length - 1; i++)
                                {
                                    nhom.Ten = "-" + nhom.Ten;
                                }
                            }
                            lstResult.Add(nhom);
                            maChaTemp = nhom.ChuoiMaCha + "." + nhom.ID;
                        }
                    }
                    lstFind = lstData.FindAll(x => x.ChuoiMaCha.Equals(maChaTemp));
                    if (lstFind != null)
                    {
                        for (int i = lstFind.Count - 1; i >= 0; i--)
                        {
                            if (lstFind[i].ChuoiMaCha == maCha && lstFind[i].MaNguoiQL != maNguoiQL)
                                continue;
                            stack.Push(lstFind[i]);
                            lstData.Remove(lstFind[i]);
                        }
                    }
                } while (stack.Count > 0);
                return lstResult;
            }
            catch (BusinessException ex)
            {
                throw ex;
            }
        }

        // Danh sách nhóm duyệt(lấy từ bảng cấu hình)
        public List<NhomDropDownModel> LayDSDuyetCuaNhanVien(int userID)
        {
            try
            {
                using (ISession session = LOANSessionManager.OpenSession())
                {
                    IDbCommand command = new SqlCommand();
                    command.Connection = session.Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "sp_NHOM_LayDSNhomDuyetChonTheoNhanVien";
                    command.Parameters.Add(new SqlParameter("@UserID", userID));
                    DataTable dt = new DataTable();
                    dt.Load(command.ExecuteReader());
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            List<NhomDropDownModel> result = new List<NhomDropDownModel>();
                            List<NhomDropDownModel> resultTemp = new List<NhomDropDownModel>();
                            foreach (DataRow item in dt.Rows)
                            {
                                NhomDropDownModel nhom = new NhomDropDownModel();
                                nhom.ID = Convert.ToInt32(item["ID"].ToString());
                                nhom.Ten = item["Ten"].ToString();
                                nhom.ChuoiMaCha = item["ChuoiMaCha"].ToString();
                                nhom.TenQL = item["TenQL"].ToString();
                                // Nếu nhóm con
                                if (resultTemp.Find(x => nhom.ChuoiMaCha.Contains("." + x.ID.ToString() + ".") || nhom.ChuoiMaCha.EndsWith("." + x.ID.ToString())) != null)
                                    continue;
                                else
                                {
                                    // Nhóm cha
                                    resultTemp.RemoveAll(x => x.ChuoiMaCha.Contains("." + nhom.ID.ToString() + ".") || x.ChuoiMaCha.EndsWith("." + nhom.ID.ToString()));
                                    resultTemp.Add(nhom);
                                }
                                if (resultTemp.Count > 0)
                                {
                                    IDbCommand commandLayDSCon = new SqlCommand();
                                    commandLayDSCon.Connection = session.Connection;
                                    commandLayDSCon.CommandType = CommandType.StoredProcedure;
                                    commandLayDSCon.CommandText = "sp_NHOM_LayCayNhomCon";
                                    for (int i = 0; i < resultTemp.Count; i++)
                                    {
                                        commandLayDSCon.Parameters.Clear();
                                        commandLayDSCon.Parameters.Add(new SqlParameter("@MaNhomCha", resultTemp[i].ID));
                                        DataTable dt2 = new DataTable();
                                        dt2.Load(commandLayDSCon.ExecuteReader());
                                        if (dt2 != null && dt2.Rows.Count > 0)
                                        {
                                            List<NhomDropDownModel> lstTemp = new List<NhomDropDownModel>();
                                            foreach (DataRow item2 in dt2.Rows)
                                            {
                                                NhomDropDownModel nhom2 = new NhomDropDownModel();
                                                nhom2.ID = Convert.ToInt32(item2["ID"].ToString());
                                                nhom2.MaNguoiQL = Convert.ToInt32(item2["MaNguoiQL"].ToString());
                                                nhom2.Ten = item2["Ten"].ToString();
                                                nhom2.ChuoiMaCha = item2["ChuoiMaCha"].ToString();
                                                nhom2.TenQL = item2["TenQL"].ToString();
                                                lstTemp.Add(nhom2);
                                            }
                                            result.AddRange(TaoCayDSNhom(lstTemp, resultTemp[i].ChuoiMaCha + "." + resultTemp[i].ID.ToString()));
                                        }
                                    }
                                }
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

        public string LayChuoiMaCha(int maNhom)
        {
            try
            {
                using (var session = LOANSessionManager.OpenSession())
                {
                    IDbCommand command = new SqlCommand();
                    command.Connection = session.Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "sp_NHOM_LayChuoiMaChaCuaMaNhom";
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@MaNhom", maNhom));
                    var dt = new DataTable();
                    dt.Load(command.ExecuteReader());
                    if (dt == null)
                        return "";
                    return dt.Rows[0]["ChuoiMaCha"].ToString();
                }
            }
            catch (BusinessException ex)
            {
                throw ex;
            }
        }

        public bool checkIsTeamLeadByUserId(int userId)
        {
            try
            {
                using (ISession session = LOANSessionManager.OpenSession())
                {
                    IDbCommand command = new SqlCommand();
                    command.Connection = session.Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "sp_CheckIsTeamlead";
                    command.Parameters.Add(new SqlParameter("@userId", userId));
                    DataTable dt = new DataTable();
                    dt.Load(command.ExecuteReader());
                    if (dt == null || dt.Rows == null || dt.Rows.Count == 0)
                        return false;
                    var result = Convert.ToBoolean(dt.Rows[0]["isTeamLead"]);
                    return result;
                    //if (dt != null)
                    //{
                    //    if (dt.Rows.Count > 0)
                    //    {
                    //        List<NhomDropDownModel> result = new List<NhomDropDownModel>();
                    //        foreach (DataRow item in dt.Rows)
                    //        {
                    //            NhomDropDownModel nhom = new NhomDropDownModel();
                    //            nhom.ID = Convert.ToInt32(item["ID"].ToString());
                    //            nhom.Ten = item["Ten"].ToString();
                    //            result.Add(nhom);
                    //        }
                    //        return result;
                    //    }
                    //}
                    //return null;
                }
            }
            catch (BusinessException ex)
            {
                throw ex;
            }
        }

    }
}
