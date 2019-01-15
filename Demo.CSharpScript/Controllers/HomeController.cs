using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Demo.CSharpScript.Models;

namespace Demo.CSharpScript.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Redirect("/Demo/GetData?name='123'");
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Demo.CSharpScript";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "QQ:1124999434";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
