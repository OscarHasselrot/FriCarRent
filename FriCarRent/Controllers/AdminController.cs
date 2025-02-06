using FriCarRent.Data;
using FriCarRent.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FriCarRent.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdmin adminRepository;

        public AdminController(IAdmin adminRepository)
        {
            this.adminRepository = adminRepository;
        }
        // GET: AdminController
        public ActionResult Index()
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                return View();
            }
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel loginVM)
        {

            if (!ModelState.IsValid)
            {
                return View(loginVM);
            }

            var user = adminRepository.GetByEmail(loginVM.Email);

            if (user == null || user.Password != loginVM.Password)
            {
                ModelState.AddModelError("", "Ogiltig epost eller lösenord.");
                return View(loginVM);

            }
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserName", user.FirstName + " " + user.LastName);
            HttpContext.Session.SetString("Role", "Admin");
            return RedirectToAction("Index", "Admin");
        } 
    }
}
