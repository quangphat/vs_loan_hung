using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.Model
{
    public class PerDiemManagementInfoModel
    {
        private int _id;
        private DateTime _createDate;
        private int _tripPurposeID;
        private string _tripPurpose;
        private int _status;
        private int _destinationID;
        private string _destination;
        private string _perDiemCode;
        private int _chargeTo;
        private DateTime _departureDate;
        private DateTime _returnDate;
        private string _engagementCode = string.Empty;
        private string _engagementName = string.Empty;
        private string _clientCode = string.Empty;
        private string _clientName = string.Empty;
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
    }
}
