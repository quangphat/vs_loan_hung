using VS_LOAN.Core.Utility;
using VS_LOAN.Core.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using VS_LOAN.Core.Business;
using VS_LOAN.Core.Web.Infrastructures;

namespace VS_LOAN.Core.Web.Helpers
{
    public class SiteMenuItem
    {
        public string Text = "";
        public string Index = "0";
        public string Href = "#";
        public string Icon = "";
        public int[] functionList = null;

        public SiteMenuItem(string text,string icon, string index, string href, int[] funcs)
        {
            this.Icon = icon;
            this.Text = text;
            this.Index = index;
            this.Href = href;
            this.functionList = funcs;
        }
    }

    public class Menu
    {
 
        protected static bool checkMenuItem(int[] _mangChucNang)
        {
            List<string> lstQuyen = new List<string>();
            lstQuyen.Add((string)GlobalData.Rules);
            if (_mangChucNang == null)
                return false;
            for (int j = 0; j < _mangChucNang.Length; j++)
            {
                if (_mangChucNang[j] == (int)QuyenIndex.Public)
                    return true;
            }

            if (GlobalData.User == null)
                return false;
            if (lstQuyen != null)
            {
                if (lstQuyen.Count > 0)
                {
                    foreach (var item in lstQuyen)
                    {
                        for (int i = 0; i < _mangChucNang.Length; i++)
                        {
                            string quyen = item.Trim();
                            int kt = (quyen.Length - (_mangChucNang[i] - 1) / 4) - 1;
                            if (kt >= 0 && kt < quyen.Length)
                            {
                                string maQuyen = quyen[kt].ToString();
                                string gt = Convert.ToString(Convert.ToInt32(maQuyen.ToString(), 16), 2).PadLeft(4, '0');

                                if (_mangChucNang[i] == 0)
                                    return true;

                                if (gt[gt.Length - (_mangChucNang[i] - 1) % 4 - 1] == '1')
                                    return true;
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < _mangChucNang.Length; i++)
                    {
                        if (_mangChucNang[i] == 0)
                            return true;
                    }
                }
            }
            return false;
        }
        public static string ReturnMenu()
        {
            List<SiteMenuItem> _siteMenu = new List<SiteMenuItem>{
                    new SiteMenuItem(Resources.Global.Menu_TC,"menu-icon home",IndexMenu.M_0,"#",new int[] { (int)QuyenIndex.Public }),
                        new SiteMenuItem("Giới thiệu","",IndexMenu.M_0_1,HomeController.LstRole["Index"]._href,new int[] { (int)QuyenIndex.Public }),
                        //new SiteMenuItem("Hướng dẫn sử dụng","",IndexMenu.M_0_2,"#",new int[] { (int)QuyenIndex.Public }),
                        new SiteMenuItem("Phiên bản","",IndexMenu.M_0_2,HomeController.LstRole["PhienBan"]._href,new int[] { (int)QuyenIndex.Public }),
                    new SiteMenuItem("Tạo hồ sơ","menu-icon hs",IndexMenu.M_1,HoSoController.LstRole["AddNew"]._href,new int[] { }),
                        new SiteMenuItem("Tạo mới","", IndexMenu.M_1_1,HoSoController.LstRole["AddNew"]._href,HoSoController.LstRole["AddNew"]._mangChucNang), // 1.1   
                        //new SiteMenuItem("Hồ sơ của tôi","", IndexMenu.M_1_2,HoSoController.LstRole["Index"]._href,HoSoController.LstRole["Index"]._mangChucNang), // 1.1 
                    new SiteMenuItem("Quản lý hồ sơ","menu-icon qlhs",IndexMenu.M_2,"#",new int[] { }),
                        new SiteMenuItem("Danh sách hồ sơ","", IndexMenu.M_2_2,QuanLyHoSoController.LstRole["DanhSachHoSo"]._href, QuanLyHoSoController.LstRole["DanhSachHoSo"]._mangChucNang), // 1.3     
                        new SiteMenuItem("Duyệt hồ sơ","", IndexMenu.M_2_3,DuyetHoSoController.LstRole["Index"]._href, DuyetHoSoController.LstRole["Index"]._mangChucNang) ,// 1.2  
                    new SiteMenuItem("Check Duplicate","menu-icon duplicate",IndexMenu.M_5,"#",new int[] { }),
                        new SiteMenuItem("Tạo mới","", IndexMenu.M_5_1,CustomerController.LstRole["AddNew"]._href, CustomerController.LstRole["AddNew"]._mangChucNang), // 1.3     
                        new SiteMenuItem("Danh sách","", IndexMenu.M_5_2,CustomerController.LstRole["Index"]._href, CustomerController.LstRole["Index"]._mangChucNang), // 1.2  
                    new SiteMenuItem("Công ty","menu-icon company",IndexMenu.M_8,"#",new int[] { }),
                        new SiteMenuItem("Tạo mới","", IndexMenu.M_8_1,CompanyController.LstRole["AddNew"]._href, CompanyController.LstRole["AddNew"]._mangChucNang), // 1.3     
                        new SiteMenuItem("Danh sách","", IndexMenu.M_8_2,CompanyController.LstRole["Index"]._href, CompanyController.LstRole["Index"]._mangChucNang), // 1.2  
                   new SiteMenuItem("Courrier","menu-icon courier",IndexMenu.M_7,"#",new int[] { }),
                        new SiteMenuItem("Tạo mới","", IndexMenu.M_7_1,CourrierController.LstRole["AddNew"]._href, CourrierController.LstRole["AddNew"]._mangChucNang), // 1.3     
                        new SiteMenuItem("Danh sách","", IndexMenu.M_7_2,CourrierController.LstRole["Index"]._href, CourrierController.LstRole["Index"]._mangChucNang), // 1.2  
                    new SiteMenuItem("MCredit","menu-icon courier",IndexMenu.M_8,"#",new int[] { }),
                        new SiteMenuItem("Check CAT","", IndexMenu.M_8_1,ControllerRoles.Roles["mcedit_checkcat"]._href, ControllerRoles.Roles["mcedit_checkcat"]._mangChucNang),
                        new SiteMenuItem("Check CIC","", IndexMenu.M_8_2,ControllerRoles.Roles["mcedit_checkcic"]._href, ControllerRoles.Roles["mcedit_checkcic"]._mangChucNang),
                        new SiteMenuItem("Check Duplicate","", IndexMenu.M_8_3,ControllerRoles.Roles["mcedit_checkdup"]._href, ControllerRoles.Roles["mcedit_checkdup"]._mangChucNang),
                        new SiteMenuItem("Check Status","", IndexMenu.M_8_4,ControllerRoles.Roles["mcedit_checkstatus"]._href, ControllerRoles.Roles["mcedit_checkstatus"]._mangChucNang),// 1.3     
                        new SiteMenuItem("Danh sách","", IndexMenu.M_7_2,CourrierController.LstRole["Index"]._href, CourrierController.LstRole["Index"]._mangChucNang) // 1.2  
                 
                    
            };
            List<SiteMenuItem> siteMenuAdmin = new List<SiteMenuItem>
            {
                  new SiteMenuItem("Tổ nhóm","menu-icon group",IndexMenu.M_3,"#",new int[] { }),
                        new SiteMenuItem("Tạo mới","", IndexMenu.M_3_1,ToNhomController.LstRole["TaoMoi"]._href, ToNhomController.LstRole["TaoMoi"]._mangChucNang), // 1.3     
                        new SiteMenuItem("Quản lý tổ nhóm","", IndexMenu.M_3_2,ToNhomController.LstRole["QLToNhom"]._href, ToNhomController.LstRole["QLToNhom"]._mangChucNang), // 1.3     
                        new SiteMenuItem("Cấu hình duyệt","", IndexMenu.M_3_3,ToNhomController.LstRole["CauHinhDuyet"]._href, ToNhomController.LstRole["CauHinhDuyet"]._mangChucNang), // 1.3     
                    new SiteMenuItem("Nhập mã APP","menu-icon sanpham",IndexMenu.M_4,"#",new int[] { }),
                        new SiteMenuItem("Quản lý mã APP","", IndexMenu.M_4_1,SanPhamVayController.LstRole["QuanLySanPham"]._href, SanPhamVayController.LstRole["QuanLySanPham"]._mangChucNang), // 1.3     
                        new SiteMenuItem("Import","", IndexMenu.M_4_2,SanPhamVayController.LstRole["Import"]._href, SanPhamVayController.LstRole["Import"]._mangChucNang) ,// 1.3  
                    new SiteMenuItem("Nhân sự","menu-icon employee",IndexMenu.M_6,EmployeeController.LstRole["Index"]._href,new int[] { }), 
                        new SiteMenuItem("Thêm mới","", IndexMenu.M_6_1,EmployeeController.LstRole["AddNew"]._href, EmployeeController.LstRole["AddNew"]._mangChucNang) // 1.3  
            };
            //var isTeamLead = new NhomBLL().CheckIsTeamlead(GlobalData.User.IDUser);
            var isAdmin = new GroupBusiness().CheckIsAdmin(GlobalData.User.IDUser);
            if (isAdmin)
            {
                _siteMenu = _siteMenu.Concat(siteMenuAdmin).ToList();
            }
            StringBuilder menuHtml = new StringBuilder();

            menuHtml.Append("<ul class='nav nav-list'>");            

            bool openMenuLv1 = false;
            bool openMenuLv2 = false;
            bool openMenuLv3 = false;
            StringBuilder menuTree = new StringBuilder();
            StringBuilder menuTreeLv2 = new StringBuilder();
            StringBuilder menuTreeLv3 = new StringBuilder();
            bool addMenuTree = false; bool addMenuTreev2 = false;
            for (int i = 0; i < _siteMenu.Count; i++)
            {
                string[] indexes = _siteMenu[i].Index.Split(new string[] { "_" }, StringSplitOptions.RemoveEmptyEntries);

                if (indexes.Length == 1)
                {
                    if (openMenuLv3 == true)
                    {
                        menuTreeLv3.Append("</ul>");
                        menuTreeLv3.Append("</li>");
                        menuTreeLv2.Append(menuTreeLv3.ToString());
                        menuTreeLv3.Remove(0, menuTreeLv3.Length);
                        openMenuLv3 = false;
                        addMenuTreev2 = false;
                    }
                    
                    if (openMenuLv2 == true)
                    {
                        menuTree.Append(menuTreeLv2.ToString());
                        menuTreeLv2.Remove(0, menuTreeLv2.Length);
                        menuTree.Append("</ul>");
                        openMenuLv2 = false;
                    }
                    if (openMenuLv1 == true)
                    {
                        menuTree.Append("</li>");
                        openMenuLv1 = false;
                        if (addMenuTree == true)
                        {
                            menuHtml.Append(menuTree.ToString());
                            addMenuTree = false;
                        }
                        menuTree.Remove(0, menuTree.Length);
                    }
                    menuTree.Append("<li class=\"hover\">");
                    if(_siteMenu[i].Href=="#")
                        menuTree.Append("<a href='" + _siteMenu[i].Href + "' class='dropdown-toggle'>");// 
                    else
                        menuTree.Append("<a href='/"+ _siteMenu[i].Href+ "' >");// 
                    //menuTree.Append("<i style='margin: 1px 0 1px; display: inline-block;' class='"+ _siteMenu[i].Icon+"'></i>");//menu-icon fa fa fa-list-alt
                    menuTree.Append("<i  class='" + _siteMenu[i].Icon + "'></i>");
                    menuTree.Append("<span  class='menu-text'>");
                    //menu-icon fa fa fa-list-alt
                    menuTree.Append(_siteMenu[i].Text);
                    menuTree.Append("</span>");
                    menuTree.Append("<b class='arrow  fa fa-angle-down'></b>");
                    menuTree.Append("</a>");                                        
                    openMenuLv1 = true;
                }
                else if (indexes.Length == 2)
                {
                    if (openMenuLv3 == true)
                    {
                        menuTreeLv3.Append("</ul>");
                        menuTreeLv3.Append("</li>");
                        menuTreeLv2.Append(menuTreeLv3.ToString());
                        menuTreeLv3.Remove(0, menuTreeLv3.Length);
                        openMenuLv3 = false;
                    }
                    else menuTreeLv3.Remove(0, menuTreeLv3.Length);
                    if(addMenuTree==false)
                        openMenuLv2 = false;
                    if (Menu.checkMenuItem(_siteMenu[i].functionList) == true)
                    {
                        if (openMenuLv2 == false)
                            menuTreeLv2.Append("<b class=\"arrow\"></b><ul class='submenu'>");
                        menuTreeLv2.Append("<li class='hover' id='" + _siteMenu[i].Index + "'>");
                        menuTreeLv2.Append("<a  href='/" + _siteMenu[i].Href + "'>");
                        menuTreeLv2.Append("<i class='menu-icon fa fa-caret-right'></i>");
                        menuTreeLv2.Append(_siteMenu[i].Text);
                        menuTreeLv2.Append("</a>");
                        menuTreeLv2.Append("<b class='arrow'></b>");
                        menuTreeLv2.Append("</li>");
                        openMenuLv2 = true;
                        addMenuTree = true;
                        
                    }
                    else if(_siteMenu[i].Href=="#")
                     {
                         if (addMenuTreev2 == false)
                         {
                             //openMenuLv2 = false;
                             menuTreeLv3.Remove(0, menuTreeLv3.Length);
                         }
                         if (openMenuLv2 == false)
                             menuTreeLv3.Append("<ul class='submenu'>");
                         menuTreeLv3.Append("<li id='" + _siteMenu[i].Index + "'>");
                         menuTreeLv3.Append("<a  href='" + _siteMenu[i].Href + "' class='dropdown-toggle'>");
                         menuTreeLv3.Append("<i class='menu-icon fa fa-caret-right'></i>");
                         menuTreeLv3.Append(_siteMenu[i].Text);
                         menuTreeLv3.Append("<b class='arrow fa fa-angle-down'></b>");
                         menuTreeLv3.Append("</a>");
                         menuTreeLv3.Append("<b class='arrow '></b>");
                         openMenuLv2 = true;
                        
                     }
                }
                else if (indexes.Length == 3)
                {
                    if (Menu.checkMenuItem(_siteMenu[i].functionList) == true)
                    {
                        if (openMenuLv3 == false)
                            menuTreeLv3.Append("<ul class='submenu'>");
                        menuTreeLv3.Append("<li id='" + _siteMenu[i].Index + "'>");
                        menuTreeLv3.Append("<a  href='/" + _siteMenu[i].Href + "'>");
                        menuTreeLv3.Append("<i class='menu-icon fa fa-leaf green'></i>");
                        menuTreeLv3.Append(_siteMenu[i].Text);
                        menuTreeLv3.Append("</a>");
                        menuTreeLv3.Append("<b class='arrow'></b>");
                        menuTreeLv3.Append("</li>");
                        addMenuTree = true;
                        openMenuLv3 = true;
                        addMenuTreev2 = true;
                    }
                }
                
            }
            
            if (openMenuLv3 == true)
            {
                menuTreeLv3.Append("</ul>");
                menuTreeLv3.Append("</li>");
                menuTreeLv2.Append(menuTreeLv3.ToString());
                menuTreeLv3.Remove(0, menuTreeLv3.Length);
                openMenuLv3 = false;
            }
            
            if (openMenuLv2 == true)
            {
                menuTree.Append(menuTreeLv2.ToString());
                menuTreeLv2.Remove(0, menuTreeLv2.Length);
                menuTree.Append("</ul>");
                openMenuLv2 = false;
            }
            if (openMenuLv1 == true)
            {
                menuTree.Append("</li>");
                openMenuLv1 = false;
                if (addMenuTree == true)
                {
                    menuHtml.Append(menuTree.ToString());
                    addMenuTree = false;
                }
            }            
            menuHtml.Append("</ul>");            

            return menuHtml.ToString();
        }
    }
}
