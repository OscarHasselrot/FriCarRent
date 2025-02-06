using FriCarRent.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FriCarRent.ViewModels
{
    public class LoginViewModel
    {
        [EmailAddress]
        public string Email { get; set; }
        [PasswordPropertyText]
        public string Password { get; set; }
        public string? ReturnUrl {  get; set; }
        
    }
}
