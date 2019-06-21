using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VS_LOAN.Core.Utility.Exceptions
{
    public class BaseRuntimeException : Exception
    {
        private String exceptionId;

        protected BaseRuntimeException() : base()
        {            
            genExceptionId();
        }

        
        protected BaseRuntimeException(String arg0) : base (arg0)
        {            
            genExceptionId();
        }

        protected BaseRuntimeException(String arg0, Exception ex)
            : base(arg0, ex)
        {
            genExceptionId();
        }


        
        private void genExceptionId()
        {
            if (exceptionId != null)
            {
                return;
            }

            exceptionId = String.Format("{0:yyyyMMddHHmmss}", DateTime.Now);

        }

        public String getExceptionId()
        {
            return exceptionId;
        }
        

        public override String ToString()
        {
            return "Exception ID:" + getExceptionId() + " " + base.ToString();
        }
    }
}
