using FriCarRent.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FriCarRent.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            
            return View();
        }
        
        public ActionResult Logout()
        {
            HttpContext.Session.Clear(); 
            return Redirect(Request.Headers["Referer"].ToString() ?? "/");
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
