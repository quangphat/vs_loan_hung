using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.Model
{
    public class PerDiemPaymentDetailInfoModel
    {
        private int _perDiemID;
        private string _perDiemCode;
        private string _creatorName;
        private string _creatorCode;
        private string _creatorPosition;
        private string _creatorCostCenter;
        private string _creatorDepartment;
        private string _creatorIBSNumber;
        private string _tripPurpose;
        private string _engagementCode;
        private string _engagementName;
        private string _costCenter;
        private string _costCenterText;
        private string _destination;
        private int _chargeTo;
        private int _perDiemStatus;
        private List<PerDiemPaymentMemberDetailModel> _memberDetailList;

        public int PerDiemID
        {
            get
            {
                return _perDiemID;
            }

            set
            {
                _perDiemID = value;
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

        public string CreatorCode
        {
            get
            {
                return _creatorCode;
            }

            set
            {
                _creatorCode = value;
            }
        }

        public string CreatorPosition
        {
            get
            {
                return _creatorPosition;
            }

            set
            {
                _creatorPosition = value;
            }
        }

        public string CreatorCostCenter
        {
            get
            {
                return _creatorCostCenter;
            }

            set
            {
                _creatorCostCenter = value;
            }
        }
      
        public string CreatorIBSNumber
        {
            get
            {
                return _creatorIBSNumber;
            }

            set
            {
                _creatorIBSNumber = value;
            }
        }
        public string CreatorDepartment
        {
            get
            {
                return _creatorDepartment;
            }

            set
            {
                _creatorDepartment = value;
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

        public List<PerDiemPaymentMemberDetailModel> MemberDetailList
        {
            get
            {
                return _memberDetailList;
            }

            set
            {
                _memberDetailList = value;
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
    }
}
