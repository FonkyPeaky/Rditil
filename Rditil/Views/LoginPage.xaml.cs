using Rditil.Services;
using Rditil.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Rditil.Views
{
    public partial class LoginPage : Page
    {
        private readonly LoginViewModel _viewModel;

        public LoginPage()
        {
            InitializeComponent();
            this.DataContext = _viewModel;
        }

        private void OnLoginClick(object sender, RoutedEventArgs e)
        {
            _viewModel.Password = PasswordBox.Password;
            _viewModel.LoginCommand.Execute(null);
        }
    }
}
