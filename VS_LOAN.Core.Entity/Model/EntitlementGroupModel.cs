using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.Model
{
  public  class EntitlementGroupModel
    {
        private int _id = 0;
        private int _perDiemID = 0;
        private int _order = 0;
        private List<EntitlementModel> _lstEntitlement = new List<EntitlementModel>();
        private List<EntitlementUserModel> _lstEmployee = new List<EntitlementUserModel>();
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

        public List<EntitlementModel> LstEntitlement
        {
            get
            {
                return _lstEntitlement;
            }

            set
            {
                _lstEntitlement = value;
            }
        }

        public List<EntitlementUserModel> LstEmployee
        {
            get
            {
                return _lstEmployee;
            }

            set
            {
                _lstEmployee = value;
            }
        }

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
    }
}
