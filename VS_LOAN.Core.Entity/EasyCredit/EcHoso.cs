using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.EasyCredit
{
    public class EcHoso : EcBaseModel
    {
        public int Id { get; set; }    
        public int TempProvinceCode { get; set; }
        public int TempDistrictCode { get; set; }
        public int TempWardCode { get; set; }
        public string TempAddress { get; set; }
        public string OccupationCode { get; set; }
        public string TypeContractCode { get; set; }
        public DateTime? ContractFromDate { get; set; }
        public DateTime? ContractToDate { get; set; }
        public string MethodIncome { get; set; }
        public string FrequentIncome { get; set; }
        public int DateIncome { get; set; }
        public decimal MonthLyIncome { get; set; }
        public decimal OtherIncome { get; set; }
        public decimal MonthLyExpense { get; set; }
        public decimal LoanAmount { get; set; }
        public int LoanTenor { get; set; }
        public string JobTitle { get; set; }
        public string CompanyName { get; set; }
        public int WorkProvinceCode { get; set; }
        public int WorkDistrictCode { get; set; }
        public int WorkWardCode { get; set; }
        public string WorkAddress { get; set; }
        public string CompanyPhone { get; set; }
        public string MarriedStatus { get; set; }
        public string HouseType { get; set; }
        public int NumberDependences { get; set; }
        public int YearOfStay { get; set; }
        public string LoanPurpose { get; set; }
        public int BankCode { get; set; }
        public string BankName { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string BankProvince { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public string DetailContact { get; set; }
        public string OtherContact { get; set; }
        public string AddressReceivingLetter { get; set; }
        public string Relation1Code { get; set; }
        public string Relation1Name { get; set; }
        public string Relation1Phone { get; set; }
        public string Relation2Code { get; set; }
        public string Relation2Name { get; set; }
        public string Relation2Phone { get; set; }
        public string EmploymentType { get; set; }
        public string FullName { get; set; }
        public string Cmnd { get; set; }
        public bool Gender { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime? BirthDay { get; set; }
        public DateTime? IssueDate { get; set; }
        public string IssuePlaceCode { get; set; }
        public int PermanentProvinceCode { get; set; }
        public int PermanentDistrictCode { get; set; }
        public int PermanentWardCode { get; set; }
        public string PermanentAddress { get; set; }
    }
}
