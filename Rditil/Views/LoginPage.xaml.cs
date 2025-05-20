using Rditil.Data;
using Rditil.Services;
using Rditil.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Rditil.Views
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page // Ensure this matches the base class in all partial declarations
    {
        private readonly LoginViewModel _viewModel;

        public LoginPage()
        {
            InitializeComponent();

            var context = new AppDbContextFactory().CreateDbContext(Array.Empty<string>());
            _viewModel = new LoginViewModel(new AuthService(context));
            this.DataContext = _viewModel; // <-- ici, bien dans le constructeur
        }

        private void OnLoginClick(object sender, RoutedEventArgs e)
        {
            _viewModel.Password = PasswordBox.Password;
            _viewModel.LoginCommand.Execute(null);
        }
    }

}
