<<<<<<< HEAD
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Rditil.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
=======
﻿using CommunityToolkit.Mvvm.Input;
using Rditil.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
>>>>>>> 1908e362463a42654d4b699764460fd7abed45f8

namespace Rditil.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly IUserService _userService;
        private readonly IAppState _appState;

        public event Action? LoginSucceeded;

        // UI fields
        [ObservableProperty] private string email = "";      
        [ObservableProperty] private string password = "";
        [ObservableProperty] private string errorMessage = "";
        [ObservableProperty] private string statusMessage = "";
        [ObservableProperty] private bool isBusy;

        private const string EmailDomain = "@randstaddigital.lu";

        public LoginViewModel(IUserService userService, IAppState appState)
        {
            _userService = userService;
<<<<<<< HEAD
            _appState = appState;
=======
            _navigationService = navigationService;

            // ✅ force l’AsyncRelayCommand du Toolkit, overload sans paramètre
            LoginCommand = new CommunityToolkit.Mvvm.Input.AsyncRelayCommand(LoginAsync);

            // ✅ commande sync
            CreateAccountCommand = new CommunityToolkit.Mvvm.Input.RelayCommand(NavigateToAdmin);
>>>>>>> 1908e362463a42654d4b699764460fd7abed45f8
        }

        [RelayCommand]
        private async Task LoginAsync()
        {
            if (IsBusy) return;

            ErrorMessage = "";
            StatusMessage = "Connexion en cours...";
            IsBusy = true;

            try
            {
                var normalizedEmail = NormalizeLogin(Email);
                var user = await _userService.GetByEmailAsync(normalizedEmail);

                if (user != null && BCrypt.Net.BCrypt.Verify(Password, user.PasswordHash))
                {
                    _appState.CurrentUser = user;
                    _appState.ManagerEmail = user.EmailNPlus1;

                    LoginSucceeded?.Invoke();
                    return;
                }

                ErrorMessage = "Identifiants invalides";
            }
            catch
            {
                ErrorMessage = "Erreur lors de la connexion";
            }
            finally
            {
                IsBusy = false;
                StatusMessage = "";
            }
        }

        private static string NormalizeLogin(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return "";

            var s = input.Trim().ToLowerInvariant();

            if (!s.Contains("@") && s.Contains(' ') && !s.Contains('.'))
                s = s.Replace(" ", ".");

            s = s.Replace(" ", "");

            if (!s.Contains("@"))
                s += EmailDomain;

            return s;
        }
    }
}
