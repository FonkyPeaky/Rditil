using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Rditil.Models;
using Rditil.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rditil.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly IUserService _userService;
        private readonly INavigationService _navigationService;

        [ObservableProperty] private string email = "";
        [ObservableProperty] private string password = "";
        [ObservableProperty] private string errorMessage = "";

        public LoginViewModel(
            IUserService userService,
            INavigationService navigationService)
        {
            _userService = userService;
            _navigationService = navigationService;
        }

        [RelayCommand]
        private async Task LoginAsync()
        {
            ErrorMessage = "";

            var user = await _userService.GetByEmailAsync(Email);

            if (user != null && BCrypt.Net.BCrypt.Verify(Password, user.PasswordHash))
            {
                _navigationService.NavigateTo<WelcomeViewModel>(
                    new Dictionary<string, object?>
                    {
                        ["CurrentUser"] = user,
                        ["ManagerEmail"] = "manager@entreprise.com"
                    });
            }
            else
            {
                ErrorMessage = "Identifiants invalides";
            }
        }
    }
}
