using Microsoft.EntityFrameworkCore;
using Rditil.Data;
using Rditil.Models;
using Rditil.Services;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using Rditil.Views;
using Rditil.ViewModels;
using Rditil.Services;
using Rditil;


namespace Rditil.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly AppDbContext _dbContext;
        private readonly INavigationService _navigationService;
        

        public event PropertyChangedEventHandler PropertyChanged;

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel(AppDbContext dbContext, INavigationService navigationService)
        {
            _dbContext = dbContext;
            _navigationService = navigationService;
            LoginCommand = new RelayCommand(Login);
        }

        public void Login()
        {
            ErrorMessage = string.Empty;

            try
            {
                string? normalizedEmail = Email?.Trim();
                string? normalizedPassword = Password?.Trim();

                Debug.WriteLine($"Tentative de login avec : Email = '{normalizedEmail}', Password = '{normalizedPassword}'");

                var user = _dbContext.Utilisateurs
                    .AsNoTracking()
                    .FirstOrDefault(u => u.Email == normalizedEmail && u.Password == normalizedPassword);

                if (user != null)
                {

                    App.CurrentUser = user;
                    Debug.WriteLine($"✔ Connexion réussie : {user.Nom} {user.Prenom}");
                    _navigationService.NavigateTo<WelcomeViewModel>();
                }
                else
                {
                    Debug.WriteLine("❌ Utilisateur non trouvé ou mot de passe incorrect.");
                    ErrorMessage = "Nom d'utilisateur ou mot de passe incorrect.";
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"⚠ Erreur lors du login : {ex.Message}");
                ErrorMessage = "Erreur de connexion.";
            }
        }











        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
