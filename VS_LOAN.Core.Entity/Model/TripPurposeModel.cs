using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.Model
{
    public class TripPurposeModel
    {
        private int _id;
        private string _description;
        private int _chargeTo;

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
    }
}
