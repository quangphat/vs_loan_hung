using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.Model
{
    public class PerDiemApproverInfoModel
    {
        private string _userID;
        private string _approverName;
        private string _approverPosition;
        private string _approverCostCenter;
        private int _status;
        private int _order;
        private DateTime _approveDate = DateTime.MinValue;

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

        public string ApproverPosition
        {
            get
            {
                return _approverPosition;
            }

            set
            {
                _approverPosition = value;
            }
        }

        public string ApproverCostCenter
        {
            get
            {
                return _approverCostCenter;
            }

            set
            {
                _approverCostCenter = value;
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
    }
}
