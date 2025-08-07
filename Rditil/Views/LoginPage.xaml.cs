using Microsoft.EntityFrameworkCore;
using Rditil.Data;
using Rditil.Services; // ← celui qu’on veut pour NavigationService
using Rditil.ViewModels;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Rditil.Views
{
    public partial class LoginPage : Page
    {
        private LoginViewModel _viewModel;

        public LoginPage(LoginViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void OnLoginClick(object sender, RoutedEventArgs e)
        {



            if (DataContext is LoginViewModel viewModel)
            {
                viewModel.Password = PasswordBox.Password; // obligatoire, non lié par Binding
                viewModel.Login();
            }
        }


    }
}
