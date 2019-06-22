using VS_LOAN.Core.Nhibernate;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Utility.Exceptions;
using VS_LOAN.Core.Entity.Model;
namespace VS_LOAN.Core.Business
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
        public bool UpdateNhanvienQuyen(NhanvienQuyenModel model)
        {
            if (model == null)
                return false;
            if (model.Ma_NV <= 0 || string.IsNullOrWhiteSpace(model.Quyen))
                return false;
            using (var session = LOANSessionManager.OpenSession())
            {
                try
                {
                    IDbCommand commandUpDate = new SqlCommand();
                    commandUpDate.Connection = session.Connection;
                    commandUpDate.CommandType = CommandType.StoredProcedure;
                    commandUpDate.CommandText = "sp_Update_NhanvienQuyen";
                    commandUpDate.Parameters.Add(new SqlParameter("@userId", model.Ma_NV));
                    commandUpDate.Parameters.Add(new SqlParameter("@maquyen", model.Quyen));
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
        public bool InsertNhanvienQuyen(NhanvienQuyenModel model)
        {
            if (model == null)
                return false;
            if (model.Ma_NV <= 0 || string.IsNullOrWhiteSpace(model.Quyen))
                return false;
            using (var session = LOANSessionManager.OpenSession())
            {
                try
                {
                    IDbCommand commandUpDate = new SqlCommand();
                    commandUpDate.Connection = session.Connection;
                    commandUpDate.CommandType = CommandType.StoredProcedure;
                    commandUpDate.CommandText = "sp_Create_NhanvienQuyen";
                    commandUpDate.Parameters.Add(new SqlParameter("@userId", model.Ma_NV));
                    commandUpDate.Parameters.Add(new SqlParameter("@maquyen", model.Quyen));
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
        public bool DeleteNhanvienQuyen(int userId)
        {
            if (userId <= 0)
                return false;
            using (var session = LOANSessionManager.OpenSession())
            {
                try
                {
                    IDbCommand commandUpDate = new SqlCommand();
                    commandUpDate.Connection = session.Connection;
                    commandUpDate.CommandType = CommandType.StoredProcedure;
                    commandUpDate.CommandText = "sp_Remove_NhanvienQuyen";
                    commandUpDate.Parameters.Add(new SqlParameter("@userId", userId));
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

        
        public NhanvienQuyenModel GetNhanvienQuyenByUserId(int userId)
        {
            try
            {
                using (var session = LOANSessionManager.OpenSession())
                {
                    IDbCommand command = new SqlCommand();
                    command.Connection = session.Connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "select top(1) * from NHAN_VIEN_QUYEN where Ma_NV = @userId";
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@userId", userId));
                    DataTable dt = new DataTable();
                    NhanvienQuyenModel model = new NhanvienQuyenModel();
                    dt.Load(command.ExecuteReader());
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow item in dt.Rows)
                            {
                                model.Ma_NV = Convert.ToInt32(item["Ma_NV"].ToString());
                                model.Quyen = item["Quyen"].ToString();
                                return model;
                            }
                        }
                        return null;
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
