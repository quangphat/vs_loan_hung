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
using VS_LOAN.Core.Utility;
using VS_LOAN.Core.Utility.Exceptions;

namespace VS_LOAN.Core.Business
{
    public class HoSoBLL
    {
        private const int Limit_Max_Page = 100;
        public int Them(HoSoModel hoSoModel, List<TaiLieuModel> lstTaiLieu, ref bool isCheckMaSanPham)
        {
            try
            {
                using (var session = LOANSessionManager.OpenSession())
                using (var transaction = session.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        if (hoSoModel.MaTrangThai == (int)TrangThaiHoSo.NhapLieu || hoSoModel.MaTrangThai == (int)TrangThaiHoSo.BoSungHoSo
                            || hoSoModel.MaTrangThai == (int)TrangThaiHoSo.GiaiNgan || hoSoModel.MaTrangThai == (int)TrangThaiHoSo.ThamDinh
                            || hoSoModel.MaTrangThai == (int)TrangThaiHoSo.TuChoi || hoSoModel.MaTrangThai == (int)TrangThaiHoSo.Nhap)
                        {

                            IDbCommand commandCheckMaSanPham = new SqlCommand();
                            commandCheckMaSanPham.Connection = session.Connection;
                            commandCheckMaSanPham.CommandType = CommandType.StoredProcedure;
                            commandCheckMaSanPham.CommandText = "sp_SAN_PHAM_VAY_CheckExist";
                            commandCheckMaSanPham.Parameters.Clear();
                            session.Transaction.Enlist(commandCheckMaSanPham);
                            commandCheckMaSanPham.Parameters.Add(new SqlParameter("@SanPhamVay", hoSoModel.SanPhamVay));
                            commandCheckMaSanPham.Parameters.Add(new SqlParameter("@ID", hoSoModel.ID));
                            var paraExist = new SqlParameter("@Exist", SqlDbType.Int);
                            paraExist.Direction = ParameterDirection.Output;
                            commandCheckMaSanPham.Parameters.Add(paraExist);
                            commandCheckMaSanPham.ExecuteNonQuery();
                            if (((int)paraExist.Value) > 0)
                            {
                                isCheckMaSanPham = true;
                                transaction.Rollback();
                                return 0;
                            }
                            else
                            {
                                IDbCommand commandCapNhatMaSanPham = new SqlCommand();
                                commandCapNhatMaSanPham.Connection = session.Connection;
                                commandCapNhatMaSanPham.CommandType = CommandType.StoredProcedure;
                                commandCapNhatMaSanPham.CommandText = "sp_SAN_PHAM_VAY_CapNhatSuDung";
                                commandCapNhatMaSanPham.Parameters.Clear();
                                session.Transaction.Enlist(commandCapNhatMaSanPham);
                                commandCapNhatMaSanPham.Parameters.Clear();
                                commandCapNhatMaSanPham.Parameters.Add(new SqlParameter("@ID", hoSoModel.ID));
                                commandCapNhatMaSanPham.Parameters.Add(new SqlParameter("@SanPhamVay", hoSoModel.SanPhamVay));
                                int result = commandCapNhatMaSanPham.ExecuteNonQuery();

                            }
                        }
                        IDbCommand commandGetAutoID = new SqlCommand();
                        commandGetAutoID.Connection = session.Connection;
                        commandGetAutoID.CommandType = CommandType.StoredProcedure;
                        commandGetAutoID.CommandText = "sp_AUTOID_GetID";
                        commandGetAutoID.Parameters.Clear();
                        session.Transaction.Enlist(commandGetAutoID);
                        commandGetAutoID.Parameters.Add(new SqlParameter("@ID", (int)AutoID.HoSo));
                        DataTable dt = new DataTable();
                        dt.Load(commandGetAutoID.ExecuteReader());
                        if (dt != null)
                        {
                            if (dt.Rows.Count > 0)
                            {
                                AutoIDModel autoModel = new AutoIDModel()
                                {
                                    ID = Convert.ToInt32(dt.Rows[0]["ID"].ToString()),
                                    NameID = dt.Rows[0]["NameID"].ToString(),
                                    Prefix = dt.Rows[0]["Prefix"].ToString(),
                                    Suffixes = dt.Rows[0]["Suffixes"].ToString(),
                                    Value = Convert.ToInt32(dt.Rows[0]["Value"].ToString())
                                };
                                hoSoModel.MaHoSo = new AutoIDBLL().GetAutoID(ref autoModel);
                                IDbCommand commandAdd = new SqlCommand();
                                commandAdd.Connection = session.Connection;
                                commandAdd.CommandType = CommandType.StoredProcedure;
                                commandAdd.CommandText = "sp_HO_SO_Them";
                                session.Transaction.Enlist(commandAdd);
                                commandAdd.Parameters.Add(new SqlParameter("@ID", SqlDbType.BigInt) { Direction = ParameterDirection.Output });
                                commandAdd.Parameters.Add(new SqlParameter("@MaHoSo", hoSoModel.MaHoSo));
                                commandAdd.Parameters.Add(new SqlParameter("@CourierCode", hoSoModel.CourierCode));
                                commandAdd.Parameters.Add(new SqlParameter("@TenKhachHang", hoSoModel.TenKhachHang));
                                commandAdd.Parameters.Add(new SqlParameter("@CMND", hoSoModel.CMND));
                                commandAdd.Parameters.Add(new SqlParameter("@DiaChi", hoSoModel.DiaChi));
                                commandAdd.Parameters.Add(new SqlParameter("@MaKhuVuc", hoSoModel.MaKhuVuc));
                                commandAdd.Parameters.Add(new SqlParameter("@SDT", hoSoModel.SDT));
                                commandAdd.Parameters.Add(new SqlParameter("@SDT2", hoSoModel.SDT2));
                                commandAdd.Parameters.Add(new SqlParameter("@GioiTinh", hoSoModel.GioiTinh));
                                commandAdd.Parameters.Add(new SqlParameter("@NgayTao", hoSoModel.NgayTao));
                                commandAdd.Parameters.Add(new SqlParameter("@MaNguoiTao", hoSoModel.MaNguoiTao));
                                commandAdd.Parameters.Add(new SqlParameter("@HoSoCuaAi", hoSoModel.HoSoCuaAi));
                                if (hoSoModel.MaKetQua == 0)
                                {
                                    commandAdd.Parameters.Add(new SqlParameter("@KetQuaHS", DBNull.Value));
                                }
                                else
                                {
                                    commandAdd.Parameters.Add(new SqlParameter("@KetQuaHS", hoSoModel.MaKetQua));
                                }
                                if (hoSoModel.NgayNhanDon == DateTime.MinValue)
                                    commandAdd.Parameters.Add(new SqlParameter("@NgayNhanDon", DBNull.Value));
                                else
                                    commandAdd.Parameters.Add(new SqlParameter("@NgayNhanDon", hoSoModel.NgayNhanDon));
                                commandAdd.Parameters.Add(new SqlParameter("@MaTrangThai", hoSoModel.MaTrangThai));
                                commandAdd.Parameters.Add(new SqlParameter("@SanPhamVay", hoSoModel.SanPhamVay));
                                commandAdd.Parameters.Add(new SqlParameter("@CoBaoHiem", hoSoModel.CoBaoHiem));
                                commandAdd.Parameters.Add(new SqlParameter("@SoTienVay", hoSoModel.SoTienVay));
                                commandAdd.Parameters.Add(new SqlParameter("@HanVay", hoSoModel.HanVay));
                                commandAdd.Parameters.Add(new SqlParameter("@TenCuaHang", hoSoModel.TenCuaHang));
                                commandAdd.ExecuteNonQuery();
                                int idHoSo = Convert.ToInt32(((SqlParameter)commandAdd.Parameters["@ID"]).Value.ToString());

                                if (idHoSo > 0)
                                {
                                    IDbCommand commandUpdateAutoID = new SqlCommand();
                                    commandUpdateAutoID.Connection = session.Connection;
                                    commandUpdateAutoID.CommandType = CommandType.StoredProcedure;
                                    commandUpdateAutoID.CommandText = "sp_AUTOID_Update";
                                    commandUpdateAutoID.Parameters.Clear();
                                    session.Transaction.Enlist(commandUpdateAutoID);
                                    commandUpdateAutoID.Parameters.Clear();
                                    commandUpdateAutoID.Parameters.Add(new SqlParameter("@ID", autoModel.ID));
                                    commandUpdateAutoID.Parameters.Add(new SqlParameter("@Prefix", autoModel.Prefix));
                                    commandUpdateAutoID.Parameters.Add(new SqlParameter("@Suffixes", autoModel.Suffixes));
                                    commandUpdateAutoID.Parameters.Add(new SqlParameter("@Value", autoModel.Value));
                                    int result = commandUpdateAutoID.ExecuteNonQuery();
                                    if (lstTaiLieu != null)
                                    {
                                        IDbCommand commandTaiLieuThem = new SqlCommand();
                                        commandTaiLieuThem.Connection = session.Connection;
                                        commandTaiLieuThem.CommandType = CommandType.StoredProcedure;
                                        commandTaiLieuThem.CommandText = "sp_TAI_LIEU_HS_Them";
                                        commandTaiLieuThem.Parameters.Clear();
                                        session.Transaction.Enlist(commandTaiLieuThem);
                                        commandTaiLieuThem.Parameters.Clear();
                                        foreach (var item in lstTaiLieu)
                                        {
                                            commandTaiLieuThem.Parameters.Clear();
                                            commandTaiLieuThem.Parameters.Add(new SqlParameter("@Maloai", item.MaLoai));
                                            commandTaiLieuThem.Parameters.Add(new SqlParameter("@DuongDan", item.LstFile[0].DuongDan));
                                            commandTaiLieuThem.Parameters.Add(new SqlParameter("@Ten", item.LstFile[0].Ten));
                                            commandTaiLieuThem.Parameters.Add(new SqlParameter("@MaHS", idHoSo));
                                            commandTaiLieuThem.ExecuteNonQuery();
                                        }
                                    }

                                    transaction.Commit();
                                    return idHoSo;
                                }
                            }
                        }

                        return 0;
                    }
                    catch (BusinessException ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }

            }
            catch (BusinessException ex)
            {
                throw ex;
            }
        }
        public bool Luu(HoSoModel hoSoModel, List<TaiLieuModel> lstTaiLieu, ref bool isCheckMaSanPham)
        {
            try
            {
                using (var session = LOANSessionManager.OpenSession())
                using (var transaction = session.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        if (hoSoModel.MaTrangThai == (int)TrangThaiHoSo.NhapLieu || hoSoModel.MaTrangThai == (int)TrangThaiHoSo.BoSungHoSo
                               || hoSoModel.MaTrangThai == (int)TrangThaiHoSo.GiaiNgan || hoSoModel.MaTrangThai == (int)TrangThaiHoSo.ThamDinh
                               || hoSoModel.MaTrangThai == (int)TrangThaiHoSo.TuChoi || hoSoModel.MaTrangThai == (int)TrangThaiHoSo.Nhap)
                        {

                            IDbCommand commandCheckMaSanPham = new SqlCommand();
                            commandCheckMaSanPham.Connection = session.Connection;
                            commandCheckMaSanPham.CommandType = CommandType.StoredProcedure;
                            commandCheckMaSanPham.CommandText = "sp_SAN_PHAM_VAY_CheckExist";
                            commandCheckMaSanPham.Parameters.Clear();
                            session.Transaction.Enlist(commandCheckMaSanPham);
                            commandCheckMaSanPham.Parameters.Add(new SqlParameter("@SanPhamVay", hoSoModel.SanPhamVay));
                            commandCheckMaSanPham.Parameters.Add(new SqlParameter("@ID", hoSoModel.ID));
                            var paraExist = new SqlParameter("@Exist", SqlDbType.Int);
                            paraExist.Direction = ParameterDirection.Output;
                            commandCheckMaSanPham.Parameters.Add(paraExist);
                            commandCheckMaSanPham.ExecuteNonQuery();
                            if (((int)paraExist.Value) > 0)
                            {
                                isCheckMaSanPham = true;
                                transaction.Rollback();
                                return false;
                            }
                            else
                            {
                                IDbCommand commandCapNhatMaSanPham = new SqlCommand();
                                commandCapNhatMaSanPham.Connection = session.Connection;
                                commandCapNhatMaSanPham.CommandType = CommandType.StoredProcedure;
                                commandCapNhatMaSanPham.CommandText = "sp_SAN_PHAM_VAY_CapNhatSuDung";
                                commandCapNhatMaSanPham.Parameters.Clear();
                                session.Transaction.Enlist(commandCapNhatMaSanPham);
                                commandCapNhatMaSanPham.Parameters.Clear();
                                commandCapNhatMaSanPham.Parameters.Add(new SqlParameter("@ID", hoSoModel.ID));
                                commandCapNhatMaSanPham.Parameters.Add(new SqlParameter("@SanPhamVay", hoSoModel.SanPhamVay));
                                int result = commandCapNhatMaSanPham.ExecuteNonQuery();

                            }
                        }
                        IDbCommand command = new SqlCommand();
                        command.Connection = session.Connection;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "sp_HO_SO_CapNhat";
                        session.Transaction.Enlist(command);
                        command.Parameters.Add(new SqlParameter("@ID", hoSoModel.ID));
                        command.Parameters.Add(new SqlParameter("@CourierCode", hoSoModel.CourierCode));
                        command.Parameters.Add(new SqlParameter("@TenKhachHang", hoSoModel.TenKhachHang));
                        command.Parameters.Add(new SqlParameter("@CMND", hoSoModel.CMND));
                        command.Parameters.Add(new SqlParameter("@DiaChi", hoSoModel.DiaChi));
                        command.Parameters.Add(new SqlParameter("@MaKhuVuc", hoSoModel.MaKhuVuc));
                        command.Parameters.Add(new SqlParameter("@SDT", hoSoModel.SDT));
                        command.Parameters.Add(new SqlParameter("@SDT2", hoSoModel.SDT2));
                        command.Parameters.Add(new SqlParameter("@GioiTinh", hoSoModel.GioiTinh));
                        command.Parameters.Add(new SqlParameter("@NgayTao", hoSoModel.NgayTao));
                        command.Parameters.Add(new SqlParameter("@MaNguoiTao", hoSoModel.MaNguoiTao));
                        command.Parameters.Add(new SqlParameter("@HoSoCuaAi", hoSoModel.HoSoCuaAi));
                        if (hoSoModel.MaKetQua == 0)
                        {
                            command.Parameters.Add(new SqlParameter("@KetQuaHS", DBNull.Value));
                        }
                        else
                        {
                            command.Parameters.Add(new SqlParameter("@KetQuaHS", hoSoModel.MaKetQua));
                        }
                        if (hoSoModel.NgayNhanDon == DateTime.MinValue)
                            command.Parameters.Add(new SqlParameter("@NgayNhanDon", DBNull.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@NgayNhanDon", hoSoModel.NgayNhanDon));
                        command.Parameters.Add(new SqlParameter("@MaTrangThai", hoSoModel.MaTrangThai));
                        command.Parameters.Add(new SqlParameter("@SanPhamVay", hoSoModel.SanPhamVay));
                        command.Parameters.Add(new SqlParameter("@CoBaoHiem", hoSoModel.CoBaoHiem));
                        command.Parameters.Add(new SqlParameter("@SoTienVay", hoSoModel.SoTienVay));
                        command.Parameters.Add(new SqlParameter("@HanVay", hoSoModel.HanVay));
                        command.Parameters.Add(new SqlParameter("@TenCuaHang", hoSoModel.TenCuaHang));
                        int rs = command.ExecuteNonQuery();
                        if (rs > 0)
                        {
                            IDbCommand commandTaiLieuXoaTatCa = new SqlCommand();
                            commandTaiLieuXoaTatCa.Connection = session.Connection;
                            commandTaiLieuXoaTatCa.CommandType = CommandType.StoredProcedure;
                            commandTaiLieuXoaTatCa.CommandText = "sp_TAI_LIEU_HS_XoaTatCa";
                            session.Transaction.Enlist(commandTaiLieuXoaTatCa);
                            commandTaiLieuXoaTatCa.Parameters.Clear();
                            commandTaiLieuXoaTatCa.Parameters.Add(new SqlParameter("@MaHS", hoSoModel.ID));
                            commandTaiLieuXoaTatCa.ExecuteNonQuery();
                            if (lstTaiLieu != null)
                            {
                                IDbCommand commandTaiLieuThem = new SqlCommand();
                                commandTaiLieuThem.Connection = session.Connection;
                                commandTaiLieuThem.CommandType = CommandType.StoredProcedure;
                                commandTaiLieuThem.CommandText = "sp_TAI_LIEU_HS_Them";
                                session.Transaction.Enlist(commandTaiLieuThem);
                                commandTaiLieuThem.Parameters.Clear();
                                foreach (var item in lstTaiLieu)
                                {
                                    commandTaiLieuThem.Parameters.Clear();
                                    commandTaiLieuThem.Parameters.Add(new SqlParameter("@Maloai", item.MaLoai));
                                    commandTaiLieuThem.Parameters.Add(new SqlParameter("@DuongDan", item.LstFile[0].DuongDan));
                                    commandTaiLieuThem.Parameters.Add(new SqlParameter("@Ten", item.LstFile[0].Ten));
                                    commandTaiLieuThem.Parameters.Add(new SqlParameter("@MaHS", hoSoModel.ID));
                                    commandTaiLieuThem.ExecuteNonQuery();
                                }
                            }
                            transaction.Commit();
                            return true;
                        }
                        return false;
                    }
                    catch (BusinessException ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }

            }
            catch (BusinessException ex)
            {
                throw ex;
            }
        }
        public bool CapNhatHoSo(HoSoModel hoSoModel, List<TaiLieuModel> lstTaiLieu, ref bool isCheckMaSanPham)
        {
            try
            {
                using (var session = LOANSessionManager.OpenSession())
                using (var transaction = session.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        if (hoSoModel.MaTrangThai == (int)TrangThaiHoSo.NhapLieu || hoSoModel.MaTrangThai == (int)TrangThaiHoSo.BoSungHoSo
                            || hoSoModel.MaTrangThai == (int)TrangThaiHoSo.GiaiNgan || hoSoModel.MaTrangThai == (int)TrangThaiHoSo.ThamDinh
                            || hoSoModel.MaTrangThai == (int)TrangThaiHoSo.TuChoi || hoSoModel.MaTrangThai == (int)TrangThaiHoSo.Nhap)
                        {

                            IDbCommand commandCheckMaSanPham = new SqlCommand();
                            commandCheckMaSanPham.Connection = session.Connection;
                            commandCheckMaSanPham.CommandType = CommandType.StoredProcedure;
                            commandCheckMaSanPham.CommandText = "sp_SAN_PHAM_VAY_CheckExist";
                            commandCheckMaSanPham.Parameters.Clear();
                            session.Transaction.Enlist(commandCheckMaSanPham);
                            commandCheckMaSanPham.Parameters.Add(new SqlParameter("@SanPhamVay", hoSoModel.SanPhamVay));
                            commandCheckMaSanPham.Parameters.Add(new SqlParameter("@ID", hoSoModel.ID));
                            var paraExist = new SqlParameter("@Exist", SqlDbType.Int);
                            paraExist.Direction = ParameterDirection.Output;
                            commandCheckMaSanPham.Parameters.Add(paraExist);
                            commandCheckMaSanPham.ExecuteNonQuery();
                            if (((int)paraExist.Value) > 0)
                            {
                                isCheckMaSanPham = true;
                                transaction.Rollback();
                                return false;
                            }
                            else
                            {
                                IDbCommand commandCapNhatMaSanPham = new SqlCommand();
                                commandCapNhatMaSanPham.Connection = session.Connection;
                                commandCapNhatMaSanPham.CommandType = CommandType.StoredProcedure;
                                commandCapNhatMaSanPham.CommandText = "sp_SAN_PHAM_VAY_CapNhatSuDung";
                                commandCapNhatMaSanPham.Parameters.Clear();
                                session.Transaction.Enlist(commandCapNhatMaSanPham);
                                commandCapNhatMaSanPham.Parameters.Clear();
                                commandCapNhatMaSanPham.Parameters.Add(new SqlParameter("@ID", hoSoModel.ID));
                                commandCapNhatMaSanPham.Parameters.Add(new SqlParameter("@SanPhamVay", hoSoModel.SanPhamVay));
                                int result = commandCapNhatMaSanPham.ExecuteNonQuery();

                            }
                        }
                        IDbCommand command = new SqlCommand();
                        command.Connection = session.Connection;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "sp_HO_SO_CapNhatHoSo";
                        session.Transaction.Enlist(command);
                        command.Parameters.Add(new SqlParameter("@ID", hoSoModel.ID));
                        command.Parameters.Add(new SqlParameter("@CourierCode", hoSoModel.CourierCode));
                        command.Parameters.Add(new SqlParameter("@TenKhachHang", hoSoModel.TenKhachHang));
                        command.Parameters.Add(new SqlParameter("@CMND", hoSoModel.CMND));
                        command.Parameters.Add(new SqlParameter("@DiaChi", hoSoModel.DiaChi));
                        command.Parameters.Add(new SqlParameter("@MaKhuVuc", hoSoModel.MaKhuVuc));
                        command.Parameters.Add(new SqlParameter("@SDT", hoSoModel.SDT));
                        command.Parameters.Add(new SqlParameter("@SDT2", hoSoModel.SDT2));
                        command.Parameters.Add(new SqlParameter("@GioiTinh", hoSoModel.GioiTinh));
                        command.Parameters.Add(new SqlParameter("@NgayCapNhat", hoSoModel.NgayCapNhat));
                        command.Parameters.Add(new SqlParameter("@MaNguoiCapNhat", hoSoModel.MaNguoiCapNhat));
                        command.Parameters.Add(new SqlParameter("@HoSoCuaAi", hoSoModel.HoSoCuaAi));
                        //command.Parameters.Add(new SqlParameter("@MaTrangThai", hoSoModel.MaTrangThai));
                        if (hoSoModel.MaKetQua == 0)
                        {
                            command.Parameters.Add(new SqlParameter("@KetQuaHS", DBNull.Value));
                        }
                        else
                        {
                            command.Parameters.Add(new SqlParameter("@KetQuaHS", hoSoModel.MaKetQua));
                        }
                        if (hoSoModel.NgayNhanDon == DateTime.MinValue)
                            command.Parameters.Add(new SqlParameter("@NgayNhanDon", DBNull.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@NgayNhanDon", hoSoModel.NgayNhanDon));
                        command.Parameters.Add(new SqlParameter("@MaTrangThai", hoSoModel.MaTrangThai));
                        command.Parameters.Add(new SqlParameter("@SanPhamVay", hoSoModel.SanPhamVay));
                        command.Parameters.Add(new SqlParameter("@CoBaoHiem", hoSoModel.CoBaoHiem));
                        command.Parameters.Add(new SqlParameter("@SoTienVay", hoSoModel.SoTienVay));
                        command.Parameters.Add(new SqlParameter("@HanVay", hoSoModel.HanVay));
                        command.Parameters.Add(new SqlParameter("@TenCuaHang", hoSoModel.TenCuaHang));
                        int rs = command.ExecuteNonQuery();
                        if (rs > 0)
                        {
                            IDbCommand commandTaiLieuXoaTatCa = new SqlCommand();
                            commandTaiLieuXoaTatCa.Connection = session.Connection;
                            commandTaiLieuXoaTatCa.CommandType = CommandType.StoredProcedure;
                            commandTaiLieuXoaTatCa.CommandText = "sp_TAI_LIEU_HS_XoaTatCa";
                            session.Transaction.Enlist(commandTaiLieuXoaTatCa);
                            commandTaiLieuXoaTatCa.Parameters.Clear();
                            commandTaiLieuXoaTatCa.Parameters.Add(new SqlParameter("@MaHS", hoSoModel.ID));
                            commandTaiLieuXoaTatCa.ExecuteNonQuery();
                            if (lstTaiLieu != null)
                            {
                                IDbCommand commandTaiLieuThem = new SqlCommand();
                                commandTaiLieuThem.Connection = session.Connection;
                                commandTaiLieuThem.CommandType = CommandType.StoredProcedure;
                                commandTaiLieuThem.CommandText = "sp_TAI_LIEU_HS_Them";
                                session.Transaction.Enlist(commandTaiLieuThem);
                                commandTaiLieuThem.Parameters.Clear();
                                foreach (var item in lstTaiLieu)
                                {
                                    commandTaiLieuThem.Parameters.Clear();
                                    commandTaiLieuThem.Parameters.Add(new SqlParameter("@Maloai", item.MaLoai));
                                    commandTaiLieuThem.Parameters.Add(new SqlParameter("@DuongDan", item.LstFile[0].DuongDan));
                                    commandTaiLieuThem.Parameters.Add(new SqlParameter("@Ten", item.LstFile[0].Ten));
                                    commandTaiLieuThem.Parameters.Add(new SqlParameter("@MaHS", hoSoModel.ID));
                                    commandTaiLieuThem.ExecuteNonQuery();
                                }
                            }
                            transaction.Commit();
                            return true;
                        }
                        return false;
                    }
                    catch (BusinessException ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }

            }
            catch (BusinessException ex)
            {
                throw ex;
            }
        }
        public bool AddGhichu(GhichuModel model)
        {
            if (model == null)
                return false;
            if (model.UserId <= 0 || model.HosoId <= 0)
                return false;
            if (string.IsNullOrWhiteSpace(model.Noidung) || model.Noidung.Length > 200)
                return false;
            try
            {
                using (var session = LOANSessionManager.OpenSession())
                {
                    using (var transaction = session.BeginTransaction(IsolationLevel.ReadCommitted))
                    {
                        IDbCommand command = new SqlCommand();
                        command.Connection = session.Connection;
                        command.CommandType = CommandType.Text;
                        command.CommandText = "insert into Ghichu (UserId,Noidung,HosoId, CommentTime) values(@UserId,@Noidung,@HosoId,@CommentTime)";
                        session.Transaction.Enlist(command);
                        command.Parameters.Clear();
                        command.Parameters.Add(new SqlParameter("@UserId", model.UserId));
                        command.Parameters.Add(new SqlParameter("@HosoId", model.HosoId));
                        command.Parameters.Add(new SqlParameter("@Noidung", model.Noidung));
                        command.Parameters.Add(new SqlParameter("@CommentTime", model.CommentTime));
                        command.ExecuteNonQuery();
                        transaction.Commit();
                        return true;
                    }

                }
            }
            catch
            {
                return false;
            }
        }
        public List<GhichuViewModel> LayDanhsachGhichu(int hosoId)
        {
            if (hosoId <= 0)
                return new List<GhichuViewModel>();
            try
            {
                using (var session = LOANSessionManager.OpenSession())
                {
                    IDbCommand command = new SqlCommand();
                    command.Connection = session.Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "sp_GetGhichuByHosoId";
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@hosoId", hosoId));
                    var dt = new DataTable();
                    dt.Load(command.ExecuteReader());
                    List<GhichuViewModel> lstGhichu = new List<GhichuViewModel>();
                    if (dt == null || dt.Rows == null || dt.Rows.Count == 0)
                        return lstGhichu;
                    GhichuViewModel ghichu;
                    foreach (DataRow item in dt.Rows)
                    {
                        ghichu = new GhichuViewModel();
                        ghichu.Id = Convert.ToInt32(item["Id"].ToString());
                        ghichu.UserId = Convert.ToInt32(item["UserId"].ToString());
                        ghichu.HosoId = Convert.ToInt32(item["HosoId"].ToString());
                        ghichu.Noidung = item["Noidung"].ToString();
                        ghichu.Commentator = item["Commentator"].ToString();
                        if (item["CommentTime"] != null && !string.IsNullOrEmpty(item["CommentTime"].ToString()))
                        {
                            ghichu.CommentTime = Convert.ToDateTime(item["CommentTime"]);
                        }
                        lstGhichu.Add(ghichu);
                    }
                    return lstGhichu;
                }
            }
            catch
            {
                return null;
            }
        }
        public HoSoInfoModel LayChiTiet(int id)
        {
            try
            {
                using (var session = LOANSessionManager.OpenSession())
                {
                    IDbCommand command = new SqlCommand();
                    command.Connection = session.Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "sp_HO_SO_LayChiTiet";
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@ID", id));
                    var dt = new DataTable();
                    dt.Load(command.ExecuteReader());
                    if (dt == null)
                        return null;
                    HoSoInfoModel hs = new HoSoInfoModel();
                    foreach (DataRow item in dt.Rows)
                    {
                        hs.ID = Convert.ToInt32(item["ID"].ToString());
                        hs.MaHoSo = item["MaHoSo"].ToString();
                        hs.TenKhachHang = item["TenKhachHang"].ToString();
                        hs.CMND = item["CMND"].ToString();
                        hs.DiaChi = item["DiaChi"].ToString();
                        hs.SDT = item["SDT"].ToString();
                        hs.SDT2 = item["SDT2"].ToString();
                        hs.GioiTinh = Convert.ToInt32(item["GioiTinh"].ToString());
                        hs.NgayTao = (DateTime)(item["NgayTao"]);
                        hs.MaNguoiTao = Convert.ToInt32(item["MaNguoiTao"].ToString());
                        hs.HoSoCuaAi = Convert.ToInt32(item["HoSoCuaAi"].ToString());
                        try
                        {
                            hs.MaKhuVuc = Convert.ToInt32(item["MaKhuVuc"].ToString());
                        }
                        catch
                        {

                        }
                        try
                        {
                            hs.CourierCode = Convert.ToInt32(item["CourierCode"].ToString());
                        }
                        catch
                        {

                        }
                        try
                        {
                            hs.NgayCapNhat = (DateTime)(item["NgayCapNhat"]);
                        }
                        catch
                        {

                        }
                        try
                        {
                            hs.MaNguoiCapNhat = Convert.ToInt32(item["MaNguoiCapNhat"].ToString());
                        }
                        catch
                        {

                        }
                        try
                        {
                            hs.NgayNhanDon = (DateTime)(item["NgayNhanDon"]);
                        }
                        catch
                        {

                        }
                        try
                        {
                            hs.MaTrangThai = Convert.ToInt32(item["MaTrangThai"].ToString());

                        }
                        catch
                        {

                        }

                        try
                        {
                            hs.MaKetQua = Convert.ToInt32(item["MaKetQua"].ToString());
                        }
                        catch
                        {

                        }
                        try
                        {
                            hs.SanPhamVay = Convert.ToInt32(item["SanPhamVay"].ToString());
                        }
                        catch
                        {

                        }

                        hs.TenCuaHang = item["TenCuaHang"].ToString();

                        if (Convert.ToBoolean(item["CoBaoHiem"].ToString()) == true)
                            hs.CoBaoHiem = 0;
                        else
                            hs.CoBaoHiem = 1;
                        try
                        {
                            hs.SoTienVay = Convert.ToInt32(item["SoTienVay"].ToString());
                        }
                        catch
                        {

                        }
                        try
                        {
                            hs.HanVay = Convert.ToInt32(item["HanVay"].ToString());
                        }
                        catch
                        {

                        }
                        hs.GhiChu = item["GhiChu"].ToString();
                        hs.TenTrangThai = item["TenTrangThai"].ToString();
                        hs.KetQuaText = item["KetQuaText"].ToString();
                        IDbCommand commandTaiLieu = new SqlCommand();
                        commandTaiLieu.Connection = session.Connection;
                        commandTaiLieu.CommandType = CommandType.StoredProcedure;
                        commandTaiLieu.CommandText = "sp_TAI_LIEU_HS_LayDS";
                        commandTaiLieu.Parameters.Clear();
                        commandTaiLieu.Parameters.Add(new SqlParameter("@MaHS", hs.ID));
                        var dtTaiLieu = new DataTable();
                        dtTaiLieu.Load(commandTaiLieu.ExecuteReader());
                        if (dtTaiLieu != null)
                        {
                            foreach (DataRow rowTaiLieu in dtTaiLieu.Rows)
                            {
                                TaiLieuModel taiLieu = new TaiLieuModel();
                                taiLieu.MaLoai = Convert.ToInt32(rowTaiLieu["MaLoai"].ToString());
                                taiLieu.LstFile.Add(new FileInfo()
                                {
                                    Ten = rowTaiLieu["Ten"].ToString(),
                                    DuongDan = rowTaiLieu["DuongDanFile"].ToString()
                                });
                                hs.LstTaiLieu.Add(taiLieu);
                            }
                        }

                    }
                    return hs;
                }
            }
            catch (BusinessException ex)
            {
                throw ex;
            }
        }

        public List<HoSoCuaToiModel> TimHoSoCuaToi(int maNV, DateTime tuNgay, DateTime denNgay, string maHS, string sdt, string trangthai)
        {
            try
            {
                using (var session = LOANSessionManager.OpenSession())
                {
                    IDbCommand command = new SqlCommand();
                    command.Connection = session.Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "sp_HO_SO_TimHoSoCuaToi";
                    command.Parameters.Add(new SqlParameter("@MaNhanVien", maNV));
                    command.Parameters.Add(new SqlParameter("@TuNgay", tuNgay));
                    command.Parameters.Add(new SqlParameter("@DenNgay", denNgay));
                    command.Parameters.Add(new SqlParameter("@MaHS", maHS));
                    command.Parameters.Add(new SqlParameter("@SDT", sdt));
                    command.Parameters.Add(new SqlParameter("@TrangThai", trangthai));
                    DataTable dt = new DataTable();
                    dt.Load(command.ExecuteReader());
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            List<HoSoCuaToiModel> result = new List<HoSoCuaToiModel>();
                            foreach (DataRow item in dt.Rows)
                            {
                                HoSoCuaToiModel hs = new HoSoCuaToiModel();
                                hs.ID = Convert.ToInt32(item["ID"].ToString());
                                hs.MaHoSo = item["MaHoSo"].ToString();
                                hs.NgayTao = Convert.ToDateTime(item["NgayTao"]);
                                hs.DoiTac = item["DoiTac"].ToString();
                                hs.CMND = item["CMND"].ToString();
                                hs.TenKH = item["TenKH"].ToString();
                                hs.TrangThaiHS = item["TrangThaiHS"].ToString();
                                hs.MaTrangThai = Convert.ToInt32(item["MaTrangThai"].ToString());
                                hs.KetQuaHS = item["KetQuaHS"].ToString();
                                try
                                {
                                    hs.NgayCapNhat = Convert.ToDateTime(item["NgayCapNhat"]);
                                }
                                catch
                                {
                                }
                                hs.NhanVienBanHang = item["NhanVienBanHang"].ToString();
                                hs.MaNV = item["MaNV"].ToString();
                                hs.MaNVSua = item["MaNVSua"].ToString(); ;
                                hs.CoBaoHiem = (bool)item["CoBaoHiem"];
                                hs.DiaChiKH = item["DiaChiKH"].ToString();
                                hs.KhuVucText = item["KhuVucText"].ToString();
                                hs.GhiChu = item["GhiChu"].ToString(); ;
                                result.Add(hs);
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
        public List<HoSoCuaToiModel> DSHoSoChuaXem(int maNV, DateTime tuNgay, DateTime denNgay, string maHS, string sdt, string trangthai)
        {
            try
            {
                using (var session = LOANSessionManager.OpenSession())
                {
                    IDbCommand command = new SqlCommand();
                    command.Connection = session.Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "sp_HO_SO_TimHoSoCuaToiChuaXem";
                    command.Parameters.Add(new SqlParameter("@MaNhanVien", maNV));
                    command.Parameters.Add(new SqlParameter("@TuNgay", tuNgay));
                    command.Parameters.Add(new SqlParameter("@DenNgay", denNgay));
                    command.Parameters.Add(new SqlParameter("@MaHS", maHS));
                    command.Parameters.Add(new SqlParameter("@SDT", sdt));
                    command.Parameters.Add(new SqlParameter("@TrangThai", trangthai));
                    DataTable dt = new DataTable();
                    dt.Load(command.ExecuteReader());
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            List<HoSoCuaToiModel> result = new List<HoSoCuaToiModel>();
                            foreach (DataRow item in dt.Rows)
                            {
                                HoSoCuaToiModel hs = new HoSoCuaToiModel();
                                hs.ID = Convert.ToInt32(item["ID"].ToString());
                                hs.MaHoSo = item["MaHoSo"].ToString();
                                hs.NgayTao = Convert.ToDateTime(item["NgayTao"]);
                                hs.DoiTac = item["DoiTac"].ToString();
                                hs.CMND = item["CMND"].ToString();
                                hs.TenKH = item["TenKH"].ToString();
                                hs.TrangThaiHS = item["TrangThaiHS"].ToString();
                                hs.MaTrangThai = Convert.ToInt32(item["MaTrangThai"].ToString());
                                hs.KetQuaHS = item["KetQuaHS"].ToString();
                                try
                                {
                                    hs.NgayCapNhat = Convert.ToDateTime(item["NgayCapNhat"]);
                                }
                                catch
                                {
                                }
                                hs.NhanVienBanHang = item["NhanVienBanHang"].ToString();
                                hs.MaNV = item["MaNV"].ToString();
                                hs.MaNVSua = item["MaNVSua"].ToString(); ;
                                hs.CoBaoHiem = (bool)item["CoBaoHiem"];
                                hs.DiaChiKH = item["DiaChiKH"].ToString();
                                hs.KhuVucText = item["KhuVucText"].ToString();
                                hs.GhiChu = item["GhiChu"].ToString(); ;
                                result.Add(hs);
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
        public bool XoaHS(int hsID, int nhanVienID, DateTime ngayThaoTac)
        {
            using (var session = LOANSessionManager.OpenSession())
            {
                try
                {
                    IDbCommand commandUpDate = new SqlCommand();
                    commandUpDate.Connection = session.Connection;
                    commandUpDate.CommandType = CommandType.StoredProcedure;
                    commandUpDate.CommandText = "sp_HO_SO_CapNhatDaXoa";
                    commandUpDate.Parameters.Add(new SqlParameter("@ID", hsID));
                    commandUpDate.Parameters.Add(new SqlParameter("@NhanVienID", nhanVienID));
                    commandUpDate.Parameters.Add(new SqlParameter("@NgayThaoTac", ngayThaoTac));
                    int result = commandUpDate.ExecuteNonQuery();
                    if (result > 0)
                        return true;
                    return false;
                }
                catch (BusinessException ex)
                {
                    throw ex;
                }
            }
        }

        public bool CapNhatTrangThaiHS(int hsID, int maNguoiThaoTac, DateTime ngayThaoTac, int maTrangThai, int maKetQua, string ghiChu)
        {
            using (var session = LOANSessionManager.OpenSession())
            {
                try
                {
                    IDbCommand commandUpDate = new SqlCommand();
                    commandUpDate.Connection = session.Connection;
                    commandUpDate.CommandType = CommandType.StoredProcedure;
                    commandUpDate.CommandText = "sp_HO_SO_CapNhatTrangThaiHS";
                    commandUpDate.Parameters.Add(new SqlParameter("@ID", hsID));
                    commandUpDate.Parameters.Add(new SqlParameter("@MaNguoiThaoTac", maNguoiThaoTac));
                    commandUpDate.Parameters.Add(new SqlParameter("@NgayThaoTac", ngayThaoTac));
                    commandUpDate.Parameters.Add(new SqlParameter("@MaTrangThai", maTrangThai));
                    commandUpDate.Parameters.Add(new SqlParameter("@MaKetQua", maKetQua));
                    commandUpDate.Parameters.Add(new SqlParameter("@GhiChu", ""));
                    int result = commandUpDate.ExecuteNonQuery();
                    if (result > 0)
                        return true;
                    return false;
                }
                catch (BusinessException ex)
                {
                    throw ex;
                }
            }
        }
        public int CountHosoDuyet(int maNVDangNhap,
            int maNhom,
            int maThanhVien,
            DateTime tuNgay,
            DateTime denNgay,
            string maHS,
            string cmnd,
            int loaiNgay,
            string trangThai,
            string freeText = null
            )
        {
            string query = string.IsNullOrWhiteSpace(freeText) ? string.Empty : freeText;
            int totalRecord = 0;
            try
            {
                using (var session = LOANSessionManager.OpenSession())
                {
                    IDbCommand command = new SqlCommand();
                    command.Connection = session.Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "sp_HO_SO_Count_TimHoSoDuyet";
                    command.Parameters.Add(new SqlParameter("@MaNVDangNhap", maNVDangNhap));
                    command.Parameters.Add(new SqlParameter("@MaNhom", maNhom));
                    command.Parameters.Add(new SqlParameter("@MaThanhVien", maThanhVien));
                    command.Parameters.Add(new SqlParameter("@TuNgay", tuNgay));
                    command.Parameters.Add(new SqlParameter("@DenNgay", denNgay));
                    command.Parameters.Add(new SqlParameter("@MaHS", maHS));
                    command.Parameters.Add(new SqlParameter("@CMND", cmnd));
                    command.Parameters.Add(new SqlParameter("@LoaiNgay", loaiNgay));
                    command.Parameters.Add(new SqlParameter("@TrangThai", trangThai));
                    command.Parameters.Add(new SqlParameter("@freeText", query));
                    DataTable dt = new DataTable();
                    dt.Load(command.ExecuteReader());
                    if (dt != null && dt.Rows != null && dt.Rows.Count > 0)
                    {
                        totalRecord = Convert.ToInt32(dt.Rows[0]["TotalRecord"].ToString());
                    }
                }
            }
            catch (Exception e)
            {
                totalRecord = 0;
            }
            return totalRecord;
        }
        public List<HoSoDuyetModel> TimHoSoDuyet(int maNVDangNhap,
            int maNhom,
            int maThanhVien,
            DateTime tuNgay,
            DateTime denNgay,
            string maHS,
            string cmnd,
            int loaiNgay,
            string trangThai,
            string freeText = null,
            int page = 1, int limit = 10, bool isDowload = false)
        {

            page = page <= 0 ? 1 : page;
            if(!isDowload)
                limit = (limit <= 0 || limit >= Limit_Max_Page) ? Limit_Max_Page : limit;
            int offset = (page - 1) * limit;
            string query = string.IsNullOrWhiteSpace(freeText) ? string.Empty : freeText;
            try
            {
                using (var session = LOANSessionManager.OpenSession())
                {
                    IDbCommand command = new SqlCommand();
                    command.Connection = session.Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "sp_HO_SO_TimHoSoDuyet";
                    command.Parameters.Add(new SqlParameter("@MaNVDangNhap", maNVDangNhap));
                    command.Parameters.Add(new SqlParameter("@MaNhom", maNhom));
                    command.Parameters.Add(new SqlParameter("@MaThanhVien", maThanhVien));
                    command.Parameters.Add(new SqlParameter("@TuNgay", tuNgay));
                    command.Parameters.Add(new SqlParameter("@DenNgay", denNgay));
                    command.Parameters.Add(new SqlParameter("@MaHS", maHS));
                    command.Parameters.Add(new SqlParameter("@CMND", cmnd));
                    command.Parameters.Add(new SqlParameter("@LoaiNgay", loaiNgay));
                    command.Parameters.Add(new SqlParameter("@TrangThai", trangThai));
                    command.Parameters.Add(new SqlParameter("@offset", offset));
                    command.Parameters.Add(new SqlParameter("@limit", limit));
                    command.Parameters.Add(new SqlParameter("@freeText", query));
                    DataTable dt = new DataTable();
                    dt.Load(command.ExecuteReader());
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            List<HoSoDuyetModel> result = new List<HoSoDuyetModel>();
                            foreach (DataRow item in dt.Rows)
                            {
                                HoSoDuyetModel hs = new HoSoDuyetModel();
                                hs.ID = Convert.ToInt32(item["ID"].ToString());
                                hs.MaHoSo = item["MaHoSo"].ToString();
                                hs.NgayTao = Convert.ToDateTime(item["NgayTao"]);
                                hs.DoiTac = item["DoiTac"].ToString();
                                hs.CMND = item["CMND"].ToString();
                                hs.TenKH = item["TenKH"].ToString();
                                hs.TrangThaiHS = item["TrangThaiHS"].ToString();
                                hs.KetQuaHS = item["KetQuaHS"].ToString();
                                hs.MaTrangThai = Convert.ToInt32(item["MaTrangThai"].ToString());
                                try
                                {
                                    hs.NgayCapNhat = Convert.ToDateTime(item["NgayCapNhat"]);
                                }
                                catch
                                {

                                }
                                hs.NhanVienBanHang = item["NhanVienBanHang"].ToString();
                                hs.MaNVLayHS = item["MaNVLayHS"].ToString();
                                hs.DoiNguBanHang = item["DoiNguBanHang"].ToString();
                                hs.MaNV = item["MaNV"].ToString();
                                hs.MaNVSua = item["MaNVSua"].ToString(); ;
                                hs.CoBaoHiem = (bool)item["CoBaoHiem"];
                                hs.DiaChiKH = item["DiaChiKH"].ToString();
                                hs.GhiChu = item["GhiChu"].ToString();
                                hs.TenSanPham = item["TenSanPham"].ToString();
                                result.Add(hs);
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
        public int CountHoSoQuanLy(int maNVDangNhap,
            int maNhom,
            int maThanhVien,
            DateTime tuNgay,
            DateTime denNgay,
            string maHS,
            string cmnd,
            string trangthai,
            int loaiNgay,
            string freeText = null
            )
        {
            string query = string.IsNullOrWhiteSpace(freeText) ? string.Empty : freeText;
            int totalRecord = 0;
            try
            {
                using (var session = LOANSessionManager.OpenSession())
                {
                    IDbCommand command = new SqlCommand();
                    command.Connection = session.Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "sp_HO_SO_Count_TimHoSoQuanLy";

                    command.Parameters.Add(new SqlParameter("@MaNVDangNhap", maNVDangNhap));
                    command.Parameters.Add(new SqlParameter("@MaNhom", maNhom));
                    command.Parameters.Add(new SqlParameter("@MaThanhVien", maThanhVien));
                    command.Parameters.Add(new SqlParameter("@TuNgay", tuNgay));
                    command.Parameters.Add(new SqlParameter("@DenNgay", denNgay));
                    command.Parameters.Add(new SqlParameter("@MaHS", maHS));
                    command.Parameters.Add(new SqlParameter("@CMND", cmnd));
                    command.Parameters.Add(new SqlParameter("@TrangThai", trangthai));
                    command.Parameters.Add(new SqlParameter("@LoaiNgay", loaiNgay));
                    command.Parameters.Add(new SqlParameter("@freeText", query));
                    DataTable dt = new DataTable();
                    dt.Load(command.ExecuteReader());
                    if (dt != null && dt.Rows != null && dt.Rows.Count > 0)
                    {
                        totalRecord = Convert.ToInt32(dt.Rows[0]["TotalRecord"].ToString());
                    }
                }
            }
            catch
            {
                totalRecord = 0;
            }
            return totalRecord;
        }
        public List<HoSoQuanLyModel> TimHoSoQuanLy(int maNVDangNhap,
                int maNhom,
                int maThanhVien,
                DateTime tuNgay,
                DateTime denNgay,
                string maHS,
                string cmnd,
                string trangthai,
                int loaiNgay,
                string freeText = null,
                int page = 1, int limit = 10,
                bool isDownload = false
                )
        {
            page = page <= 0 ? 1 : page;
            if(!isDownload)
                limit = (limit <= 0 || limit >= Limit_Max_Page) ? Limit_Max_Page : limit;
            int offset = (page - 1) * limit;
            string query = string.IsNullOrWhiteSpace(freeText) ? string.Empty : freeText;
            try
            {
                using (var session = LOANSessionManager.OpenSession())
                {
                    IDbCommand command = new SqlCommand();
                    command.Connection = session.Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "sp_HO_SO_TimHoSoQuanLy";

                    command.Parameters.Add(new SqlParameter("@MaNVDangNhap", maNVDangNhap));
                    command.Parameters.Add(new SqlParameter("@MaNhom", maNhom));
                    command.Parameters.Add(new SqlParameter("@MaThanhVien", maThanhVien));
                    command.Parameters.Add(new SqlParameter("@TuNgay", tuNgay));
                    command.Parameters.Add(new SqlParameter("@DenNgay", denNgay));
                    command.Parameters.Add(new SqlParameter("@MaHS", maHS));
                    command.Parameters.Add(new SqlParameter("@CMND", cmnd));
                    command.Parameters.Add(new SqlParameter("@TrangThai", trangthai));
                    command.Parameters.Add(new SqlParameter("@LoaiNgay", loaiNgay));
                    command.Parameters.Add(new SqlParameter("@offset", offset));
                    command.Parameters.Add(new SqlParameter("@limit", limit));
                    command.Parameters.Add(new SqlParameter("@freeText", query));
                    DataTable dt = new DataTable();
                    dt.Load(command.ExecuteReader());
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            List<HoSoQuanLyModel> result = new List<HoSoQuanLyModel>();
                            foreach (DataRow item in dt.Rows)
                            {
                                HoSoQuanLyModel hs = new HoSoQuanLyModel();
                                hs.ID = Convert.ToInt32(item["ID"].ToString());
                                hs.MaHoSo = item["MaHoSo"].ToString();
                                hs.NgayTao = Convert.ToDateTime(item["NgayTao"]);
                                hs.DoiTac = item["DoiTac"].ToString();
                                hs.CMND = item["CMND"].ToString();
                                hs.TenKH = item["TenKH"].ToString();
                                hs.TrangThaiHS = item["TrangThaiHS"].ToString();
                                hs.MaTrangThai = Convert.ToInt32(item["MaTrangThai"].ToString());
                                hs.KetQuaHS = item["KetQuaHS"].ToString();
                                try
                                {
                                    hs.NgayCapNhat = Convert.ToDateTime(item["NgayCapNhat"]);
                                }
                                catch
                                {

                                }
                                hs.NhanVienBanHang = item["NhanVienBanHang"].ToString();
                                hs.DoiNguBanHang = item["DoiNguBanHang"].ToString();
                                hs.MaNVLayHS = item["MaNVLayHS"].ToString();
                                hs.MaNV = item["MaNV"].ToString();
                                hs.MaNVSua = item["MaNVSua"].ToString();
                                hs.CoBaoHiem = (bool)item["CoBaoHiem"];
                                hs.DiaChiKH = item["DiaChiKH"].ToString();
                                hs.KhuVucText = item["KhuVucText"].ToString();
                                hs.GhiChu = item["GhiChu"].ToString();
                                hs.TenSanPham = item["TenSanPham"].ToString();
                                result.Add(hs);
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

        public List<HoSoDuyetModel> DSHoSoDuyetChuaXem(int maNVDangNhap, int maNhom, int maThanhVien, DateTime tuNgay, DateTime denNgay, string maHS, string cmnd, int loaiNgay, string trangThai)
        {
            try
            {
                using (var session = LOANSessionManager.OpenSession())
                {
                    IDbCommand command = new SqlCommand();
                    command.Connection = session.Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "sp_HO_SO_TimHoSoDuyetChuaXem";

                    command.Parameters.Add(new SqlParameter("@MaNVDangNhap", maNVDangNhap));
                    command.Parameters.Add(new SqlParameter("@MaNhom", maNhom));
                    command.Parameters.Add(new SqlParameter("@MaThanhVien", maThanhVien));
                    command.Parameters.Add(new SqlParameter("@TuNgay", tuNgay));
                    command.Parameters.Add(new SqlParameter("@DenNgay", denNgay));
                    command.Parameters.Add(new SqlParameter("@MaHS", maHS));
                    command.Parameters.Add(new SqlParameter("@CMND", cmnd));
                    command.Parameters.Add(new SqlParameter("@LoaiNgay", loaiNgay));
                    command.Parameters.Add(new SqlParameter("@TrangThai", trangThai));
                    DataTable dt = new DataTable();
                    dt.Load(command.ExecuteReader());
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            List<HoSoDuyetModel> result = new List<HoSoDuyetModel>();
                            foreach (DataRow item in dt.Rows)
                            {
                                HoSoDuyetModel hs = new HoSoDuyetModel();
                                hs.ID = Convert.ToInt32(item["ID"].ToString());
                                hs.MaHoSo = item["MaHoSo"].ToString();
                                hs.NgayTao = Convert.ToDateTime(item["NgayTao"]);
                                hs.DoiTac = item["DoiTac"].ToString();
                                hs.CMND = item["CMND"].ToString();
                                hs.TenKH = item["TenKH"].ToString();
                                hs.TrangThaiHS = item["TrangThaiHS"].ToString();
                                hs.KetQuaHS = item["KetQuaHS"].ToString();
                                hs.MaTrangThai = Convert.ToInt32(item["MaTrangThai"].ToString());
                                try
                                {
                                    hs.NgayCapNhat = Convert.ToDateTime(item["NgayCapNhat"]);
                                }
                                catch
                                {

                                }
                                hs.NhanVienBanHang = item["NhanVienBanHang"].ToString();
                                hs.MaNVLayHS = item["MaNVLayHS"].ToString();
                                hs.DoiNguBanHang = item["DoiNguBanHang"].ToString();
                                hs.MaNV = item["MaNV"].ToString();
                                hs.MaNVSua = item["MaNVSua"].ToString(); ;
                                hs.CoBaoHiem = (bool)item["CoBaoHiem"];
                                hs.DiaChiKH = item["DiaChiKH"].ToString(); ;
                                hs.GhiChu = item["GhiChu"].ToString(); ;
                                result.Add(hs);
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
