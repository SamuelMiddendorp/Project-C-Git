using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheRichLifeProject.Models;

namespace TheRichLifeProject.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // Check if a user is logged in or not
            if (User.Claims.Any())
            {
                ViewBag.Role = User.Claims.First(x => x.Type == ClaimTypes.Role).Value;
            }
            else
            {
                ViewBag.Role = "You are not logged in";
            }
            return View();
        }
        [Authorize(Roles = "Admin")]
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
