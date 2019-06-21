using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.Model
{
    public class PerDiemApproverModel
    {
        private int _perDiemID;
        private string _userID;
        private string _fullName;
        private int _order;
        private int _approveStatus;
        private string _note = string.Empty;
        private DateTime _approveDate;

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

        public int Order
        {
            get
            {
                return _order;
            }

            set
            {
                _order = value;
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

        public string Note
        {
            get
            {
                return _note;
            }

            set
            {
                _note = value;
            }
        }

        public DateTime ApproveDate
        {
            get
            {
                return _approveDate;
            }

            set
            {
                _approveDate = value;
            }
        }

        public string FullName
        {
            get
            {
                return _fullName;
            }

            set
            {
                _fullName = value;
            }
        }
    }
}
