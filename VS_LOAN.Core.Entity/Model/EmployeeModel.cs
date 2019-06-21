using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Entity.Model
{
  public class EmployeeModel: UserPMModel
    {
        private string _positionName = string.Empty;
        public string PositionName
        {
            get
            {
                return _positionName;
            }
            set
            {
                _positionName = value;
            }
        }
        private string _costCenterName = string.Empty;
        public string CostCenterName
        {
            get
            {
                return _costCenterName;
            }
            set
            {
                _costCenterName = value;
            }
        }

        private string _department;
        public string Department
        {
            get
            {
                return _department;
            }

            set
            {
                _department = value;
            }
        }

        private string _code;
        public string Code
        {
            get
            {
                return _code;
            }

            set
            {
                _code = value;
            }
        }
        private string _national = string.Empty;
        public string National
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

        public string IBSNumber
        {
            get
            {
                return _ibsNumber;
            }

            set
            {
                _ibsNumber = value;
            }
        }

        private string _ibsNumber = string.Empty;
        
    }
}
