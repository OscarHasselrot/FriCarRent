using FriCarRent.Models;

namespace FriCarRent.Data
{
    public class CustomerRepository : ICustomer
    {
        private readonly ApplicationDbContext applicationDbContext;

        public CustomerRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public void Add(Customer customer)
        {
            applicationDbContext.Customers.Add(customer);
            applicationDbContext.SaveChanges();
        }

        public void Delete(Customer customer)
        {
            applicationDbContext.Customers.Remove(customer);
            applicationDbContext.SaveChanges();
        }

        public IEnumerable<Customer> GetAll()
        {
            return applicationDbContext.Customers.OrderBy(c => c.LastName);
        }
        public Customer GetByEmail(string email)
        {
            return applicationDbContext.Customers.FirstOrDefault(c => c.Email == email);
        }

        public Customer GetById(int? id)
        {
            return applicationDbContext.Customers.FirstOrDefault(c => c.Id == id);
        }

        public void Update(Customer customer)
        {
            applicationDbContext.Customers.Update(customer);
            applicationDbContext.SaveChanges();
        }
    }
}
