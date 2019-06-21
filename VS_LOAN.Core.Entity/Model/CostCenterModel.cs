using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.Model
{
   public class CostCenterModel
    {
        private string _code;
        private DateTime _fromDate;
        private DateTime _toDate;
        private string _controllingArea;
        private string _shortText;
        private string _longText;
        private string _personResponsible;
        private string _profitCenter;
        private string _costCenterGroup;
        private string _companyCode;
        private string _functionalArea;
        private int _buildingID;
        private bool _professional;
        private string _bookingApprover;

        public string Code
        {
            get
            {
                return _code;
            }

            set
            {
                _code = value;
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

        public string ControllingArea
        {
            get
            {
                return _controllingArea;
            }

            set
            {
                _controllingArea = value;
            }
        }

        public string ShortText
        {
            get
            {
                return _shortText;
            }

            set
            {
                _shortText = value;
            }
        }

        public string LongText
        {
            get
            {
                return _longText;
            }

            set
            {
                _longText = value;
            }
        }

        public string PersonResponsible
        {
            get
            {
                return _personResponsible;
            }

            set
            {
                _personResponsible = value;
            }
        }

        public string ProfitCenter
        {
            get
            {
                return _profitCenter;
            }

            set
            {
                _profitCenter = value;
            }
        }

        public string CostCenterGroup
        {
            get
            {
                return _costCenterGroup;
            }

            set
            {
                _costCenterGroup = value;
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

        public string FunctionalArea
        {
            get
            {
                return _functionalArea;
            }

            set
            {
                _functionalArea = value;
            }
        }

        public int BuildingID
        {
            get
            {
                return _buildingID;
            }

            set
            {
                _buildingID = value;
            }
        }

        public bool Professional
        {
            get
            {
                return _professional;
            }

            set
            {
                _professional = value;
            }
        }

        public string BookingApprover
        {
            get
            {
                return _bookingApprover;
            }

            set
            {
                _bookingApprover = value;
            }
        }
    }
}
