using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.Model
{
    public class PerDiemPaymentModel
    {
        private int _perDiemID;
        private string _userID;
        private string _creator;
        private DateTime _createDate;
        private float _perDiemTotal;
        private float _lessCashAdvance;
        private int _lessCashAdvanceType;
        private string _advanceCode;

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

        public string Creator
        {
            get
            {
                return _creator;
            }

            set
            {
                _creator = value;
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

        public int LessCashAdvanceType
        {
            get
            {
                return _lessCashAdvanceType;
            }

            set
            {
                _lessCashAdvanceType = value;
            }
        }

        public string AdvanceCode
        {
            get
            {
                return _advanceCode;
            }

            set
            {
                _advanceCode = value;
            }
        }
    }
}
