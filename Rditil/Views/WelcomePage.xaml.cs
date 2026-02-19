using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Rditil.Data;
using Rditil.Services;
using Rditil.ViewModels;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Rditil.Views
{
    public partial class WelcomePage : Page
    {
        public WelcomePage()
        {
            InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Lancer toutes les animations
            if (Resources["EnterStoryboard"] is Storyboard enterSb)
                enterSb.Begin();

            if (Resources["LeftPanelSlideIn"] is Storyboard leftSb)
                leftSb.Begin();

            if (Resources["RightPanelSlideIn"] is Storyboard rightSb)
                rightSb.Begin();

            if (Resources["ContentFadeIn"] is Storyboard contentSb)
                contentSb.Begin();

            // Animations flottantes des orbes
            if (Resources["OrbFloat1"] is Storyboard orb1)
                orb1.Begin();

            if (Resources["OrbFloat2"] is Storyboard orb2)
                orb2.Begin();

            if (Resources["OrbFloat3"] is Storyboard orb3)
                orb3.Begin();

            // Animation pulse du score
            if (Resources["ScorePulse"] is Storyboard scorePulse)
                scorePulse.Begin();

            // Charger les stats de l'utilisateur
            await LoadUserStatsAsync();
        }

        private async System.Threading.Tasks.Task LoadUserStatsAsync()
        {
            try
            {
                var appState = App.AppHost.Services.GetService<IAppState>();
                var dbFactory = App.AppHost.Services.GetService<IDbContextFactory<AppDbContext>>();

                if (appState?.CurrentUser == null || dbFactory == null)
                    return;

                var userId = appState.CurrentUser.Id_Utilisateur;

                using var db = await dbFactory.CreateDbContextAsync();

                // Récupérer les examens de l'utilisateur
                var examens = await db.Examens
                    .Where(ex => ex.Id_Utilisateur == userId)
                    .OrderByDescending(ex => ex.DateExamen)
                    .ToListAsync();

                if (examens.Any())
                {
                    // Meilleur score
                    var bestExam = examens.OrderByDescending(ex => ex.Score).First();
                    ScoreText.Text = bestExam.Score.ToString();
                    ScoreDateText.Text = bestExam.DateExamen.ToString("dd/MM/yyyy");

                    // Temps moyen (si DureeExamen est renseigné)
                    var avgDuration = examens
                        .Where(ex => ex.DureeExamen.TotalMinutes > 0)
                        .Select(ex => ex.DureeExamen)
                        .ToList();

                    if (avgDuration.Any())
                    {
                        var avgTicks = (long)avgDuration.Average(d => d.Ticks);
                        var avgTime = TimeSpan.FromTicks(avgTicks);
                        TempsText.Text = $"{(int)avgTime.TotalMinutes:00}:{avgTime.Seconds:00}";
                    }
                    else
                    {
                        TempsText.Text = "--:--";
                    }

                    // Vérifier si examen fait aujourd'hui pour l'objectif
                    var today = DateTime.Today;
                    var examToday = examens.Any(ex => ex.DateExamen.Date == today);
                    if (examToday)
                    {
                        ObjectifProgress.Width = 200; // Full width
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur chargement stats: {ex.Message}");
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigation via le service custom
var nav = App.AppHost.Services.GetRequiredService<INavigationService>();
nav.NavigateTo<ExamViewModel>();
}

        private void ProgressButton_Click(object sender, RoutedEventArgs e)
        {
            var progressPage = App.AppHost.Services.GetService(typeof(ProgressPage)) as ProgressPage;
            var progressVm = App.AppHost.Services.GetService(typeof(ProgressViewModel)) as ProgressViewModel;

            if (progressPage != null && progressVm != null)
            {
                progressPage.DataContext = progressVm;

                // ✅ déclenche le load
                progressVm.OnNavigatedTo(null);

                NavigationService?.Navigate(progressPage);
            }
        }

    }
}
