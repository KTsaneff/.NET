using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebShopDemo.Models;

namespace WebShopDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;

        public HomeController(ILogger<HomeController> _logger)
        {
            logger = _logger;
        }

        public IActionResult Index()
        {;
            this.HttpContext.Session.SetString("name", "UserName");

            return View();
        }

        public IActionResult Privacy()
        {
            string? name = this.HttpContext.Session.GetString("name");

            if (!string.IsNullOrEmpty(name))
            {
                return Ok(name);
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}