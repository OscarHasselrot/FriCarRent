using FriCarRent.Data;
using FriCarRent.Models;
using FriCarRent.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;



namespace FriCarRent.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomer customerRepository;


        public CustomerController(ICustomer customerRepository)
        {
            this.customerRepository = customerRepository;

        }
        // GET: CustomerController
        public ActionResult Index()
        {
            return View();
        }

        // GET: CustomerController/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                id = HttpContext.Session.GetInt32("UserId");
            }
            if (HttpContext.Session.GetString("Role") == "Customer" && HttpContext.Session.GetInt32("UserId") == id)
            {
                return View(customerRepository.GetById(id));
            }
            else if (HttpContext.Session.GetString("Role") == "Admin")
            {
                return View(customerRepository.GetById(id));
            }
            else
            {
                ViewBag.Message = "Du har inte rätt behörighet";
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult AllCustomers()
        {
            if (HttpContext.Session.GetString("Role") == "Admin")
            {
                return View(customerRepository.GetAll());
            }
            else
            {
                ViewBag.Message = "Du har inte rätt behörighet";
                return RedirectToAction("Index", "Home");
            }
        }
        public IActionResult LoginRegister(string? returnUrl)
        {
            var model = new LoginRegisterViewModel();
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }
        
        [HttpPost]
        public ActionResult Login(LoginViewModel loginVM)
        {

            if (!ModelState.IsValid)
            {
                return View("LoginRegister", new LoginRegisterViewModel { LoginVM = loginVM });
            }

            var user = customerRepository.GetByEmail(loginVM.Email);

            if (user == null || user.Password != loginVM.Password)
            {
                if (loginVM.ReturnUrl != null)
                {
                    ViewBag.ReturnUrl = loginVM.ReturnUrl;
                }
                ModelState.AddModelError("", "Ogiltig epost eller lösenord");
                return View("LoginRegister", new LoginRegisterViewModel { LoginVM = loginVM});

            }
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserName", user.FirstName + " " + user.LastName);
            HttpContext.Session.SetString("Role", "Customer");

            if (loginVM.ReturnUrl != null)
            {
                return Redirect(loginVM.ReturnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Register([Bind(Prefix = "CreateUserVM")] CreateUserViewModel createUserVM)
        {
            Customer customer = new Customer();
            customer.FirstName = createUserVM.FirstName;
            customer.LastName = createUserVM.LastName;
            customer.Email = createUserVM.Email;
            customer.Password = createUserVM.Password;

            if (!ModelState.IsValid)
            {
                return View("LoginRegister", new LoginRegisterViewModel { CreateUserVM = createUserVM });
            }
            customerRepository.Add(customer);

            HttpContext.Session.SetInt32("UserId", customer.Id);
            HttpContext.Session.SetString("UserName", customer.FirstName + " " + customer.LastName);
            HttpContext.Session.SetString("Role", "Customer");

            if (createUserVM.ReturnUrl != null)
            {
                return Redirect(createUserVM.ReturnUrl);
            }
            return RedirectToAction("Index", "Home");

        }

        // GET: CustomerController/Create
        public ActionResult Create()
        {
            return View();
        }


        // POST: CustomerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    customerRepository.Add(customer);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View();
                }
            }
            return View();
        }

        // GET: CustomerController/Edit/5
        public ActionResult Edit(int id)
        {
            if (HttpContext.Session.GetString("Role") == "Customer" && HttpContext.Session.GetInt32("UserId") == id)
            {
                return View(customerRepository.GetById(id));

            }
            else if (HttpContext.Session.GetString("Role") == "Admin")
            {
                return View(customerRepository.GetById(id));
            }
            else
            {
                ViewBag.Message = "Du har inte rätt behörighet.";
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: CustomerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Customer customer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    customerRepository.Update(customer);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View();
                }
            }
            return View();
        }

        // GET: CustomerController/Delete/5
        public ActionResult Delete(int id)
        {
            if (HttpContext.Session.GetString("Role") == "Admin")
            {
                return View(customerRepository.GetById(id));
            }
            else
            {
                ViewBag.Message = "Du har inte rätt behörighet";
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: CustomerController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Customer customer)
        {
            try
            {
                customerRepository.Delete(customer);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
