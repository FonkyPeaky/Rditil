using Microsoft.EntityFrameworkCore;
using Rditil.Data;
using Rditil.Models;
using Rditil.Services;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Rditil.ViewModels;
using BCrypt.Net;


namespace Rditil.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly AppDbContext _dbContext;
        private readonly INavigationService _navigationService;
        public ICommand NavigateToAdminCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        private string _email;
        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(); }
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel(AppDbContext dbContext, INavigationService navigationService)
        {
            _dbContext = dbContext;
            _navigationService = navigationService;
            LoginCommand = new RelayCommand(Login);
            NavigateToAdminCommand = new RelayCommand(NavigateToAdmin);

        }

        public void Login()
        {
            ErrorMessage = string.Empty;

            try
            {
                string normalizedEmail = Email?.Trim();
                string inputPassword = Password?.Trim();

                Debug.WriteLine($"Tentative de login : {normalizedEmail}");

                var user = _dbContext.Utilisateurs
                    .AsNoTracking()
                    .FirstOrDefault(u => u.Email == normalizedEmail);

                if (user != null && BCrypt.Net.BCrypt.Verify(inputPassword, user.Password))
                {
                    App.CurrentUser = user;
                    _navigationService.NavigateTo<WelcomeViewModel>();
                    Debug.WriteLine($"✔ Connexion réussie : {user.Nom} {user.Prenom}");
                }
                else
                {
                    ErrorMessage = "Nom d'utilisateur ou mot de passe incorrect.";
                    Debug.WriteLine("❌ Authentification échouée.");
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Erreur de connexion.";
                Debug.WriteLine($"⚠ Erreur : {ex.Message}");
            }
        }

        private void NavigateToAdmin()
        {
            _navigationService.NavigateTo<AdminPanelViewModel>();
        }


        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
