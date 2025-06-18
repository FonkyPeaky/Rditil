using Rditil.Data;
using Rditil.Services;
using Rditil.ViewModels;
using System;
using System.Linq;
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

            var context = new AppDbContextFactory().CreateDbContext(Array.Empty<string>());
            var authService = new AuthService(context);
             
            _viewModel = new LoginViewModel(authService);
            this.DataContext = _viewModel;
        }

        private void OnLoginClick(object sender, RoutedEventArgs e)
        {
            _viewModel.Password = PasswordBox.Password;
            _viewModel.LoginCommand.Execute(null);
        }
    }
}
