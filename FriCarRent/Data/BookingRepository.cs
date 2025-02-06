using FriCarRent.Models;
using Microsoft.EntityFrameworkCore;

namespace FriCarRent.Data
{
    public class BookingRepository : IBooking
    {
        private readonly ApplicationDbContext applicationDbContext;

        public BookingRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }
        public void Add(Booking booking)
        {
           applicationDbContext.Bookings.Add(booking);
            applicationDbContext.SaveChanges();
        }

        public void Delete(Booking booking)
        {
            applicationDbContext.Bookings.Remove(booking);
            applicationDbContext.SaveChanges();
        }

        public IEnumerable<Booking> GetAll()
        {
            return applicationDbContext.Bookings.Include(b => b.Customer).Include(b => b.Car).ToList();
        }

        public Booking GetById(int id)
        {
            return applicationDbContext.Bookings.Include(b => b.Customer).Include(b => b.Car).FirstOrDefault(b => b.Id == id);
        }

        public void Update(Booking booking)
        {
            applicationDbContext.Bookings.Update(booking);
            applicationDbContext.SaveChanges();
        }
    }
}
