using VS_LOAN.Core.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VS_LOAN.Core.Utility;
using VS_LOAN.Core.Entity.Model;
using VS_LOAN.Core.Repository;

namespace VS_LOAN.Core.Web.Controllers
{
    public class KhuVucController : BaseController
    {
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
