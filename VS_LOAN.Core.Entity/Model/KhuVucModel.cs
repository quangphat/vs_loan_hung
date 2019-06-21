using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.Model
{
   public class KhuVucModel
    {
        private int _id;
        private string _ten;

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

        public string Ten
        {
            get
            {
                return _ten;
            }

            set
            {
                _ten = value;
            }
        }
    }
}
