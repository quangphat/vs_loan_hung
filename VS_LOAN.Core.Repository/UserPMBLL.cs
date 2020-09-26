using VS_LOAN.Core.Entity.Model;
using VS_LOAN.Core.Nhibernate;
using VS_LOAN.Core.Utility;
using VS_LOAN.Core.Utility.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace VS_LOAN.Core.Repository
{
   public class UserPMBLL
    {
        public UserPMModel DangNhap(string user, string pass)
        {
            try
            {
                using (var session = LOANSessionManager.OpenSession())
                {
                    IDbCommand command = new SqlCommand();
                    command.Connection = session.Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "sp_Employee_Login";
                    command.Parameters.Add(new SqlParameter("@UserName", user));
                    command.Parameters.Add(new SqlParameter("@Password", pass));
                    var dt = new DataTable();
                    dt.Load(command.ExecuteReader());
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            UserPMModel userModel = new UserPMModel();
                            userModel.Code= dt.Rows[0]["Code"].ToString();
                            userModel.IDUser =Convert.ToInt32(dt.Rows[0]["ID"].ToString());
                            userModel.Email = dt.Rows[0]["Email"].ToString();
                            userModel.UserName = dt.Rows[0]["UserName"].ToString();
                            userModel.FullName = dt.Rows[0]["FullName"].ToString();
                            //userModel.FirstLogin =Convert.ToBoolean(dt.Rows[0]["FistLogin"].ToString());
                            userModel.OrgId = Convert.ToInt32(dt.Rows[0]["OrgId"].ToString());
                            userModel.RoleId = string.IsNullOrWhiteSpace(dt.Rows[0]["RoleId"].ToString()) ? 0 :  Convert.ToInt32(dt.Rows[0]["RoleId"].ToString());
                            return userModel;
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
        public bool ChangePass(string userID, string pass, bool firstLogin = false)
        {
            try
            {
                using (var session = LOANSessionManager.OpenSession())
                {
                    IDbCommand command = new SqlCommand();
                    command.Connection = session.Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "sp_NHANVIEN_ChangePass";
                    command.Parameters.Add(new SqlParameter("@ID", userID));
                    command.Parameters.Add(new SqlParameter("@Pass", pass));
                    if(firstLogin)
                    {
                        command.Parameters.Add(new SqlParameter("@changePassword", true));
                    }
                    command.ExecuteNonQuery();
                    return true;
                }
            }
            catch (BusinessException ex)
            {
                throw ex;
            }
        }



        public UserPMModel Get(string user)
        {
            try
            {
                using (var session = LOANSessionManager.OpenSession())
                {
                    IDbCommand command = new SqlCommand();
                    command.Connection = session.Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "sp_PD_USER_GetWidthUserName";
                    command.Parameters.Add(new SqlParameter("@UserName", user));
                    var dt = new DataTable();
                    dt.Load(command.ExecuteReader());
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            UserPMModel userModel = new UserPMModel();
                            userModel.IDUser = Convert.ToInt32(dt.Rows[0]["ID"].ToString());
                            userModel.Email = dt.Rows[0]["Email"].ToString();
                            userModel.UserName = dt.Rows[0]["UserName"].ToString();
                            userModel.FullName = dt.Rows[0]["FullName"].ToString();
                            userModel.TypeUser = Convert.ToInt32(dt.Rows[0]["Type"]);
                            return userModel;
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

        public UserPMModel GetUserByID(string userID)
        {
            try
            {
                using (var session = LOANSessionManager.OpenSession())
                {
                    IDbCommand command = new SqlCommand();
                    command.Connection = session.Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "sp_NHAN_VIEN_GetUserByID";
                    command.Parameters.Add(new SqlParameter("@UserID", userID));
                    var dt = new DataTable();
                    dt.Load(command.ExecuteReader());
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                UserPMModel userModel = new UserPMModel();
                                userModel.IDUser =Convert.ToInt32(dt.Rows[i]["ID"].ToString());
                                userModel.Email = dt.Rows[i]["Email"].ToString();
                                userModel.Code = dt.Rows[i]["Code"].ToString();
                                userModel.UserName = dt.Rows[i]["UserName"].ToString();
                                userModel.FullName = dt.Rows[i]["FullName"].ToString();
                                userModel.Phone = dt.Rows[i]["Phone"].ToString();
                                userModel.Password = dt.Rows[i]["Password"].ToString();
                                userModel.TypeUser = Convert.ToInt32(dt.Rows[i]["Type"]);
                                return userModel;
                            }
                        
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
