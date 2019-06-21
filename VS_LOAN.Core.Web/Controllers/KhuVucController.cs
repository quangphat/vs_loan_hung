﻿using VS_LOAN.Core.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VS_LOAN.Core.Utility;
using VS_LOAN.Core.Entity.Model;
using VS_LOAN.Core.Business;

namespace VS_LOAN.Core.Web.Controllers
{
    public class KhuVucController : Controller
    {
        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.Public })]
        public JsonResult LayDSTinh()
        {
            List<KhuVucModel> rs = new KhuVucBLL().LayDSTinh();
            return Json(new { DSTinh = rs });
        }
        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.Public })]
        public JsonResult LayDSHuyen(int maTinh)
        {
            List<KhuVucModel> rs = new KhuVucBLL().LayDSHuyen(maTinh);
            return Json(new { DSHuyen = rs });
        }

    }
}
