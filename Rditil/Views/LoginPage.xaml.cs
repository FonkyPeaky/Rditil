using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using Rditil.ViewModels;

namespace Rditil.Views
{
    public partial class LoginPage : Page
    {
        public LoginPage(LoginViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (Resources["EnterStoryboard"] is Storyboard sb)
                sb.Begin();
        }

        private void OnAnyFieldKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                OnLoginClick(this, new RoutedEventArgs());
            }
        }

        private async void OnLoginClick(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel vm)
            {
                vm.Password = PasswordBox.Password; // PasswordBox non-bindable → on copie
                await vm.LoginCommand.ExecuteAsync(null); // ⬅️ exécute la AsyncRelayCommand
                // vm.LoginCommand.Execute(null);
            }
        }
    }
}
