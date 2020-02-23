using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VS_LOAN.Core.Business.Interfaces;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Entity.EasyCredit;
using VS_LOAN.Core.Entity.Infrastructures;

namespace VS_LOAN.Core.Web.Controllers
{
    [Route("EcEmployment")]
    public class EcEmploymentController : BaseController
    {
        
        public EcEmploymentController(CurrentProcess currentProcess, 
            I):base(currentProcess)
        {
            
        }

    }
}