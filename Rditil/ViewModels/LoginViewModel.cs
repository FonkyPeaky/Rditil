using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;                 // <- Toolkit
using Rditil.Services;

namespace Rditil.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly IUserService _userService;
        private readonly INavigationService _navigationService;

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

        // Commands (types Toolkit)
        public IAsyncRelayCommand LoginCommand { get; }
        public IRelayCommand CreateAccountCommand { get; }

        public LoginViewModel(IUserService userService, INavigationService navigationService)
        {
            _userService = userService;
            _navigationService = navigationService;

            // ✅ force l’AsyncRelayCommand du Toolkit, overload sans paramètre
            LoginCommand = new CommunityToolkit.Mvvm.Input.AsyncRelayCommand(LoginAsync);

            // ✅ commande sync
            CreateAccountCommand = new CommunityToolkit.Mvvm.Input.RelayCommand(NavigateToAdmin);
        }

        private async Task LoginAsync()
        {
            ErrorMessage = string.Empty;

            var user = await _userService.GetByEmailAsync(Email);
            if (user != null && BCrypt.Net.BCrypt.Verify(Password, user.PasswordHash))
            {
                App.CurrentUser = user;
                _navigationService.NavigateTo<WelcomeViewModel>();
            }
            else
            {
                ErrorMessage = "Email ou mot de passe incorrect.";
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
