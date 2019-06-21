using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.Model
{
   public class NhanVienInfoModel
    {
        private int _id;
        private string _fullText;

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

        public string FullText
        {
            get
            {
                return _fullText;
            }

            set
            {
                _fullText = value;
            }
        }
    }
}
