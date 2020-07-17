
using VS_LOAN.Core.Repository;
using VS_LOAN.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using VS_LOAN.Core.Entity;

namespace VS_LOAN.Core.Web.Helpers
{
    public class RedirectLink
    {
        public string Link { get; set;}
        public string Title { get; set; }
    }
        
    public class MenuWarning
    {
        public static int[] arrayQuyenTop = new int[] { (int)QuyenIndex.DuyetHoSo};
        public static string GetMenuTopCanhBao()
        {
            StringBuilder menuHtml = new StringBuilder();

            DateTime tuNgay = DateTime.Now.AddDays(-50);
            DateTime denNgay = DateTime.Now.AddDays(10);
            foreach (var quyen in arrayQuyenTop)
            {
                if (checkQuyen(quyen))
                {
                    switch (quyen)
                    {

                        case (int)QuyenIndex.DuyetHoSo:
                            List<RedirectLink> lstContent = new List<RedirectLink>();
                            string trangthai = "";
                            trangthai += ((int)TrangThaiHoSo.TuChoi).ToString() + "," + ((int)TrangThaiHoSo.NhapLieu).ToString() + "," + ((int)TrangThaiHoSo.ThamDinh).ToString() + "," + ((int)TrangThaiHoSo.BoSungHoSo).ToString() + "," + ((int)TrangThaiHoSo.GiaiNgan).ToString();
                            var lstNotApprove = new HoSoBLL().DSHoSoDuyetChuaXem(GlobalData.User.IDUser, 0, 0, tuNgay, denNgay, string.Empty, string.Empty, 1, trangthai);
                            if (lstNotApprove != null)
                            {
                                foreach (var item in lstNotApprove)
                                {
                                    RedirectLink rl = new RedirectLink();

                                    rl.Title = "[" + item.MaHoSo + "] " + item.TenKH;
                                    rl.Link = "/DuyetHoSo/XemHSByID/" + item.ID;
                                    lstContent.Add(rl);
                                }
                            }
                            menuHtml.Append(ListNotApprove(lstContent, "fa fa-envelope", "icon-animated-vertical", "/DuyetHoSo/Index", "Hồ sơ chưa duyệt"));
                            break;
                            //case (int)QuyenIndex.Public:
                            //    var lstRequetDoNotBrowse = new RequestBLL().GetMyProgress(GlobalData.User.IDUser, ((int)Utility.RequestStatus.DoNotBrowse).ToString());
                            //    lstContent = lstRequetDoNotBrowse.Select(x => x.RequestCode).ToList();
                            //    menuHtml.Append(ListDoNotBrowse(lstContent, "fa fa-envelope", "icon-animated-vertical", "/MyProcessRequest/Index", Resources.Global.MenuWarning_MyRequest_DoNotRequest));
                            //    break;

                    }
                }
            }
            if (GlobalData.User != null)
            {
                List<RedirectLink> lstContent = new List<RedirectLink>();

                string trangthai = "";
                //trangthai += "," + ((int)TrangThaiHoSo.TuChoi).ToString();
                var lstDoNotBrowse = new HoSoBLL().DSHoSoChuaXem(GlobalData.User.IDUser, tuNgay, denNgay,string.Empty, string.Empty, trangthai);
                if (lstDoNotBrowse != null)
                {
                    foreach (var item in lstDoNotBrowse)
                    {
                        RedirectLink rl = new RedirectLink();
                        rl.Title = "[" + item.MaHoSo + "] " + item.TenKH;
                        rl.Link = "/QuanLyHoSo/XemHSByID/" + item.ID;
                        lstContent.Add(rl);
                    }
                }
                menuHtml.Append(ListDoNotBrowse(lstContent, "fa fa-bell", "icon-animated-vertical", "/QuanLyHoSo/DanhSachHoSo", "Thông báo chưa xem"));
            }
            return menuHtml.ToString();
        }
        protected static bool checkQuyen(int maChucNang)
        {
            if (GlobalData.User == null)
                return false;
            List<string> lstQuyen = new List<string>();
            lstQuyen.Add(GlobalData.Rules);
            if (lstQuyen != null)
            {
                if (lstQuyen.Count > 0)
                {
                    foreach (var item in lstQuyen)
                    {
                        string quyen = item.Trim();
                        int kt = (quyen.Length - (maChucNang - 1) / 4) - 1;
                        if (kt >= 0 && kt < quyen.Length)
                        {
                            string maQuyen = quyen[kt].ToString();
                            string gt = Convert.ToString(Convert.ToInt32(maQuyen.ToString(), 16), 2).PadLeft(4, '0');

                            if (gt[gt.Length - (maChucNang - 1) % 4 - 1] == '1')
                            {
                                return true;
                            }
                        }
                    }
                }

            }
            return false;
        }
        public static string ListNotApprove(List<RedirectLink> lstContent, string icon, string animate, string href, string text)
        {
            if (lstContent.Count == 0)
                return "";
            StringBuilder htmlContent = new StringBuilder();
            htmlContent.Append("<li class='green'>");
            htmlContent.Append("<a title='" + text + "' data-toggle='dropdown' class='dropdown-toggle' href='#'>");
            htmlContent.Append("<i class='ace-icon " + icon + " " + animate + "'></i>");
            htmlContent.Append("<span class='badge badge-warning'>" + lstContent.Count + "</span>");
            htmlContent.Append("</a>");

            htmlContent.Append("<ul class='dropdown-menu-right dropdown-navbar navbar-green dropdown-menu dropdown-caret dropdown-close'>");
            htmlContent.Append("<li class='dropdown-header'>");
            htmlContent.Append("<i class='ace-icon " + icon + "'></i>");
            htmlContent.Append(lstContent.Count + " " + text);
            htmlContent.Append("</li>");

            htmlContent.Append("<li class='dropdown-content'>");
            htmlContent.Append("<ul class='dropdown-menu dropdown-navbar'>");
            foreach (var item in lstContent)
            {
                htmlContent.Append("<li>");
                htmlContent.Append("<a href='" + item.Link + "' class='clearfix'>");
                htmlContent.Append("<span class='msg-body' style='margin-left:0px;>");
                htmlContent.Append("<span class='msg-title'>");
                htmlContent.Append(item.Title);
                htmlContent.Append("</span>");
                htmlContent.Append("</span>");
                htmlContent.Append("</a>");
                htmlContent.Append("</li>");
            }


            htmlContent.Append("</ul>");
            htmlContent.Append("</li>");

            htmlContent.Append("<li class='dropdown-footer'>");
            htmlContent.Append("<a href='" + href + "'>");
            htmlContent.Append("View all");
            htmlContent.Append("<i class='ace-icon fa fa-arrow-right'></i>");
            htmlContent.Append("</a>");
            htmlContent.Append("</li>");
            htmlContent.Append("</ul>");
            htmlContent.Append("</li>");
            return htmlContent.ToString();
        }
        public static string DSChuaXuatBan(List<RedirectLink> lstContent, string icon, string animate, string href, string text)
        {
            if (lstContent.Count == 0)
                return "";
            StringBuilder htmlContent = new StringBuilder();
            htmlContent.Append("<li class='green'>");
            htmlContent.Append("<a title='" + text + "' data-toggle='dropdown' class='dropdown-toggle' href='#'>");
            htmlContent.Append("<i class='ace-icon " + icon + " " + animate + "'></i>");
            htmlContent.Append("<span class='badge badge-warning'>" + lstContent.Count + "</span>");
            htmlContent.Append("</a>");

            htmlContent.Append("<ul class='dropdown-menu-right dropdown-navbar navbar-green dropdown-menu dropdown-caret dropdown-close'>");
            htmlContent.Append("<li class='dropdown-header'>");
            htmlContent.Append("<i class='ace-icon " + icon + "'></i>");
            htmlContent.Append(lstContent.Count + " " + text);
            htmlContent.Append("</li>");

            htmlContent.Append("<li class='dropdown-content'>");
            htmlContent.Append("<ul class='dropdown-menu dropdown-navbar'>");
            foreach (var item in lstContent)
            {
                htmlContent.Append("<li>");
                htmlContent.Append("<a href='" + item.Link + "' class='clearfix'>");
                htmlContent.Append("<span class='msg-body' style='margin-left:0px;>");
                htmlContent.Append("<span class='msg-title'>");
                htmlContent.Append(item.Title);
                htmlContent.Append("</span>");
                htmlContent.Append("</span>");
                htmlContent.Append("</a>");
                htmlContent.Append("</li>");
            }


            htmlContent.Append("</ul>");
            htmlContent.Append("</li>");

            htmlContent.Append("<li class='dropdown-footer'>");
            htmlContent.Append("<a href='" + href + "'>");
            htmlContent.Append("View all");
            htmlContent.Append("<i class='ace-icon fa fa-arrow-right'></i>");
            htmlContent.Append("</a>");
            htmlContent.Append("</li>");
            htmlContent.Append("</ul>");
            htmlContent.Append("</li>");
            return htmlContent.ToString();
        }

        public static string ListDoNotBrowse(List<RedirectLink> lstContent, string icon, string animate, string href, string text)
        {
            if (lstContent.Count == 0)
                return "";
            StringBuilder htmlContent = new StringBuilder();
            htmlContent.Append("<li class='red'>");
            htmlContent.Append("<a title='" + text + "' data-toggle='dropdown' class='dropdown-toggle' href='#'>");
            htmlContent.Append("<i class='ace-icon " + icon + " " + animate + "'></i>");
            htmlContent.Append("<span class='badge badge-inverse'>" + lstContent.Count + "</span>");
            htmlContent.Append("</a>");

            htmlContent.Append("<ul class='dropdown-menu-right dropdown-navbar navbar-pink dropdown-menu dropdown-caret dropdown-close'>");
            htmlContent.Append("<li class='dropdown-header'>");
            htmlContent.Append("<i class='ace-icon " + icon + "'></i>");
            htmlContent.Append(lstContent.Count + " " + text);
            htmlContent.Append("</li>");

            htmlContent.Append("<li class='dropdown-content'>");
            htmlContent.Append("<ul class='dropdown-menu dropdown-navbar'>");
            foreach (var item in lstContent)
            {
                htmlContent.Append("<li>");
                htmlContent.Append("<a href='" + item.Link + "' class='clearfix'>");
                htmlContent.Append("<span class='msg-body' style='margin-left:0px;>");
                htmlContent.Append("<span class='msg-title'>");
                htmlContent.Append(item.Title);
                htmlContent.Append("</span>");
                htmlContent.Append("</span>");
                htmlContent.Append("</a>");
                htmlContent.Append("</li>");
            }


            htmlContent.Append("</ul>");
            htmlContent.Append("</li>");

            htmlContent.Append("<li class='dropdown-footer'>");
            htmlContent.Append("<a href='" + href + "'>");
            htmlContent.Append("View all");
            htmlContent.Append("<i class='ace-icon fa fa-arrow-right'></i>");
            htmlContent.Append("</a>");
            htmlContent.Append("</li>");
            htmlContent.Append("</ul>");
            htmlContent.Append("</li>");
            return htmlContent.ToString();
        }


    }
}