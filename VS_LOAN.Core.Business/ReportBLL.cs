using VS_LOAN.Core.Entity.Model;
using VS_LOAN.Core.Nhibernate;
using VS_LOAN.Core.Utility.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Business
{
    public class ReportBLL
    {
        public List<PerDiemReportInfoModel> GetPerDiemInfo(DateTime fromDate, DateTime toDate, string status)
        {
            try
            {
                using (var session = LOANSessionManager.OpenSession())
                {
                    IDbCommand command = new SqlCommand();
                    command.Connection = session.Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "sp_PD_PER_DIEM_INFO_ReportPerDiemInfoByCreateDate";
                    command.Parameters.Add(new SqlParameter("@Status", status));
                    if (fromDate == DateTime.MinValue)
                        command.Parameters.Add(new SqlParameter("@FromDate", DBNull.Value));
                    else
                        command.Parameters.Add(new SqlParameter("@FromDate", fromDate));
                    if (toDate == DateTime.MinValue)
                        command.Parameters.Add(new SqlParameter("@ToDate", DBNull.Value));
                    else
                        command.Parameters.Add(new SqlParameter("@ToDate", toDate));
                    DataTable dt = new DataTable();
                    dt.Load(command.ExecuteReader());
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            List<PerDiemReportInfoModel> rs = new List<PerDiemReportInfoModel>();
                            foreach (DataRow item in dt.Rows)
                            {
                                PerDiemReportInfoModel perDiem = new PerDiemReportInfoModel();
                               
                                perDiem.EmployeeName = item["EmployeeName"].ToString();
                                perDiem.EmployeeCode = item["EmployeeCode"].ToString();
                                perDiem.EmployeePosition = item["EmployeePosition"].ToString();
                                perDiem.EmployeeDepartment = item["EmployeeDepartment"].ToString();
                                perDiem.CostCenter = item["CostCenter"].ToString();
                                perDiem.CompanyCode = item["CompanyCode"].ToString();
                                perDiem.TripPurpose = item["TripPurpose"].ToString();
                                perDiem.ChargedTo = item["ChargedTo"].ToString();
                                perDiem.EngagementCode = item["EngagementCode"].ToString();
                                perDiem.ApproverName = item["ApproverName"].ToString();
                                if (item["DepartureDay"].ToString() != "")
                                {
                                    try
                                    {
                                        perDiem.DepartureDay = Convert.ToDateTime(item["DepartureDay"].ToString());
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }
                                if (item["ReturnDay"].ToString() != "")
                                {
                                    try
                                    {
                                        perDiem.ReturnDay = Convert.ToDateTime(item["ReturnDay"].ToString());
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }
                                try
                                {
                                    perDiem.LessCashAdvance = float.Parse(item["LessCashAdvance"].ToString());
                                }
                                catch { }
                                perDiem.Currency = item["Currency"].ToString();
                                perDiem.TotalPerDiemAllowance = float.Parse(item["TotalPerDiemAllowance"].ToString());
                                perDiem.Payable = perDiem.TotalPerDiemAllowance - perDiem.LessCashAdvance;
                                rs.Add(perDiem);
                            }
                            return rs;
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
