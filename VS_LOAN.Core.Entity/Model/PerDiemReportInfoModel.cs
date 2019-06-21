using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.Model
{
    public class PerDiemReportInfoModel
    {
        private string _employeeName = string.Empty;
        private string _employeeCode = string.Empty;
        private string _employeePosition = string.Empty;
        private string _employeeDepartment = string.Empty;
        private string _costCenter = string.Empty;
        private string _companyCode = string.Empty;
        private string _tripPurpose = string.Empty;
        private string _chargedTo = string.Empty;
        private string _engagementCode = string.Empty;
        private DateTime _departureDay = DateTime.MinValue;
        private DateTime _returnDay = DateTime.MinValue;
        private string _currency = string.Empty;
        private float _totalPerDiemAllowance = 0;
        private float _lessCashAdvance = 0;
        private float _payable = 0;
        private string _payment = string.Empty;
        private string _approverName = string.Empty;

        public string EmployeeName
        {
            get
            {
                return _employeeName;
            }

            set
            {
                _employeeName = value;
            }
        }

        public string EmployeeCode
        {
            get
            {
                return _employeeCode;
            }

            set
            {
                _employeeCode = value;
            }
        }

        public string EmployeePosition
        {
            get
            {
                return _employeePosition;
            }

            set
            {
                _employeePosition = value;
            }
        }

        public string EmployeeDepartment
        {
            get
            {
                return _employeeDepartment;
            }

            set
            {
                _employeeDepartment = value;
            }
        }

        public string CostCenter
        {
            get
            {
                return _costCenter;
            }

            set
            {
                _costCenter = value;
            }
        }

        public string CompanyCode
        {
            get
            {
                return _companyCode;
            }

            set
            {
                _companyCode = value;
            }
        }

        public string TripPurpose
        {
            get
            {
                return _tripPurpose;
            }

            set
            {
                _tripPurpose = value;
            }
        }

        public string ChargedTo
        {
            get
            {
                return _chargedTo;
            }

            set
            {
                _chargedTo = value;
            }
        }

        public string EngagementCode
        {
            get
            {
                return _engagementCode;
            }

            set
            {
                _engagementCode = value;
            }
        }

        public DateTime DepartureDay
        {
            get
            {
                return _departureDay;
            }

            set
            {
                _departureDay = value;
            }
        }

        public DateTime ReturnDay
        {
            get
            {
                return _returnDay;
            }

            set
            {
                _returnDay = value;
            }
        }

        public string Currency
        {
            get
            {
                return _currency;
            }

            set
            {
                _currency = value;
            }
        }

        public float TotalPerDiemAllowance
        {
            get
            {
                return _totalPerDiemAllowance;
            }

            set
            {
                _totalPerDiemAllowance = value;
            }
        }

        public float LessCashAdvance
        {
            get
            {
                return _lessCashAdvance;
            }

            set
            {
                _lessCashAdvance = value;
            }
        }

        public float Payable
        {
            get
            {
                return _payable;
            }

            set
            {
                _payable = value;
            }
        }

        public string Payment
        {
            get
            {
                return _payment;
            }

            set
            {
                _payment = value;
            }
        }

        public string ApproverName
        {
            get
            {
                return _approverName;
            }

            set
            {
                _approverName = value;
            }
        }
    }
}
