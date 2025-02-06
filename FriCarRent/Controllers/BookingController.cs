using FriCarRent.Data;
using FriCarRent.Models;
using FriCarRent.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace FriCarRent.Controllers
{
    public class BookingController : Controller
    {
        private readonly IBooking bookingRepository;
        private readonly ICar carRepository;
        private readonly ICustomer customerRepository;

        public BookingController(IBooking bookingRepository, ICar carRepository, ICustomer customerRepository)
        {
            this.bookingRepository = bookingRepository;
            this.carRepository = carRepository;
            this.customerRepository = customerRepository;

        }
        // GET: BookingController
        public ActionResult Index()
        {
            if (HttpContext.Session.GetString("Role") == "Admin")
            {
                var bookings = bookingRepository
                    .GetAll()
                    .Select(b => new BookingsViewModel
                    {
                        Id = b.Id,
                        CarId = b.Car.Id,
                        CustomerEmail = b.Customer.Email,
                        CarName = b.Car.Model,
                        StartDate = b.StartDate,
                        EndDate = b.EndDate,
                        TotalPrice = b.TotalPrice,

                    })
                    .ToList();
                return View(bookings);
            }
            else if (HttpContext.Session.GetString("Role") == "Customer")
            {
                var user = customerRepository.GetById((int)HttpContext.Session.GetInt32("UserId"));
                var bookings = bookingRepository.GetAll().Where(u => u.Customer.Id == user.Id).Select(b => new BookingsViewModel
                {
                    Id = b.Id,
                    CarId = b.Car.Id,
                    CustomerEmail = b.Customer.Email,
                    CarName = b.Car.Model,
                    StartDate = b.StartDate,
                    EndDate = b.EndDate,
                    TotalPrice = b.TotalPrice,
                });
                return View(bookings);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: BookingController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: BookingController/Create
        public ActionResult Create(int carId)
        {

            var car = carRepository.GetById(carId);
            if (car == null)
            {
                return RedirectToAction("Index", "Car");

            }

            if (HttpContext.Session.GetString("Role") == "Admin")
            {
                var customers = customerRepository.GetAll().Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = $"{c.FirstName} {c.LastName}"

                });
                ViewBag.customers = customers;
            }
            else
            {
                var customer = customerRepository.GetById((int)HttpContext.Session.GetInt32("UserId"));

                if (customer == null)
                {

                    string? returnUrl = Url.Action("Create", "Booking", new { carId = car.Id });
                    return RedirectToAction("Login", "Customer", new { returnUrl });
                }
                ViewBag.customerId = customer.Id;
                ViewBag.customer = $"{customer.FirstName} {customer.LastName}";


            }
            ViewBag.carId = car.Id;
            ViewBag.car = $"{car.Brand}: {car.Model}. {car.Price}kr/dag";
            ViewBag.price = car.Price;
            return View();
        }

        // POST: BookingController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BookingViewModel bookingVM)
        {

            if (bookingVM.StartDate <= DateTime.Now)
            {
                ModelState.AddModelError("StartDate", "Startdatum har redan passerat");

                var car = carRepository.GetById(bookingVM.CarId);
                if (car == null)
                {
                    return RedirectToAction("Index", "Car");
                }

                if (HttpContext.Session.GetString("Role") == "Admin")
                {
                    var customers = customerRepository.GetAll().Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = $"{c.FirstName} {c.LastName}"
                    });
                    ViewBag.customers = customers;
                }
                else
                {
                    var customer = customerRepository.GetById((int)HttpContext.Session.GetInt32("UserId"));

                    if (customer == null)
                    {
                        string? returnUrl = Url.Action("Create", "Booking", new { carId = car.Id });
                        return RedirectToAction("Login", "Customer", new { returnUrl });
                    }
                    ViewBag.customerId = customer.Id;
                    ViewBag.customer = $"{customer.FirstName} {customer.LastName}";
                }

                ViewBag.carId = car.Id;
                ViewBag.car = $"{car.Brand}: {car.Model}. {car.Price}kr/dag";
                ViewBag.price = car.Price;

                return View(bookingVM);
            }

            var booking = new Booking
            {
                Car = carRepository.GetById(bookingVM.CarId),
                Customer = customerRepository.GetById(bookingVM.CustomerId),
                StartDate = bookingVM.StartDate,
                EndDate = bookingVM.EndDate
            };

            int totalDays = (booking.EndDate - booking.StartDate).Days + 1;
            booking.TotalPrice = bookingVM.TotalPrice * totalDays;

            try
            {
                if (ModelState.IsValid)
                {
                    bookingRepository.Add(booking);
                    return RedirectToAction("BookingConfirmation", new { id = booking.Id, carId = bookingVM.CarId });
                }
            }
            catch
            {
                ModelState.AddModelError("", "Ett fel inträffade vid bokningen.");
            }

            // Repopulate ViewBag before returning the view in case of failure
            var carRetry = carRepository.GetById(bookingVM.CarId);
            if (carRetry != null)
            {
                ViewBag.carId = carRetry.Id;
                ViewBag.car = $"{carRetry.Brand}: {carRetry.Model}. {carRetry.Price}kr/dag";
                ViewBag.price = carRetry.Price;
            }

            return View(bookingVM);
        }
        // GET: BookingController/BookingConfirmation/5
        public ActionResult BookingConfirmation(int id, int carId)
        {
            var booking = bookingRepository.GetById(id);
            var car = carRepository.GetById(carId);
            ViewBag.car = $"{car.Brand} {car.Model}";
            return View(bookingRepository.GetById(id));
        }

        // GET: BookingController/Edit/5
        public ActionResult Edit(int id)
        {
            if (HttpContext.Session.GetString("Role") == "Admin")
            {
                return View(bookingRepository.GetById(id));
            }
            else if (HttpContext.Session.GetString("Role") == "Customer")
            {
                var user = customerRepository.GetById((int)HttpContext.Session.GetInt32("UserId"));
                var booking = bookingRepository.GetById(id);

                if (user.Id == booking.Customer.Id)
                {

                    return View(booking);
                }
                else
                {
                    ViewBag.Message = "Uppdatering av bokning misslyckades";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                ViewBag.Message = "Updatering av bokning misslyckades";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: BookingController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Booking booking)
        {
            var existingBooking = bookingRepository.GetById(booking.Id);

            if (existingBooking == null)
            {
                ViewBag.Message = "Bokning hittades inte.";
                return RedirectToAction(nameof(Index));
            }
            booking.Car = carRepository.GetById(booking.Car.Id);
            booking.Customer = customerRepository.GetById(booking.Customer.Id);

            if (booking.StartDate <= DateTime.Now)
            {
                ModelState.AddModelError("", "Startdatum har redan passerat.");
                return View(booking);
            }

            int totalDays = (booking.EndDate - booking.StartDate).Days + 1;
            booking.TotalPrice = existingBooking.Car.Price * totalDays;

            try
            {
                if (ModelState.IsValid)
                {
                    existingBooking.StartDate = booking.StartDate;
                    existingBooking.EndDate = booking.EndDate;
                    existingBooking.TotalPrice = booking.TotalPrice;

                    bookingRepository.Update(existingBooking);
                    return RedirectToAction(nameof(Index));
                }

                return View(booking);
            }
            catch
            {
                ModelState.AddModelError("", "Ett fel inträffade vid uppdatering av bokningen.");
                return View(booking);
            }
        }

        // GET: BookingController/Delete/5
        public ActionResult Delete(int id)
        {
            var booking = bookingRepository.GetById(id);
            if (HttpContext.Session.GetString("Role") == "Customer")
            {
                var user = customerRepository.GetById((int)HttpContext.Session.GetInt32("UserId"));
                if (booking.Customer.Id == user.Id)
                {
                    return View(booking);
                }
                else
                {
                    ViewBag.Message = "Du har inte rätt behörighet.";
                    return RedirectToAction(nameof(Index));

                }
            }
            else if (HttpContext.Session.GetString("Role") == "Admin")
            {
                return View(booking);
            }
            else
            {
                ViewBag.Message = "Du har inte rätt behörighet.";
                return RedirectToAction(nameof(Index));
            }

        }

        // POST: BookingController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Booking booking)
        {
            try
            {
                ViewBag.MessageSuccess = "Bokning borttagen.";
                bookingRepository.Delete(booking);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ViewBag.Message = "Radering av bokning misslyckades";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
