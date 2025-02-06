using FriCarRent.Models;

namespace FriCarRent.Data
{
    public interface IAdmin
    {
        Admin GetByEmail(string email);
        Admin GetById(int id);  
    }
}
