using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.Model
{
    public class PerDiemPaymentMemberDetailModel
    {
        private string _userID;
        private string _employeeID = string.Empty;
        private string _employeeName = string.Empty;
        private string _employeePosition = string.Empty;
        private string _employeeCostCenter = string.Empty;
        private float _perDiemTotal;
        private float _lessCashAdvance;

        public string EmployeeID
        {
            get
            {
                return _employeeID;
            }

            set
            {
                _employeeID = value;
            }
        }

        public string UserID
        {
            get
            {
                return _userID;
            }

            set
            {
                _userID = value;
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

        public string EmployeeCostCenter
        {
            get
            {
                return _employeeCostCenter;
            }

            set
            {
                _employeeCostCenter = value;
            }
        }

        public float PerDiemTotal
        {
            get
            {
                return _perDiemTotal;
            }

            set
            {
                _perDiemTotal = value;
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
    }
}
