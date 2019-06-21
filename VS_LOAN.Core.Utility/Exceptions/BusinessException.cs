using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VS_LOAN.Core.Utility.Exceptions
{
    public class BusinessException : BaseRuntimeException
    {        

        public BusinessException()
            : base()
        {
        }

        
        public BusinessException(String message)
            : base(message)
        {
            
        }

        public BusinessException(String message, Exception ex)
            : base(message, ex)
        {

        }
    }
}
