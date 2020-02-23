using VS_LOAN.Core.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VS_LOAN.Core.Utility;
using VS_LOAN.Core.Entity.Model;
using VS_LOAN.Core.Business;
using VS_LOAN.Core.Entity.Infrastructures;

namespace VS_LOAN.Core.Web.Controllers
{
    public class KhuVucController : BaseController
    {
        public KhuVucController(CurrentProcess currentProcess):base(currentProcess)
        {

        }
        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.Public })]
        public JsonResult LayDSTinh()
        {
            List<KhuVucModel> rs = new KhuVucBLL().LayDSTinh();
            return ToJsonResponse(true, null, rs);
        }
        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.Public })]
        public JsonResult LayDSHuyen(int maTinh)
        {
            List<KhuVucModel> rs = new KhuVucBLL().LayDSHuyen(maTinh);
            return ToJsonResponse(true, null, rs);
        }

    }
}
