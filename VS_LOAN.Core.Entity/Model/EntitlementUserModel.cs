using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.Model
{
  public  class EntitlementUserModel
    {
        private string _userID;
        private string _name;
        private string _position;
        private string _costCenter;
        private string _ibsNumber;

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

        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
            }
        }

        public string Position
        {
            get
            {
                return _position;
            }

            set
            {
                _position = value;
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

        public string IBSNumber
        {
            get
            {
                return _ibsNumber;
            }

            set
            {
                _ibsNumber = value;
            }
        }
    }
}
