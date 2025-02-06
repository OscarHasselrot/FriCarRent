using FriCarRent.Models;

namespace FriCarRent.Data
{
    public interface ICustomer
    {
        IEnumerable<Customer> GetAll();
        Customer GetById(int? id);
        Customer GetByEmail(string email);
        void Add(Customer customer);
        void Update(Customer customer);
        void Delete(Customer customer);






    }
}
