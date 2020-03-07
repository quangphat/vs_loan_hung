using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.Model
{
   public class DoiTacModel
    {
        private int _iD;
        private string _name;

        public int ID
        {
            get
            {
                return _iD;
            }

            set
            {
                _iD = value;
            }
        }
        //public int F88Value { get; set; }
        public string Ten
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
    }
}
