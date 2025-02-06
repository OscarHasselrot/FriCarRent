using FriCarRent.Models;
using Microsoft.EntityFrameworkCore;

namespace FriCarRent.Data
{
    public class ApplicationDbContext: DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Admin> Admins { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base (options)
        {
            
        }
    }
}
