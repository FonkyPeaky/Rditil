using Rditil.ViewModels;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace Rditil.Views
{
    public partial class LoginPage : Page
    {
        // --- CONSTRUCTEUR RUNTIME (DI) ---
        public LoginPage(LoginViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        // --- CONSTRUCTEUR DESIGNER (sans paramètre) ---
        public LoginPage()
        {
            InitializeComponent();

            // ne jamais polluer le runtime avec ce VM
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                DataContext = new DesignLoginViewModel();
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (Resources["EnterStoryboard"] is Storyboard sb)
                sb.Begin();
        }

        private void OnAnyFieldKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                OnLoginClick(this, new RoutedEventArgs());
        }

        private async void OnLoginClick(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel vm)
            {
                vm.Password = PasswordBox.Password;   // PasswordBox non-bindable
                await vm.LoginCommand.ExecuteAsync(null);
            }
        }
    }

    // -------- ViewModel de DESIGN UNIQUEMENT --------
    internal sealed class DesignLoginViewModel
    {
        public string Email { get; set; } = "john.doe@randstad.com";
        public string Password { get; set; } = "";
        public ICommand LoginCommand { get; } = new DummyCommand();
    }

    internal sealed class DummyCommand : ICommand
    {
        public bool CanExecute(object parameter) => true;
        public void Execute(object parameter) { }
        public event EventHandler CanExecuteChanged { add { } remove { } }
    }
}
