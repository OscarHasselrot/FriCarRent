using FriCarRent.Models;

namespace FriCarRent.Data
{
    public class AdminRepository : IAdmin
    {
        private readonly ApplicationDbContext applicationDbContext;

        public AdminRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }
        public Admin GetByEmail(string email)
        {
            
            return applicationDbContext.Admins.FirstOrDefault(a => a.Email == email);
        }

        public Admin GetById(int id)
        {
            return applicationDbContext.Admins.FirstOrDefault(a => a.Id == id);
        }
    }
}
