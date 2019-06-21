using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity
{
   public class RLanguage
    {
        private string _ma = string.Empty;
        public string Ma
        {
            get
            {
                return _ma;
            }
            set
            {
                _ma = value;
            }
        }
        private Dictionary<string, string> _lstLanguage = new Dictionary<string, string>();
        public Dictionary<string, string> LstLanguage
        {
            get
            {
                return _lstLanguage;
            }
            set
            {
                _lstLanguage = value;
            }
        }
    }
}
