using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VS_LOAN.Core.Utility.Exceptions
{
    public class SystemException : BaseRuntimeException
    {
        public SystemException()
            : base()
        {
        }

        public SystemException(String message)
            : base(message)
        {

        }
    }
}
