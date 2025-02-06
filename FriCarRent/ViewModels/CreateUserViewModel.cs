using System.ComponentModel.DataAnnotations;

namespace FriCarRent.ViewModels
{
    public class CreateUserViewModel
    {
        public string? ReturnUrl { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
