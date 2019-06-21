using VS_LOAN.Core.Utility;
using VS_LOAN.Core.Utility.Exceptions;
using VS_LOAN.Core.Web.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;

namespace VS_LOAN.Core.Web.Controllers
{
    public class FileController : Controller
    {
        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.Public })]
        public FileStreamResult GetPDF(string path)
        {
            try
            {
                if (path == string.Empty)
                    return null;
                FileStream fs = new FileStream(Server.MapPath(path), FileMode.Open, FileAccess.Read);
                Response.AppendHeader("content-disposition", "inline; filename=" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".pdf");
                return File(fs, MediaTypeNames.Application.Pdf);
            }
            catch (BusinessException ex)
            {
                return null;
            }
        }

        [CheckPermission(MangChucNang = new int[] { (int)QuyenIndex.Public })]
        public FileStreamResult GetFile(string path)
        {
            try
            {
                string mimeType = MimeMapping.GetMimeMapping(path);
                string ext = System.IO.Path.GetExtension(path);
                FileStream fs = new FileStream(Server.MapPath(path), FileMode.Open, FileAccess.Read);
                if (ext == ".pdf")
                    Response.AppendHeader("content-disposition", "inline; filename=" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ext);
                else
                    Response.AppendHeader("content-disposition", "attachment; filename=" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ext);
                return File(fs, mimeType);
            }
            catch (Exception ex)
            {
                Response.Redirect("/Notpage/Index");
                return null;
            }
        }
        public FileStreamResult GetFileNotLogin(string path)
        {
            try
            {
                string mimeType = MimeMapping.GetMimeMapping(path);
                string ext = System.IO.Path.GetExtension(path);
                FileStream fs = new FileStream(Server.MapPath(path), FileMode.Open, FileAccess.Read);
                if (ext == ".pdf")
                    Response.AppendHeader("content-disposition", "inline; filename=" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ext);
                else
                    Response.AppendHeader("content-disposition", "attachment; filename=" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ext);
                return File(fs, mimeType);
            }
            catch (Exception ex)
            {
                Response.Redirect("/Notpage/Index");
                return null;
            }
        }
    }
}
