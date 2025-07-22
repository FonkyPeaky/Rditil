using Rditil.Models;
using Rditil.Services;
using Rditil.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace Rditil.Views
{
    public partial class ExamenView : Window
    {
        public ExamenView(string userEmail)
        {
            InitializeComponent();

            

            var settings = new SmtpSettings
            {
                Server = "smtp.example.com",
                Port = 587,
                Username = "your_username",
                Password = "your_password",
                FromEmail = "noreply@example.com",
                EnableSsl = true
            };

            var emailService = new EmailService(settings);

            
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
