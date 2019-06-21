using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.Model
{
    public class PerDiemInfoModel
    {
        private int _id;
        private string _userID;
        private DateTime _createDate;
        private int _tripPurposeID;
        private DateTime _departureDate;
        private DateTime _returnDate;
        private int _perDiemRateID;
        private int _chargeTo;
        private int _status;
        private string _engagementCode;
        private string _perDiemCode;
        private string _costCenter;
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

        //public DateTime FromDate
        //{
        //    get
        //    {
        //        return _fromDate;
        //    }

        //    set
        //    {
        //        _fromDate = value;
        //    }
        //}

        //public DateTime ToDate
        //{
        //    get
        //    {
        //        return _toDate;
        //    }

        //    set
        //    {
        //        _toDate = value;
        //    }
        //}

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
    }
}
