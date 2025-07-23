using System.Windows;
using System.Windows.Controls;
using Rditil.Services;
using Rditil.ViewModels;

namespace Rditil.Views
{
    public partial class LoginPage : Window
    {
        private LoginViewModel viewModel;

        public LoginPage()
        {
            InitializeComponent();
            viewModel = new LoginViewModel();
            DataContext = viewModel;
        }

        private void OnLoginClick(object sender, RoutedEventArgs e)
        {
            if (viewModel.AuthenticateUser())
            {
                var welcomePage = new WelcomePage();
                NavigationService.NavigateTo(this, welcomePage);
            }
            else
            {
                MessageBox.Show("Nom d'utilisateur ou mot de passe incorrect.");
            }
        }
    }
}
