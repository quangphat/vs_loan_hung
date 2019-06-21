using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.Model
{
    public class PerDiemApprovementInfoModel
    {
        private int _id;
        private int _tripPurposeID;
        private string _tripPurpose;
        private int _perDiemStatus;
        private int _chargeTo;
        private int _destinationID;
        private string _destination;
        private string _creatorName;
        private string _creatorCostCenterName;
        private string _creatorDepartmentName;
        private DateTime _createDate;
        private int _approveStatus;
        private string _perDiemCode;
        private string _engagementCode = string.Empty;
        private string _engagementName = string.Empty;
        private string _clientCode = string.Empty;
        private string _clientName = string.Empty;
        private DateTime _departureDate;
        private DateTime _returnDate;
        private int _noOfDay;
        private string _currency;
        private float _perDiemAllowance;
      
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

        public int PerDiemStatus
        {
            get
            {
                return _perDiemStatus;
            }

            set
            {
                _perDiemStatus = value;
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

        public string CreatorName
        {
            get
            {
                return _creatorName;
            }

            set
            {
                _creatorName = value;
            }
        }

        public string CreatorCostCenterName
        {
            get
            {
                return _creatorCostCenterName;
            }

            set
            {
                _creatorCostCenterName = value;
            }
        }

        public string CreatorDepartmentName
        {
            get
            {
                return _creatorDepartmentName;
            }

            set
            {
                _creatorDepartmentName = value;
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

        public int ApproveStatus
        {
            get
            {
                return _approveStatus;
            }

            set
            {
                _approveStatus = value;
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

        public string ClientCode
        {
            get
            {
                return _clientCode;
            }

            set
            {
                _clientCode = value;
            }
        }

        public string ClientName
        {
            get
            {
                return _clientName;
            }

            set
            {
                _clientName = value;
            }
        }

        public DateTime DepartureDate
        {
            get
            {
                return _departureDate;
            }

            set
            {
                _departureDate = value;
            }
        }

        public DateTime ReturnDate
        {
            get
            {
                return _returnDate;
            }

            set
            {
                _returnDate = value;
            }
        }

        public int NoOfDay
        {
            get
            {
                return _noOfDay;
            }

            set
            {
                _noOfDay = value;
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

        public float PerDiemAllowance
        {
            get
            {
                return _perDiemAllowance;
            }

            set
            {
                _perDiemAllowance = value;
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

        
    }
}
