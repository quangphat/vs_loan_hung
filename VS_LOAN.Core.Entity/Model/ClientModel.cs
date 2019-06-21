using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.Model
{
   public class ClientModel
    {
        private string id = string.Empty;

        public string Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

       

        private string _name = string.Empty;
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
    }
}
