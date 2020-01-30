using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using VS_LOAN.Core.Web.Helpers;
using VS_LOAN.Core.Utility;
using VS_LOAN.Core.Web.Controllers;
using VS_LOAN.Core.Entity.Infrastructures;

namespace VS_LOAN.Core.Web.Controllers
{
    public class HomeController : LoanController
    {
        public HomeController(CurrentProcess currentProcess) : base(currentProcess)
        {

        }
        public static Dictionary<string, ActionInfo> LstRole
        {
            get
            {
                  Dictionary<string, ActionInfo> _lstRole = new Dictionary<string, ActionInfo>();
                  _lstRole.Add("Index", new ActionInfo() { _formindex = IndexMenu.M_0_1, _href = "Home/Index", _mangChucNang = new int[] { (int)QuyenIndex.Public } });
                  _lstRole.Add("HuongDanSuDung", new ActionInfo() { _formindex = IndexMenu.M_0_2, _href = "Home/HuongDanSuDung", _mangChucNang = new int[] { (int)QuyenIndex.Public } });
                  _lstRole.Add("PhienBan", new ActionInfo() { _formindex = IndexMenu.M_0_3, _href = "Home/PhienBan", _mangChucNang = new int[] { (int)QuyenIndex.Public } });
                  return _lstRole;
            }

        }
        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.Public })]
        public ActionResult Index()
        {
            ViewBag.formindex = LstRole[RouteData.Values["action"].ToString()]._formindex;
            return View();
        }
        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.Public })]
        public ActionResult HuongDanSuDung()
        {
            ViewBag.formindex = LstRole[RouteData.Values["action"].ToString()]._formindex;
            return View();
        }

        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.Public })]
        public FileResult DownloadFile(string file)
        {
            try
            {
                byte[] fileBytes = System.IO.File.ReadAllBytes(HostingEnvironment.MapPath("~/App_Data/" + file));
                var response = new FileContentResult(fileBytes, "application/octet-stream");
                response.FileDownloadName = file;
                return response;
            }
            catch (Exception)
            {
                return null;
            }
        }
       [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.Public })]
        public ActionResult PhienBan()
        {
            ViewBag.formindex = LstRole[RouteData.Values["action"].ToString()]._formindex;            
            return View();
        }
        
    }
}
