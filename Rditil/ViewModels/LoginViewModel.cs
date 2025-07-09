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
        private readonly IExcelService _excelService;

        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private string errorMessage;

        public ICommand LoginCommand { get; }

        public LoginViewModel(IExcelService excelService)
        {
            _excelService = excelService;
            LoginCommand = new AsyncRelayCommand(LoginAsync);
        }

        private async Task LoginAsync()
        {
            // Simuler async même si Excel est sync
            await Task.Delay(100);

            var user = _excelService.GetUserByEmailAndPassword(Email, Password);
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
