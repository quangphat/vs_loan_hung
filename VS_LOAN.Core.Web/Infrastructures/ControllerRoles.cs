using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VS_LOAN.Core.Utility;
using VS_LOAN.Core.Web.Helpers;

namespace VS_LOAN.Core.Web.Infrastructures
{
    public static class ControllerRoles
    {
        public static Dictionary<string, ActionInfo> Roles = new Dictionary<string, ActionInfo> {
             { "home_about", new ActionInfo{ _formindex = IndexMenu.M_0_1, _href = "Home/Index", _mangChucNang = new int[] { (int)QuyenIndex.Public } } },
             { "home_guide", new ActionInfo{ _formindex = IndexMenu.M_0_2, _href = "Home/HuongDanSuDung", _mangChucNang = new int[] { (int)QuyenIndex.Public } } },
             { "home_version", new ActionInfo{ _formindex = IndexMenu.M_0_3, _href = "Home/PhienBan", _mangChucNang = new int[] { (int)QuyenIndex.Public } } },
             { "profile_addnew", new ActionInfo{ _formindex = IndexMenu.M_1_1, _href = "HoSo/AddNew", _mangChucNang = new int[] { (int)QuyenIndex.Public } } },
             { "profile_list", new ActionInfo{ _formindex = IndexMenu.M_2_2, _href = "QuanLyHoSo/DanhSachHoSo", _mangChucNang = new int[] { (int)QuyenIndex.Public } } },
             { "profile_approve", new ActionInfo{ _formindex = IndexMenu.M_2_2, _href = "DuyetHoSo/Index", _mangChucNang = new int[] { (int)QuyenIndex.Public } } },
             { "checkdup_list", new ActionInfo{ _formindex = IndexMenu.M_5_2, _href = "Customer/Index", _mangChucNang = new int[] { (int)QuyenIndex.Public } } },
             { "checkdup_addnew", new ActionInfo{ _formindex = IndexMenu.M_5_1, _href = "Customer/Index", _mangChucNang = new int[] { (int)QuyenIndex.Public } } },
             { "company_list", new ActionInfo{ _formindex = IndexMenu.M_8_2, _href = "Company/Index", _mangChucNang = new int[] { (int)QuyenIndex.Public } } },
             { "company_addnew", new ActionInfo{ _formindex = IndexMenu.M_8_1, _href = "Company/AddNew", _mangChucNang = new int[] { (int)QuyenIndex.Public } } },
             { "courier_list", new ActionInfo{ _formindex = IndexMenu.M_7_2, _href = "Courrier/Index", _mangChucNang = new int[] { (int)QuyenIndex.Public } } },
             { "courier_addnew", new ActionInfo{ _formindex = IndexMenu.M_7_1, _href = "Courrier/AddNew", _mangChucNang = new int[] { (int)QuyenIndex.Public } } },
             { "team_addnew", new ActionInfo{ _formindex = IndexMenu.M_3_1, _href = "ToNhom/TaoMoi", _mangChucNang = new int[] { (int)QuyenIndex.Public } } },
             { "team_list", new ActionInfo{ _formindex = IndexMenu.M_3_2, _href = "ToNhom/QLToNhom", _mangChucNang = new int[] { (int)QuyenIndex.Public } } },
             { "team_config", new ActionInfo{ _formindex = IndexMenu.M_3_3, _href = "ToNhom/CauHinhDuyet", _mangChucNang = new int[] { (int)QuyenIndex.Public } } },
             { "product_list", new ActionInfo{ _formindex = IndexMenu.M_4_1, _href = "SanPhamVay/QuanLySanPham", _mangChucNang = new int[] { (int)QuyenIndex.Public } } },
             { "product_import", new ActionInfo{ _formindex = IndexMenu.M_4_2, _href = "SanPhamVay/Import", _mangChucNang = new int[] { (int)QuyenIndex.Public } } },
             { "employee_add", new ActionInfo{ _formindex = IndexMenu.M_4_1, _href = "Employee/AddNew", _mangChucNang = new int[] { (int)QuyenIndex.Public } } },
             { "employee_list", new ActionInfo{ _formindex = IndexMenu.M_4_2, _href = "Employee/Index", _mangChucNang = new int[] { (int)QuyenIndex.Public } } },
             { "mcedit_checkcat", new ActionInfo{ _formindex = IndexMenu.M_9_1, _href = "MCredit/CheckCat", _mangChucNang = new int[] { (int)QuyenIndex.Public } } },
             { "mcedit_checkcic", new ActionInfo{ _formindex = IndexMenu.M_9_2, _href = "MCredit/CheckCIC", _mangChucNang = new int[] { (int)QuyenIndex.Public } } },
             { "mcedit_checkdup", new ActionInfo{ _formindex = IndexMenu.M_9_3, _href = "MCredit/CheckDuplicate", _mangChucNang = new int[] { (int)QuyenIndex.Public } } },
             { "mcedit_checkstatus", new ActionInfo{ _formindex = IndexMenu.M_9_4, _href = "MCredit/CheckStatus", _mangChucNang = new int[] { (int)QuyenIndex.Public } } },
               { "ocbList", new ActionInfo{ _formindex = IndexMenu.M_15, _href = "ocb/temp", _mangChucNang = new int[] { (int)QuyenIndex.Public } } },
             { "mcedit_list_temp", new ActionInfo{ _formindex = IndexMenu.M_9_5, _href = "MCredit/Temp", _mangChucNang = new int[] { (int)QuyenIndex.Public } } },
             { "mcedit_list", new ActionInfo{ _formindex = IndexMenu.M_9_6, _href = "MCredit/Index", _mangChucNang = new int[] { (int)QuyenIndex.Public } } },
             { "revoke_list", new ActionInfo{ _formindex = IndexMenu.M_10_1, _href = "Revoke/Index", _mangChucNang = new int[] { (int)QuyenIndex.Public } } },
             { "mirae_list", new ActionInfo{ _formindex = IndexMenu.M_16, _href = "Mirae/AddNew", _mangChucNang = new int[] { (int)QuyenIndex.Public } } },

             { "mirae_checkDefer", new ActionInfo{ _formindex = IndexMenu.M_16_3, _href = "MiraeDefer/Index", _mangChucNang = new int[] { (int)QuyenIndex.Public } } },

              { "mirae_managementstatus", new ActionInfo{ _formindex = IndexMenu.M_16_3, _href = "MiraeStatus/Index", _mangChucNang = new int[] { (int)QuyenIndex.Public } } },
            


            { "mirae_checkDuplicate", new ActionInfo{ _formindex = IndexMenu.M_16_2, _href = "mirae/CheckCIC", _mangChucNang = new int[] { (int)QuyenIndex.Public } } }



        };
    }
}