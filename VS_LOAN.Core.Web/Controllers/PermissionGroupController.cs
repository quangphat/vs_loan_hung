using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using VS_LOAN.Core.Repository;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Entity.Model;
using VS_LOAN.Core.Utility;
using VS_LOAN.Core.Utility.Exceptions;
using VS_LOAN.Core.Web.Helpers;
using VS_LOAN.Core.Repository.Interfaces;

namespace VS_LOAN.Core.Web.Controllers
{
    public class PermissionGroupController : LoanController
    {
        protected readonly IGroupRepository _rpGroup;
        public PermissionGroupController(IGroupRepository groupRepository) : base()
        {
            _rpGroup = groupRepository;
        }
        public static Dictionary<string, ActionInfo> LstRole
        {
            get
            {
                Dictionary<string, ActionInfo> _lstRole = new Dictionary<string, ActionInfo>();
                _lstRole.Add("TaoMoi", new ActionInfo() { _formindex = IndexMenu.M_5_1, _href = "PermissionGroup/TaoMoi", _mangChucNang = new int[] { (int)QuyenIndex.QLNHomQuyen } });
                _lstRole.Add("DanhsachNhomQuyen", new ActionInfo() { _formindex = IndexMenu.M_5_2, _href = "PermissionGroup/DanhsachNhomQuyen", _mangChucNang = new int[] { (int)QuyenIndex.QLNHomQuyen } });
               
                return _lstRole;
            }
        }

        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.QLNHomQuyen })]
        public ActionResult TaoMoi()
        {
            ViewBag.formindex = "";//LstRole[RouteData.Values["action"].ToString()]._formindex;
            return View();
        }
        
        
       


        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.QLToNhom })]
        public ActionResult QLToNhom()
        {
            ViewBag.formindex = "";//LstRole[RouteData.Values["action"].ToString()]._formindex;
            return View();
        }

        

        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.QLToNhom })]
        public ActionResult Sua()
        {
            ViewBag.formindex = LstRole["QLToNhom"]._formindex;
            if(Session["ToNhom_Sua_ID"] == null)
                return RedirectToAction("QLToNhom");
            int idNhom = (int)Session["ToNhom_Sua_ID"];
            ViewBag.ThongTinNhom = new GroupRepository().LayTheoMaAsync(idNhom);
            return View();
        }

        public ActionResult SuaToNhomByID(int id)
        {
            Session["ToNhom_Sua_ID"] = id;
            return RedirectToAction("Sua");
        }

      
       

        public ActionResult XemToNhomByID(int id)
        {
            Session["ToNhom_ChiTiet_ID"] = id;
            return RedirectToAction("ChiTiet");
        }

        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.QLToNhom })]
        public ActionResult LuuSua(int maNhom, string ten, string tenNgan, int maNguoiQuanLy, int maNhomCha, List<int> lstThanhVien)
        {
            try
            {

                bool result = false;
                NhomModel nhom = new NhomModel();
                nhom.ID = maNhom;
                nhom.MaNguoiQL = maNguoiQuanLy;
                nhom.MaNhomCha = maNhomCha;
                if (maNhomCha != 0)
                    nhom.ChuoiMaCha = new GroupRepository().LayChuoiMaCha(maNhomCha) + "." + maNhomCha;
                else
                    nhom.ChuoiMaCha = "0";
                nhom.Ten = ten;
                nhom.TenNgan = tenNgan;
                result = new GroupRepository().Sua(nhom, lstThanhVien);
                if (result)
                {
                    return ToResponse(true,null, result);
                }
                return ToResponse(false,"Không thành công", 0);
            }
            catch (BusinessException ex)
            {
                return ToResponse(false, ex.Message);
            }
            
        }


        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.CauHinhDuyet })]
        public ActionResult CauHinhDuyet()
        {
            ViewBag.formindex = "";//LstRole[RouteData.Values["action"].ToString()]._formindex;
            return View();
        }

       
    }
}
