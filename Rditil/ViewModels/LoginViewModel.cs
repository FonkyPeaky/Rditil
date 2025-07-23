using CommunityToolkit.Mvvm.Input;
using Rditil.Data;
using Rditil.Models;
using Rditil.Services;
using Rditil.Views;
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
        public string Nom { get; set; }
        public string Prenom { get; set; }

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

                var homePage = new WelcomePage();
                NavigationService.NavigateTo(Application.Current.MainWindow, homePage);
            }
            else
            {
                ErrorMessage = "Email ou mot de passe incorrect.";
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public bool AuthenticateUser()
        {
            using (var context = new AppDbContextFactory().CreateDbContext(null))
            {
                return context.Utilisateurs.Any(u =>
                    u.Email == Email && u.Password == Password);
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
