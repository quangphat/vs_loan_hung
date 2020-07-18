using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VietStar.Client.Models;

namespace VietStar.Client.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            
            HttpContext.Session.SetString("username", "quangphat");
            return View();
        }

        public IActionResult Privacy()
        {
            var username = HttpContext.Session.GetString("username");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
