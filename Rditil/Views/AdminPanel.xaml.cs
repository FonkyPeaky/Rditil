using Microsoft.EntityFrameworkCore;
using Rditil.Models;
using Rditil.ViewModels;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Rditil.Views
{
    public partial class AdminPanel : Page // Ensure this matches the base class in all partial declarations
    {
        public AdminPanel()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(); // Fixed: Removed 'this.Close()' as 'Page' does not have a 'Close' method.
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Placeholder for button click logic.
        }

        private void AddUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is AdminPanelViewModel vm)
            {
                vm.NewUser.Password = PasswordBox.Password;
            }
        }
    }
}
