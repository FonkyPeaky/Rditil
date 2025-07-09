using System.Windows;
using System.Windows.Input;

namespace Rditil.Views
{
    public partial class AdminPanel : Window
    {
        public AdminPanel()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                this.DragMove();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            // Si plus tard tu recrées AdminViewModel, tu pourras réactiver ça
            // if (DataContext is AdminViewModel vm)
            //     vm.NouvelUtilisateur.Password = PasswordBox.Password;
        }
    }
}
