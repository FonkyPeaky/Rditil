using Rditil.Models;
using Rditil.Services;
using Rditil.ViewModels;
using System.Windows.Controls;

namespace Rditil.Views
{
    public partial class QuestionPage : Page
    {
        public QuestionPage(string userEmail)
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
    }
}
