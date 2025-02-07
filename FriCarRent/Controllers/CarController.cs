using FriCarRent.Data;
using FriCarRent.Models;
using FriCarRent.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FriCarRent.Controllers
{
    public class CarController : Controller
    {
        private readonly ICar carRepository;


        public CarController(ICar carRepository)
        {
            this.carRepository = carRepository;

        }
        // GET: CarController
        public ActionResult Index()
        {
            return View(carRepository.GetAll());
        }

        

        public ActionResult Book(Car car)
        {

            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                string returnUrl = Url.Action("Book", "Car");
                return RedirectToAction("LoginRegister", "Customer", new { returnUrl });
            }
            return RedirectToAction("Create", "Booking", new { carId = car.Id });
        }
        // GET: CarController/Create
        public ActionResult Create()
        {
            return View();
        }


        // POST: CarController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Car car)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    carRepository.Create(car);

                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View();
                }
            }
            return View();
        }

        // GET: CarController/Edit/5
        public ActionResult Edit(int id)
        {
            return View(carRepository.GetById(id));
        }

        // POST: CarController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Car car)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    carRepository.Update(car);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View();
                }
            }
            return View();
        }

        // GET: CarController/Delete/5
        public ActionResult Delete(int id)
        {
            return View(carRepository.GetById(id));
        }

        // POST: CarController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Car car)
        {

            try
            {
                ViewBag.Message = "Bil borttagen";
                carRepository.Delete(car);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
