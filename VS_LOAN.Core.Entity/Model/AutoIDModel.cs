using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.Model
{
   public class AutoIDModel
    {
        private int id = 0;

        public int ID
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
        private string nameID = string.Empty;
        public string NameID
        {
            get
            {
                return nameID;
            }

            set
            {
                nameID = value;
            }
        }
        private string prefix = string.Empty;
        public string Prefix
        {
            get
            {
                return prefix;
            }

            set
            {
                prefix = value;
            }
        }
        private string suffixes = string.Empty;
        public string Suffixes
        {
            get
            {
                return suffixes;
            }

            set
            {
                suffixes = value;
            }
        }

        public int Value
        {
            get
            {
                return value;
            }

            set
            {
                this.value = value;
            }
        }

        private int value = 0;
       
        
    }
}
