using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TheRichLifeProject.Models;

namespace TheRichLifeProject.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            try
            {
                ViewBag.Role = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value;
            }
            catch
            {
                ViewBag.Role = "You are not logged in";
            }
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
