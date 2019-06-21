using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPMG_PD.Core.Entity.Model
{
    public class PerDiemPaymentProcessInfoModel
    {
        private int _id;
        private string _employeeName = string.Empty;
        private string _employeePosition = string.Empty;
        private string _employeeDepartment = string.Empty;
        private DateTime _fromDate;
        private DateTime _toDate;
        private string _tripPurpose = string.Empty;
        private int _status;
        private int _chargedTo;
        private string _perDiemCode = string.Empty;
        private float _totalPerDiemAllowance;
        private string _approverName = string.Empty;

        public int ID
        {
            get
            {
                return _id;
            }

            set
            {
                _id = value;
            }
        }

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

        public DateTime FromDate
        {
            get
            {
                return _fromDate;
            }

            set
            {
                _fromDate = value;
            }
        }

        public DateTime ToDate
        {
            get
            {
                return _toDate;
            }

            set
            {
                _toDate = value;
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

        public int Status
        {
            get
            {
                return _status;
            }

            set
            {
                _status = value;
            }
        }

        public int ChargedTo
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

        public string PerDiemCode
        {
            get
            {
                return _perDiemCode;
            }

            set
            {
                _perDiemCode = value;
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
