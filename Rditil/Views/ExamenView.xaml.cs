using System.Windows;
using System.Windows.Media.Animation;
using Microsoft.Extensions.DependencyInjection;
using Rditil.Data;
using Rditil.Services;
using Rditil.ViewModels;

namespace Rditil.Views
{
    public partial class ExamenView
    {
        public ExamenView(string userEmail, string managerEmail, string userFullName = null)
        {
            InitializeComponent();

            var sp = ((App)Application.Current).Services; // exposé par ton AppHost
            var ctx = sp.GetRequiredService<AppDbContext>();
            var mail = sp.GetRequiredService<IEmailService>();

            var vm = new ExamViewModel(ctx, mail, userEmail, managerEmail)
            {
                UserFullName = userFullName
            };

            // Navigation vers EndPage quand terminé
            vm.ExamFinished += (s, e) =>
            {
                NavigationService?.Navigate(new EndPage(new ResultViewModel(
                    e.Score, e.Total, e.TimeUsed, e.TimeExpired)));
            };

            DataContext = vm;

            // Démarrer tout de suite l’épreuve (1h + 40 QCM)
            vm.Demarrer();
        }
    }
}
