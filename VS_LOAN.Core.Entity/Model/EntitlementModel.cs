using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.Model
{
    public class EntitlementModel
    {
        private int _id;
        private int _entitlementGroupID;
        private DateTime _startDate;
        private bool _startType;
        private DateTime _endDate;
        private bool _endType;
        private string _proportionData;
        private float _total;
        public bool Breakfast { get; set; }
        public bool Lunch { get; set; }
        public bool Dinner { get; set; }
        public bool Other { get; set; }
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
        public string GetTripsDay()
        {
            string result = string.Empty;
            result=StartDate.ToString("dd/MM/yyyy");
            if (StartType)
                result += " (PM)";
            result +=" - "+EndDate.ToString("dd/MM/yyyy");
            if (EndType)
                result += " (AM)";
            double total = ((EndDate - StartDate).TotalDays + 1);
            if (total == 1)
                result += " " + '('+ total + " day)";
                else
            result += " " + '('+ total + " days)";
           
            return result;
        }
      
        public int EntitlementGroupID
        {
            get
            {
                return _entitlementGroupID;
            }

            set
            {
                _entitlementGroupID = value;
            }
        }

        public DateTime StartDate
        {
            get
            {
                return _startDate;
            }

            set
            {
                _startDate = value;
            }
        }

        public bool StartType
        {
            get
            {
                return _startType;
            }

            set
            {
                _startType = value;
            }
        }

        public DateTime EndDate
        {
            get
            {
                return _endDate;
            }

            set
            {
                _endDate = value;
            }
        }

        public bool EndType
        {
            get
            {
                return _endType;
            }

            set
            {
                _endType = value;
            }
        }

        public string ProportionData
        {
            get
            {
                return _proportionData;
            }

            set
            {
                _proportionData = value;
            }
        }

        public float Total
        {
            get
            {
                return _total;
            }

            set
            {
                _total = value;
            }
        }
        public bool GetProportionDataDetail(string proportioned)
        {
            if (ProportionData.IndexOf(proportioned) >= 0)
                return true;
            else
                return false;
             
        }
      
    }
}
