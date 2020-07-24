using VS_LOAN.Core.Utility;
using VS_LOAN.Core.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using VS_LOAN.Core.Repository;
using VS_LOAN.Core.Web.Infrastructures;
using VS_LOAN.Core.Repository.Interfaces;
using VS_LOAN.Core.Entity;

namespace VS_LOAN.Core.Web.Helpers
{
    public class SiteMenuItem
    {
        public string Text = "";
        public string Index = "0";
        public string Href = "#";
        public string Icon = "";
        public int[] functionList = null;

        public SiteMenuItem(string text, string icon, string index, string href, int[] funcs)
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
        public IEmployeeRepository _rpEmployee;
        public Menu(IEmployeeRepository employeeRepository)
        {
            _rpEmployee = employeeRepository;
        }
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
            var orgId = GlobalData.User.OrgId;
            if (orgId == (int)Organization.RevokeDebt)
                return ReturnInvokeDebtMenu();
            return ReturnVietbankMenu();
        }
        protected static string ReturnInvokeDebtMenu()
        {
            List<SiteMenuItem> _siteMenu = new List<SiteMenuItem>{

                    new SiteMenuItem("Quản lý hồ sơ","menu-icon qlhs",IndexMenu.M_2,"#",new int[] { }),
                        new SiteMenuItem("Danh sách hồ sơ","", IndexMenu.M_2_2,ControllerRoles.Roles["profile_list"]._href, ControllerRoles.Roles["profile_list"]._mangChucNang), // 1.3      
            };
            List<SiteMenuItem> siteMenuAdmin = new List<SiteMenuItem>
            {
                  new SiteMenuItem("Tổ nhóm","menu-icon group",IndexMenu.M_3,"#",new int[] { }),
                        new SiteMenuItem("Tạo mới","", IndexMenu.M_3_1,ControllerRoles.Roles["team_addnew"]._href, ControllerRoles.Roles["team_addnew"]._mangChucNang), // 1.3     
                        new SiteMenuItem("Quản lý tổ nhóm","", IndexMenu.M_3_2,ControllerRoles.Roles["team_list"]._href,ControllerRoles.Roles["team_list"]._mangChucNang), // 1.3     
                        new SiteMenuItem("Cấu hình duyệt","", IndexMenu.M_3_3,ControllerRoles.Roles["team_config"]._href, ControllerRoles.Roles["team_config"]._mangChucNang), // 1.3     
                    new SiteMenuItem("Nhân sự","menu-icon employee",IndexMenu.M_6,ControllerRoles.Roles["employee_list"]._href,new int[] { }),
                        new SiteMenuItem("Thêm mới","", IndexMenu.M_6_1,ControllerRoles.Roles["employee_add"]._href,ControllerRoles.Roles["employee_add"]._mangChucNang) // 1.3  
            };
            
            if (GlobalData.User.UserType == (int)UserTypeEnum.Admin)
            {
                _siteMenu = _siteMenu.Concat(siteMenuAdmin).ToList();
            }
            return GenerateMenu(_siteMenu);
        }
        protected static string ReturnVietbankMenu()
        {
            List<SiteMenuItem> _siteMenu = new List<SiteMenuItem>{
                    new SiteMenuItem(Resources.Global.Menu_TC,"menu-icon home",IndexMenu.M_0,"#",new int[] { (int)QuyenIndex.Public }),
                        new SiteMenuItem("Giới thiệu","",IndexMenu.M_0_1,ControllerRoles.Roles["home_about"]._href,new int[] { (int)QuyenIndex.Public }),
                        //new SiteMenuItem("Hướng dẫn sử dụng","",IndexMenu.M_0_2,"#",new int[] { (int)QuyenIndex.Public }),
                        new SiteMenuItem("Phiên bản","",IndexMenu.M_0_2,ControllerRoles.Roles["home_version"]._href,new int[] { (int)QuyenIndex.Public }),
                    new SiteMenuItem("Tạo hồ sơ","menu-icon hs",IndexMenu.M_1,ControllerRoles.Roles["profile_addnew"]._href,new int[] { }),
                        new SiteMenuItem("Tạo mới","", IndexMenu.M_1_1,ControllerRoles.Roles["profile_addnew"]._href,ControllerRoles.Roles["profile_addnew"]._mangChucNang), // 1.1   
                        //new SiteMenuItem("Hồ sơ của tôi","", IndexMenu.M_1_2,HoSoController.LstRole["Index"]._href,HoSoController.LstRole["Index"]._mangChucNang), // 1.1 
                    new SiteMenuItem("Quản lý hồ sơ","menu-icon qlhs",IndexMenu.M_2,"#",new int[] { }),
                        new SiteMenuItem("Danh sách hồ sơ","", IndexMenu.M_2_2,ControllerRoles.Roles["profile_list"]._href, ControllerRoles.Roles["profile_list"]._mangChucNang), // 1.3     
                        new SiteMenuItem("Duyệt hồ sơ","", IndexMenu.M_2_3,ControllerRoles.Roles["profile_approve"]._href, ControllerRoles.Roles["profile_approve"]._mangChucNang) ,// 1.2  
                    new SiteMenuItem("Check Duplicate","menu-icon duplicate",IndexMenu.M_5,"#",new int[] { }),
                        new SiteMenuItem("Tạo mới","", IndexMenu.M_5_1,ControllerRoles.Roles["checkdup_addnew"]._href, ControllerRoles.Roles["checkdup_addnew"]._mangChucNang), // 1.3     
                        new SiteMenuItem("Danh sách","", IndexMenu.M_5_2,ControllerRoles.Roles["checkdup_list"]._href, ControllerRoles.Roles["checkdup_list"]._mangChucNang), // 1.2  
                    new SiteMenuItem("Công ty","menu-icon company",IndexMenu.M_8,"#",new int[] { }),
                        new SiteMenuItem("Tạo mới","", IndexMenu.M_8_1,ControllerRoles.Roles["company_addnew"]._href, ControllerRoles.Roles["company_addnew"]._mangChucNang), // 1.3     
                        new SiteMenuItem("Danh sách","", IndexMenu.M_8_2,ControllerRoles.Roles["company_list"]._href, ControllerRoles.Roles["company_list"]._mangChucNang), // 1.2  
                   new SiteMenuItem("Courrier","menu-icon courier",IndexMenu.M_7,"#",new int[] { }),
                        new SiteMenuItem("Tạo mới","", IndexMenu.M_7_1,ControllerRoles.Roles["courier_addnew"]._href, ControllerRoles.Roles["courier_addnew"]._mangChucNang), // 1.3     
                        new SiteMenuItem("Danh sách","", IndexMenu.M_7_2,ControllerRoles.Roles["courier_list"]._href, ControllerRoles.Roles["courier_list"]._mangChucNang), // 1.2  
                    new SiteMenuItem("MCredit","menu-icon courier",IndexMenu.M_8,"#",new int[] { }),
                        new SiteMenuItem("Check CAT","", IndexMenu.M_9_1,ControllerRoles.Roles["mcedit_checkcat"]._href, ControllerRoles.Roles["mcedit_checkcat"]._mangChucNang),
                        new SiteMenuItem("Check CIC","", IndexMenu.M_9_2,ControllerRoles.Roles["mcedit_checkcic"]._href, ControllerRoles.Roles["mcedit_checkcic"]._mangChucNang),
                        new SiteMenuItem("Check Duplicate","", IndexMenu.M_9_3,ControllerRoles.Roles["mcedit_checkdup"]._href, ControllerRoles.Roles["mcedit_checkdup"]._mangChucNang),
                        new SiteMenuItem("Check Status","", IndexMenu.M_9_4,ControllerRoles.Roles["mcedit_checkstatus"]._href, ControllerRoles.Roles["mcedit_checkstatus"]._mangChucNang),// 1.3  
                        new SiteMenuItem("Danh sách tạo mới","", IndexMenu.M_9_5,ControllerRoles.Roles["mcedit_list_temp"]._href, ControllerRoles.Roles["mcedit_list_temp"]._mangChucNang), // 1.2  
                        new SiteMenuItem("Danh sách MC","", IndexMenu.M_9_6,ControllerRoles.Roles["mcedit_list"]._href, ControllerRoles.Roles["mcedit_list"]._mangChucNang) // 1.2  
                 
                    
            };
            List<SiteMenuItem> siteMenuAdmin = new List<SiteMenuItem>
            {
                  new SiteMenuItem("Tổ nhóm","menu-icon group",IndexMenu.M_3,"#",new int[] { }),
                        new SiteMenuItem("Tạo mới","", IndexMenu.M_3_1,ControllerRoles.Roles["team_addnew"]._href, ControllerRoles.Roles["team_addnew"]._mangChucNang), // 1.3     
                        new SiteMenuItem("Quản lý tổ nhóm","", IndexMenu.M_3_2,ControllerRoles.Roles["team_list"]._href,ControllerRoles.Roles["team_list"]._mangChucNang), // 1.3     
                        new SiteMenuItem("Cấu hình duyệt","", IndexMenu.M_3_3,ControllerRoles.Roles["team_config"]._href, ControllerRoles.Roles["team_config"]._mangChucNang), // 1.3     
                    new SiteMenuItem("Nhập mã APP","menu-icon sanpham",IndexMenu.M_4,"#",new int[] { }),
                        new SiteMenuItem("Quản lý mã APP","", IndexMenu.M_4_1,ControllerRoles.Roles["product_list"]._href, ControllerRoles.Roles["product_list"]._mangChucNang), // 1.3     
                        new SiteMenuItem("Import","", IndexMenu.M_4_2,ControllerRoles.Roles["product_import"]._href, ControllerRoles.Roles["product_import"]._mangChucNang) ,// 1.3  
                    new SiteMenuItem("Nhân sự","menu-icon employee",IndexMenu.M_6,ControllerRoles.Roles["employee_list"]._href,new int[] { }),
                        new SiteMenuItem("Thêm mới","", IndexMenu.M_6_1,ControllerRoles.Roles["employee_add"]._href,ControllerRoles.Roles["employee_add"]._mangChucNang) // 1.3  
            };

            //var isTeamLead = new NhomBLL().CheckIsTeamlead(GlobalData.User.IDUser);
            var isAdmin = new GroupRepository().CheckIsAdmin(GlobalData.User.IDUser);
            if (isAdmin)
            {
                _siteMenu = _siteMenu.Concat(siteMenuAdmin).ToList();
            }
            return GenerateMenu(_siteMenu);
        }
        protected static string GenerateMenu(List<SiteMenuItem> menus)
        {

            StringBuilder menuHtml = new StringBuilder();

            menuHtml.Append("<ul class='nav nav-list'>");

            bool openMenuLv1 = false;
            bool openMenuLv2 = false;
            bool openMenuLv3 = false;
            StringBuilder menuTree = new StringBuilder();
            StringBuilder menuTreeLv2 = new StringBuilder();
            StringBuilder menuTreeLv3 = new StringBuilder();
            bool addMenuTree = false; bool addMenuTreev2 = false;
            for (int i = 0; i < menus.Count; i++)
            {
                string[] indexes = menus[i].Index.Split(new string[] { "_" }, StringSplitOptions.RemoveEmptyEntries);

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
                    if (menus[i].Href == "#")
                        menuTree.Append("<a href='" + menus[i].Href + "' class='dropdown-toggle'>");// 
                    else
                        menuTree.Append("<a href='/" + menus[i].Href + "' >");// 
                    //menuTree.Append("<i style='margin: 1px 0 1px; display: inline-block;' class='"+ _siteMenu[i].Icon+"'></i>");//menu-icon fa fa fa-list-alt
                    menuTree.Append("<i  class='" + menus[i].Icon + "'></i>");
                    menuTree.Append("<span  class='menu-text'>");
                    //menu-icon fa fa fa-list-alt
                    menuTree.Append(menus[i].Text);
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
                    if (addMenuTree == false)
                        openMenuLv2 = false;
                    if (Menu.checkMenuItem(menus[i].functionList) == true)
                    {
                        if (openMenuLv2 == false)
                            menuTreeLv2.Append("<b class=\"arrow\"></b><ul class='submenu'>");
                        menuTreeLv2.Append("<li class='hover' id='" + menus[i].Index + "'>");
                        menuTreeLv2.Append("<a  href='/" + menus[i].Href + "'>");
                        menuTreeLv2.Append("<i class='menu-icon fa fa-caret-right'></i>");
                        menuTreeLv2.Append(menus[i].Text);
                        menuTreeLv2.Append("</a>");
                        menuTreeLv2.Append("<b class='arrow'></b>");
                        menuTreeLv2.Append("</li>");
                        openMenuLv2 = true;
                        addMenuTree = true;

                    }
                    else if (menus[i].Href == "#")
                    {
                        if (addMenuTreev2 == false)
                        {
                            //openMenuLv2 = false;
                            menuTreeLv3.Remove(0, menuTreeLv3.Length);
                        }
                        if (openMenuLv2 == false)
                            menuTreeLv3.Append("<ul class='submenu'>");
                        menuTreeLv3.Append("<li id='" + menus[i].Index + "'>");
                        menuTreeLv3.Append("<a  href='" + menus[i].Href + "' class='dropdown-toggle'>");
                        menuTreeLv3.Append("<i class='menu-icon fa fa-caret-right'></i>");
                        menuTreeLv3.Append(menus[i].Text);
                        menuTreeLv3.Append("<b class='arrow fa fa-angle-down'></b>");
                        menuTreeLv3.Append("</a>");
                        menuTreeLv3.Append("<b class='arrow '></b>");
                        openMenuLv2 = true;

                    }
                }
                else if (indexes.Length == 3)
                {
                    if (Menu.checkMenuItem(menus[i].functionList) == true)
                    {
                        if (openMenuLv3 == false)
                            menuTreeLv3.Append("<ul class='submenu'>");
                        menuTreeLv3.Append("<li id='" + menus[i].Index + "'>");
                        menuTreeLv3.Append("<a  href='/" + menus[i].Href + "'>");
                        menuTreeLv3.Append("<i class='menu-icon fa fa-leaf green'></i>");
                        menuTreeLv3.Append(menus[i].Text);
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
