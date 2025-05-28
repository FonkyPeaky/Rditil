using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Rditil.Models;
using Rditil.Services;
using Rditil.Views;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Rditil.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly AuthService _authService;

        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private string errorMessage;


        public ICommand LoginCommand { get; }

        public LoginViewModel(AuthService authService)
        {
            _authService = authService;
            LoginCommand = new RelayCommand<object>(async _ => await LoginAsync());
        }

        private async Task LoginAsync()
        {
            var user = await _authService.LoginAsync(Email, Password);
            if (user != null)
            {
                App.CurrentUser = user;
                var window = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                if (user.IsAdmin)
                    window.MainFrame.Navigate(new AdminPanel());
                else
                    window.MainFrame.Navigate(new WelcomePage());
            }
            else
            {
                ErrorMessage = "Identifiants invalides.";
            }
        }
    }
}
