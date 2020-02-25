using Dapper;
using LoanRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Entity.EasyCredit;

namespace LoanRepository.Classes
{
    public class EcHosoRepository : BaseRepository, IEcHosoRepository
    {
        
        public async Task<int> Insert(EcHoso model)
        {
            var p = AddOutputParam("Id");
            p.Add("RequestId", model.RequestId);
            p.Add("PartnerCode", model.PartnerCode);
            p.Add("ProposalId", model.ProposalId);
            p.Add("Status", model.Status);
            p.Add("EcResult", model.EcResult);
            p.Add("OTPCode", model.OTPCode);
            p.Add("SMSId", model.SMSId);
            p.Add("TempProvinceCode", model.TempProvinceCode);
            p.Add("TempDistrictCode", model.TempDistrictCode);
            p.Add("TempWardCode", model.TempWardCode);
            p.Add("TempAddress", model.TempAddress);
            p.Add("OccupationCode", model.OccupationCode);
            p.Add("TypeContractCode", model.TypeContractCode);
            p.Add("ContractFromDate", model.ContractFromDate);
            p.Add("ContractToDate", model.ContractToDate);
            p.Add("MethodIncome", model.MethodIncome);
            p.Add("FrequentIncome", model.FrequentIncome);
            p.Add("DateIncome", model.DateIncome);
            p.Add("MonthLyIncome", model.MonthLyIncome);
            p.Add("OtherIncome", model.OtherIncome);
            p.Add("MonthLyExpense", model.MonthLyExpense);
            p.Add("JobTitle", model.JobTitle);
            p.Add("CompanyName", model.CompanyName);
            p.Add("WorkProvinceCode", model.WorkProvinceCode);
            p.Add("WorkDistrictCode", model.WorkDistrictCode);
            p.Add("WorkWardCode", model.WorkWardCode);
            p.Add("WorkAddress", model.WorkAddress);
            p.Add("CompanyPhone", model.CompanyPhone);
            p.Add("MarriedStatus", model.MarriedStatus);
            p.Add("HouseType", model.HouseType);
            p.Add("NumberDependences", model.NumberDependences);
            p.Add("YearOfStay", model.YearOfStay);
            p.Add("LoanPurpose", model.LoanPurpose);
            p.Add("BankCode", model.BankCode);
            p.Add("BankName", model.BankName);
            p.Add("BranchCode", model.BranchCode);
            p.Add("BranchName", model.BranchName);
            p.Add("BankProvince", model.BankProvince);
            p.Add("AccountNumber", model.AccountNumber);
            p.Add("AccountName", model.AccountName);
            p.Add("DetailContact", model.DetailContact);
            p.Add("OtherContact", model.OtherContact);
            p.Add("AddressReceivingLetter", model.AddressReceivingLetter);
            p.Add("Relation1Code", model.Relation1Code);
            p.Add("Relation1Name", model.Relation1Name);
            p.Add("Relation1Phone", model.Relation1Phone);
            p.Add("Relation2Code", model.Relation2Code);
            p.Add("Relation2Name", model.Relation2Name);
            p.Add("Relation2Phone", model.Relation2Phone);
            p.Add("LoanAmount", model.LoanAmount);
            p.Add("LoanTenor", model.LoanTenor);
            p.Add("FullName", model.FullName);
            p.Add("Cmnd", model.Cmnd);
            p.Add("Gender", model.Gender);
            p.Add("Phone", model.Phone);
            p.Add("Email", model.Email);
            p.Add("BirthDay", model.BirthDay);
            p.Add("IssueDate", model.IssueDate);
            p.Add("IssuePlaceCode", model.IssuePlaceCode);
            p.Add("PermanentProvinceCode", model.PermanentProvinceCode);
            p.Add("PermanentDistrictCode", model.PermanentDistrictCode);
            p.Add("PermanentWardCode", model.PermanentWardCode);
            p.Add("PermanentAddress", model.PermanentAddress);
            p.Add("CreatedTime", model.CreatedTime);
            p.Add("CreatedBy", model.CreatedBy);
            p.Add("UpdatedTime", model.UpdatedTime);
            p.Add("UpdatedBy", model.UpdatedBy);
            using (var conn = GetConnection())
            {
                await conn.ExecuteAsync("sp_InsertEcHoso",p, commandType: System.Data.CommandType.StoredProcedure);
                return p.Get<int>("Id");
            }
        }

        public async Task<bool> InsertHosoRequest(int hosoId, string requestId)
        {
            using (var conn = GetConnection())
            {
                await conn.ExecuteAsync("sp_InsertEcHosoRequestId", new {
                    hosoId,
                    RequestId = requestId
                }, commandType: System.Data.CommandType.StoredProcedure);
                return true;
            }
        }
    }
}

