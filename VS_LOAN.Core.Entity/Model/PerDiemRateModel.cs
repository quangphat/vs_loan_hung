using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.Model
{
    public class PerDiemRateModel
    {
        private int _id;
        private string _description;
        private float _rate;
        private string _currency;
        private int _destinationID;
        private int _national;

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

        public string Description
        {
            get
            {
                return _description;
            }

            set
            {
                _description = value;
            }
        }

        public float Rate
        {
            get
            {
                return _rate;
            }

            set
            {
                _rate = value;
            }
        }

        public string Currency
        {
            get
            {
                return _currency;
            }

            set
            {
                _currency = value;
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

        public int NationalID
        {
            get
            {
                return _national;
            }

            set
            {
                _national = value;
            }
        }
    }
}
