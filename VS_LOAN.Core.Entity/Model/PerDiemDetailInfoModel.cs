using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.Model
{
    public class PerDiemDetailInfoModel
    {
        private int _id;
        private int _tripPurposeID;
        private string _tripPurpose;
        private int _status;
        private int _chargeTo;
        private int _destinationID;
        private string _destination;
        private int _perDiemRateID;
        private string _perDiemRate;
        private string _employeeName;
        private string _employeeCode;
        private string _employeePosition;
        private string _employeeIBSNumber;
        private string _employeeCostCenter;
        private string _employeeDepartment;
        private string _engagementCode;
        private string _engagementName;
        private string _costCenter;
        private string _costCenterText;
        private string _perDiemCode;
        private string _createrID = string.Empty;
        private DateTime _createDate = DateTime.MinValue;
        private List<EntitlementGroupModel> _lstEntitlementGroup=new List<EntitlementGroupModel>();
        private List<PerDiemApproverInfoModel> _approverList;
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

        public int TripPurposeID
        {
            get
            {
                return _tripPurposeID;
            }

            set
            {
                _tripPurposeID = value;
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

        public int ChargeTo
        {
            get
            {
                return _chargeTo;
            }

            set
            {
                _chargeTo = value;
            }
        }

        public int DestinationID
        {
            get
            {
                return _destinationID;
            }

            set
            {
                _destinationID = value;
            }
        }

        public string Destination
        {
            get
            {
                return _destination;
            }

            set
            {
                _destination = value;
            }
        }

        public int PerDiemRateID
        {
            get
            {
                return _perDiemRateID;
            }

            set
            {
                _perDiemRateID = value;
            }
        }

        public string PerDiemRate
        {
            get
            {
                return _perDiemRate;
            }

            set
            {
                _perDiemRate = value;
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

        //public List<PerDiemEntitlementModel> EntitlementList
        //{
        //    get
        //    {
        //        return _entitlementList;
        //    }

        //    set
        //    {
        //        _entitlementList = value;
        //    }
        //}

        public List<PerDiemApproverInfoModel> ApproverList
        {
            get
            {
                return _approverList;
            }

            set
            {
                _approverList = value;
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
        public string EngagementName
        {
            get
            {
                return _engagementName;
            }

            set
            {
                _engagementName = value;
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

        public List<EntitlementGroupModel> LstEntitlementGroup
        {
            get
            {
                return _lstEntitlementGroup;
            }

            set
            {
                _lstEntitlementGroup = value;
            }
        }

        public string CreaterID
        {
            get
            {
                return _createrID;
            }

            set
            {
                _createrID = value;
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

        public DateTime CreateDate
        {
            get
            {
                return _createDate;
            }

            set
            {
                _createDate = value;
            }
        }

        public string CostCenterText
        {
            get
            {
                return _costCenterText;
            }

            set
            {
                _costCenterText = value;
            }
        }

        public string EmployeeIBSNumber
        {
            get
            {
                return _employeeIBSNumber;
            }

            set
            {
                _employeeIBSNumber = value;
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
    }
}
