using CommunityToolkit.Mvvm.Input;
using Rditil.Models;
using Rditil.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Rditil.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ErrorMessage { get; set; }

        public RelayCommand LoginCommand { get; set; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(Login);
        }

        private void Login()
        {
            var userService = new DbUserService(App.DbContext);

            var user = userService.GetUserByEmailAndPassword(Email, Password);
            if (user != null)
            {
                App.CurrentUser = user;

                var examWindow = new Views.ExamPage();
                examWindow.Show();

                Application.Current.Windows[0]?.Close(); // Ferme la fenêtre Login
            }
            else
            {
                ErrorMessage = "Email ou mot de passe incorrect.";
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
