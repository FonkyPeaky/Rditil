using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using Rditil.ViewModels;

namespace Rditil.Views
{
    public partial class AdminPanel : Page
    {
        public AdminPanel()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (Resources["EnterStoryboard"] is Storyboard sb)
                sb.Begin();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is AdminPanelViewModel vm && sender is PasswordBox pb)
            {
                vm.MotDePasse = pb.Password;
            }
        }

        private void AddUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is AdminPanelViewModel vm && PasswordBox != null)
            {
                vm.MotDePasse = PasswordBox.Password;
            }
        }
    }
}
