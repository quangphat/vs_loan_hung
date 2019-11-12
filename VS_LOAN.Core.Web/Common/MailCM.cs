
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Net.Mail;
using System.IO;
using VS_LOAN.Core.Business;
using VS_LOAN.Core.Entity.Model;
using System.Web.Http.Results;
using System.Web.Mvc;
using System.Threading;
using VS_LOAN.Core.Utility;

namespace VS_LOAN.Core.Web.Common
{
    public class MailCM
    {
        private static string hostLink = "http://portal.vietbankfc.vn/";
        public static void SendMailToAdmin(int id,string url)
        {
            try
            {
                var hoSo = new HoSoBLL().LayChiTiet(id);
                if (hoSo != null)
                {
                   var lstUserApprove = new NhanVienNhomBLL().LayDSNhanVien(hoSo.HoSoCuaAi, (int)QuyenIndex.DuyetHoSo);
                    if (lstUserApprove != null)
                    {
                        foreach (var item in lstUserApprove)
                        {
                            string mailTo = "";
                            string subject = "[FINTECHCOM]{0}";
                            string mailBody = "";
                            string genderSend = "";
                            UserPMModel userSend = new UserPMBLL().GetUserByID(hoSo.HoSoCuaAi.ToString());
                            UserPMModel userReceive = item;
                            mailTo = userReceive.Email;
                            string genderReceive = "";
                            string template = "";
                            mailBody += "<p style=\"margin-left:20px;margin-top:20px\"><font size=\"2.5\" face=\"Arial\">Dear{0} {1}\r\n\r\n";
                            mailBody += "Bạn được chọn để duyệt hồ sơ bởi {2} {3}.\r\n";
                            mailBody += "Hồ sơ: {4}\r\n";
                            mailBody += "Mã hồ sơ: <a href='" + url + "/DuyetHoSo/XemHSByID/" + id + "'>{5}</a>\r\n\r\n";
                            mailBody += "Vui lòng click vào link bên dưới {6} thực hiện việc kiểm duyệt.\r\n\r\n";
                            mailBody += "http://" + url + "/DuyetHoSo/XemHSByID/" + id + ".\r\n\r\n";
                            mailBody += "Đây là một email được tạo ra hệ thống. Xin vui lòng không trả lời tin nhắn này.\r\n";
                            //mailBody += "Nếu bạn có bất kỳ thắc mắc, xin vui lòng liên hệ {7}\r\n";
                            mailBody += "Trân trọng,\r\n\r\n";
                            mailBody += "VIETBANK – HỆ THỐNG KIỂM DUYỆT HỒ SƠ\r\n\r\n</font></p>";
                            template += "<html><table style=\"width: 700px\"  cellpadding=\"0\" cellspacing=\"0\"><tr style =\"background-color:#0D622F\"><td style =\"width:15%;float:left\"><img style=\"width:120px;margin-left: 20px;margin-top: 20px;\" src ='" + CMLink.PathLogo + "'></td><td style=\"width:85%\"><p style=\"float:left; margin: 10px 0px 10px 15px\"><font size=\"6\" face=\"VIETBANK – HỆ THỐNG KIỂM DUYỆT HỒ SƠ\" style=\"color:white\">HỆ THỐNG KIỂM DUYỆT HỒ SƠ</font></p ></td></tr>";
                            template += "<tr style=\"background-color:#61ec66\" align=\"left\" ><td colspan='2'>###</td></tr>";
                            template += "</table></html>";
                            mailBody = mailBody.Replace("\r\n", "<br>");
                            mailBody = string.Format(mailBody, genderReceive, userReceive.FullName, genderSend, userSend.FullName, hoSo.TenKhachHang, hoSo.MaHoSo, "đây");
                            template = template.Replace("###", mailBody);
                            SendMail(mailTo, string.Format(subject, hoSo.TenKhachHang), hoSo.TenKhachHang, template);
                        }
                    }
                    
                }
            }
            catch
            {

            }
        }
      
        //public static void SendMailTToIT(int requestID, string url)
        //{
        //    try
        //    {
        //        var requestModel = new RequestBLL().GetRequestByID(requestID);
        //        if (requestModel != null)
        //        {
        //            string mailTo = "";
        //            string subject = "[iREQUEST – INNOVATION REQUEST SYSTEM]{0}";
        //            string genderSend = "";
        //            UserPMModel userSend = new UserPMBLL().GetUserByID(requestModel.CreatorID);
        //            List<UserPMModel> lstInnovation = new UserPMBLL().GetList((int)GroupUser.INPM);
        //            if (lstInnovation != null)
        //            {
        //                foreach (var item in lstInnovation)
        //                {
        //                    string template = "";
        //                    string mailBody = "";
        //                    UserPMModel userReceive = new UserPMBLL().GetUserByID(item.IDUser);
        //                    mailTo = userReceive.Email;
        //                    string genderReceive = "";
        //                    mailBody += "<p style=\"margin-left:20px;margin-top:20px\"><font size=\"2.5\" face=\"Arial\">Dear Innovation team,\r\n\r\n";
        //                    mailBody += "There is a request waiting for Innovation approval.\r\n";
        //                    mailBody += "Request name: {0}\r\n";
        //                    mailBody += "Request code: <a href='" + url + "/DiscussRequest/EditByID/" + requestID + "'>{1}</a>\r\n\r\n";
        //                    mailBody += "Please click <a href='" + url + "/DiscussRequest/EditByID/" + requestID + "'>{2}</a> to take necessary actions.\r\n\r\n";
        //                    mailBody += "This is a system generated email. Please do not respond to this message.\r\n";
        //                    mailBody += "If you have any queries, please contact {3}\r\n";
        //                    mailBody += "Sincerely,\r\n\r\n";
        //                    mailBody += "iREQUEST – INNOVATION REQUEST SYSTEM\r\n\r\n</font></p>";
        //                    mailBody = mailBody.Replace("\r\n", "<br>");
        //                    template += "<html><table style=\"width: 700px\"  cellpadding=\"0\" cellspacing=\"0\"><tr style =\"background-color:#08088A\"><td style =\"width:15%;float:left\"><img style=\"width:20px;margin-left: 20px;margin-top: 20px;\"src =\"http:/" + url + "/Content/images/kpmgLogo.png\"></td><td style=\"width:85%\"><p style=\"float:left; margin: 10px 0px 10px 15px\"><font size=\"6\" face=\"KPMG light\" style=\"color:white\">iREQUEST System</font></p ></td></tr>";
        //                    template += "<tr style=\"background-color:#BDD6EE\" align=\"left\" ><td colspan='2'>###</td></tr>";
        //                    template += "</table></html>";
        //                    mailBody = string.Format(mailBody, requestModel.Name, requestModel.RequestCode, "here", "trucnguyen1@kpmg.com.vn");
        //                    template = template.Replace("###", mailBody);
        //                    var task = SendMail(mailTo, subject, requestModel.Name, template);

        //                }
        //            }
        //        }
        //    }
        //    catch
        //    {
        //    }
        //}
        ///// <summary>
        ///// //SonNX
        ///// //17/4/2019
        ///// Send mail to assigned staff
        ///// </summary>
        ///// <param name="requestID"></param>
        ///// <param name="url"></param>
        ///// <param name="userID"></param>
        //public static void SendMailToAssignedStaff(int requestID, string url,int userID)
        //{
        //    try
        //    {
        //        var requestModel = new RequestBLL().GetRequestByID(requestID);
        //        if (requestModel != null)
        //        {
        //            string template = "";
        //            string mailTo = "";
        //            string subject = "[iREQUEST – INNOVATION REQUEST SYSTEM]{0}";
        //            string mailBody = "";
        //            UserPMModel userSend = new UserPMBLL().GetUserByID(GlobalData.User.IDUser);
        //            UserPMModel userReceive = new UserPMBLL().GetUserByID(userID);
        //            mailTo = userReceive.Email;
        //            string genderReceive = "";
        //            mailBody += "<p style=\"margin-left:20px;margin-top:20px\"><font size=\"2.5\" face=\"Arial\">Dear{0} {1}\r\n\r\n";
        //            mailBody += " You are chose by {2} to become a member of one request in iRequest system.\r\nRequest name: {3}\r\n";
        //            mailBody += " Request code: <a href='" + url + "/AssignedRequest/IndexByID/" + requestID + "'>{4}</a>\r\n\r\n";
        //            mailBody += " Please click on <a href='" + url + "/AssignedRequest/IndexByID/" + requestID + "'>{5}</a> to take necessary actions.\r\n\r\n";
        //            mailBody += " This is a system generated email. Please do not respond to this message. \r\n\r\n";
        //            mailBody += " If you have any queries, please contact : {6}\r\n";
        //            mailBody += " Sincerely, \r\n\r\n";
        //            mailBody += " iREQUEST – INNOVATION REQUEST SYSTEM\r\n\r\n</font></p>";
        //            mailBody = mailBody.Replace("\r\n", "<br>");
        //            template += "<html><table style=\"width: 700px\"  cellpadding=\"0\" cellspacing=\"0\"><tr style =\"background-color:#08088A\"><td style =\"width:15%;float:left\"><img style=\"width:20px;margin-left: 20px;margin-top: 20px;\"src =\"http:/" + url + "/Content/images/kpmgLogo.png\"></td><td style=\"width:85%\"><p style=\"float:left; margin: 10px 0px 10px 15px\"><font size=\"6\" face=\"KPMG light\" style=\"color:white\">iREQUEST System</font></p ></td></tr>";
        //            template += "<tr style=\"background-color:#BDD6EE\" align=\"left\" ><td colspan='2'>###</td></tr>";
        //            template += "</table></html>";
        //            mailBody = string.Format(mailBody, genderReceive, userReceive.FullName, GlobalData.User.FullName, requestModel.Name, requestModel.RequestCode, "here", "trucnguyen1@kpmg.com.vn");
        //            template = template.Replace("###", mailBody);
        //            var task = SendMail(mailTo, subject, requestModel.Name, template);
        //        }
        //    }
        //    catch
        //    {
        //    }
        //}

        ///// <summary>
        ///// SonNX
        ///// 17/4/2019
        ///// Send mail to Technical lead
        ///// </summary>
        ///// <param name="requestID"></param>
        ///// <param name="url"></param>
        ///// <param name="userID"></param>
        //public static void SendMailToLeader(int requestID, string url,int userID)
        //{
        //    try
        //    {
        //        var requestModel = new RequestBLL().GetRequestByID(requestID);
        //        if (requestModel != null)
        //        {
        //            UserPMModel userReceive = new UserPMBLL().GetUserByID(userID);
        //            string subject = "[iREQUEST – INNOVATION REQUEST SYSTEM]{0}";
        //            List<MailAddress> mailTo = new List<MailAddress>();
        //            List<MailAddress> ccList = new List<MailAddress>();
        //            MailAddress mailTotmp = new MailAddress(userReceive.Email);
        //            mailTo.Add(mailTotmp);
        //            UserPMModel userSend = new UserPMBLL().GetUserByID(GlobalData.User.IDUser);
        //            List<UserPMModel> lstInnovation = new UserPMBLL().GetList((int)GroupUser.INPM);
        //            if (lstInnovation != null)
        //            {
        //                foreach (var item in lstInnovation)//check another in lstInovation
        //                {
        //                    if(item.IDUser != GlobalData.User.IDUser)
        //                    {
                                
        //                        MailAddress tmp = new MailAddress(item.Email);
        //                        ccList.Add(tmp);
        //                    }
        //                }
        //            }
        //            string template = "";
        //            string mailBody = "";
        //            MailAddress mailFrom = new MailAddress(userSend.Email);
        //            string genderReceive = "";
        //            mailBody += "<p style=\"margin-left:20px;margin-top:20px\"><font size=\"2.5\" face=\"Arial\">Dear{0} {1}\r\n\r\n";
        //            mailBody += "You are chose to become a Technical lead for one request in the iRequest system.\r\n ";
        //            mailBody += "Request name: {2}\r\n";
        //            mailBody += "Request code: <a href='" + url + "/AssignedRequest/IndexByID/" + requestID + "'>{3}</a>\r\n\r\n";
        //            mailBody += "Please click on <a href='" + url + "/AssignedRequest/IndexByID/" + requestID + "'>{4}</a> to take necessary actions.\r\n\r\n";
        //            mailBody += "This is a system generated email. Please do not respond to this message. \r\n";
        //            mailBody += "If you have any queries, please contact : {5}\r\n";
        //            mailBody += "Sincerely,\r\n\r\n";
        //            mailBody += "iREQUEST – INNOVATION REQUEST SYSTEM\r\n\r\n</font></p>";
        //            mailBody = mailBody.Replace("\r\n", "<br>");
        //            template += "<html><table style=\"width: 700px\"  cellpadding=\"0\" cellspacing=\"0\"><tr style =\"background-color:#08088A\"><td style =\"width:15%;float:left\"><img style=\"width:20px;margin-left: 20px;margin-top: 20px;\"src =\"http:/" + url + "/Content/images/kpmgLogo.png\"></td><td style=\"width:85%\"><p style=\"float:left; margin: 10px 0px 10px 15px\"><font size=\"6\" face=\"KPMG light\" style=\"color:white\">iREQUEST System</font></p ></td></tr>";
        //            template += "<tr style=\"background-color:#BDD6EE\" align=\"left\" ><td colspan='2'>###</td></tr>";
        //            template += "</table></html>";
        //            mailBody = string.Format(mailBody, genderReceive, userReceive.FullName, requestModel.Name, requestModel.RequestCode, "here", "trucnguyen1@kpmg.com.vn");
        //            template = template.Replace("###", mailBody);
        //            var task = SendMail(mailFrom, mailTo, ccList, string.Format(subject, requestModel.Name), template);
        //        }
        //    }
        //    catch
        //    {
        //    }
        //}
        ///// <summary>
        ///// //SonNX
        ///// //18/04/2019
        ///// //Send email to creator when submitting idea
        ///// </summary>
        ///// <param name="requestID"></param>

        //public static void SendMailToCreatorIdea(int userID, string url)
        //{
        //    string html = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory+ "\\Content\\TemplatesEmail\\SendToIDea.html");
        //    string mailTo = "";
        //    string subject = "[iREQUEST – INNOVATION REQUEST SYSTEM]{0}";
        //    //string mailBody = "";
        //    UserPMModel userReceive = new UserPMBLL().GetUserByID(userID);
        //    mailTo = userReceive.Email;
        //    //string genderReceive = "";
        //    //mailBody += "<p style='font-size:9.5pt;line-height:150%;font-family:\"Arial\",sans-serif;color:#00B0F0'>Dear{0} {1}\r\n\r\n";
        //    //mailBody += "Thank you for sharing your idea!\r\n\r\n";
        //    //mailBody += "Your prompt act of sharing ideas with us is a meaningful step to build an innovation culture at KPMG. We will carefully review it and get back to you shortly. Together we can do great things. Stay stunned for that!\r\n\r\n";
        //    //mailBody += "And <b style=\"color:#0089BA\">CONGRATULATION!</b> You now win a chance to receive a reward with <b style=\"color:#0089BA\">10 stars</b>";
        //    //mailBody += "<img width=19 height=19 src=\"\">";
        //    //mailBody += "if your idea is chosen for the<b>“Ideas of the month”</b> list. The result will be announced every month to all participants.\r\n\r\n";
        //    //mailBody += "Please note that this is a system generated email. Do not reply to this email address";
        //    //mailBody += "If you have any queries, please contact : {2}\r\n\r\n</p>";
        //    //mailBody = mailBody.Replace("\r\n", "<br>");
        //    //mailBody = string.Format(mailBody, genderReceive, userReceive.FullName, "vinhtdo@kpmg.com.vn");
        //    string linktmp = "http://"+url + "/Content/TemplatesEmail/Thank you.jpg";
        //    html = html.Replace("[0]", linktmp);
        //    html = html.Replace("[1]", userReceive.FullName);
        //    html = html.Replace("[2]", "http://" + url + "/Content/TemplatesEmail/star.png");
        //    var task = SendMail(mailTo, subject, "THANK YOU LETTER", html);
        //}



        public  static void  SendMail(string mailTo,string subject, string name, string mailBody)
        {
            var message = new MailMessage();
            message.From = new MailAddress("FINTECHCOM<" + "system@fintechcom.vn" + ">");
            message.To.Add(new MailAddress(mailTo));//
            message.Subject = subject;
            message.Body = mailBody;
            message.IsBodyHtml = true;
            var smtp = new SmtpClient("webmail.fintechcom.vn", 25);
            {
                try
                {
                    smtp.Credentials = new System.Net.NetworkCredential("system@fintechcom.vn", "vinh240390");
                    smtp.EnableSsl = false;
                    Thread thread = new Thread(() => smtp.Send(message));
                    thread.Start();

                }
                catch (System.Net.Mail.SmtpException ex)
                {
                    throw ex;
                }
            }
            //var message = new MailMessage();
            //message.From = new MailAddress("fintechcomhcm@gmail.com");
            //message.To.Add(new MailAddress("tronghung1993@gmail.com"));//
            //message.Bcc.Add(new MailAddress("trongsanguit@gmail.com"));//
            //message.Subject = subject;
            //message.Body = mailBody;
            //message.IsBodyHtml = true;
            //var smtp = new SmtpClient("smtp.gmail.com", 587);
            //{
            //    try
            //    {
            //        smtp.Credentials = new System.Net.NetworkCredential("fintechcomhcm@gmail.com", "Sctv@123");
            //        smtp.EnableSsl = true;
            //        // await smtp.SendMailAsync(message);
            //        Thread thread = new Thread(() => smtp.Send(message));
            //        thread.Start();

            //    }
            //    catch (System.Net.Mail.SmtpException ex)
            //    {
            //        throw ex;
            //    }
            //}

        }


    }
}