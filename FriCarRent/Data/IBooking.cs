using FriCarRent.Models;

namespace FriCarRent.Data
{
    public interface IBooking
    {
        IEnumerable<Booking> GetAll();
        Booking GetById(int id);
        void Add(Booking booking);
        void Update(Booking booking);
        void Delete(Booking booking);

    }
}
