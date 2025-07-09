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

            var excelService = new ExcelService("chemin/vers/excel.xlsx");
            var smtpSettings = new SmtpSettings
            {
                Server = "smtp.office365.com",
                Port = 587,
                Username = "Rditil@outlook.com",
                Password = "LE_MDP",
                FromEmail = "Rditil@outlook.com",
                EnableSsl = true
            };
            var emailService = new EmailService(smtpSettings);

            DataContext = new ExamenViewModel(excelService, emailService, userEmail);

            Application.Current.Dispatcher.Invoke(() =>
            {
                var endPage = new EndPage(score);
                endPage.Show();
                Application.Current.Windows
                    .OfType<ExamenView>()
                    .FirstOrDefault()?.Close();
            });
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