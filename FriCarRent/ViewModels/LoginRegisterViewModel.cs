namespace FriCarRent.ViewModels
{
    public class LoginRegisterViewModel
    {
        public LoginViewModel LoginVM { get; set; } = new LoginViewModel();
        public CreateUserViewModel CreateUserVM { get; set; } = new CreateUserViewModel();
    }
}
