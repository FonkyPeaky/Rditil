using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Rditil.Models;
using Rditil.Services;
using Rditil.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using Timer = System.Timers.Timer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Rditil.ViewModels
{
    public partial class ExamViewModel : ObservableObject
    {
        private readonly EmailService _emailService;
        private readonly DbQuestionService _questionService;
        private readonly string _userEmail;

        private int _index = 0;
        private int _score = 0;
        private readonly Timer _timer;
        private TimeSpan _tempsRestant = TimeSpan.FromMinutes(60);

        public ObservableCollection<Question> QuestionsTirees { get; set; }

        [ObservableProperty] private Question questionEnCours;
        [ObservableProperty] private ObservableCollection<ReponseChoix> reponsesEnCours;
        [ObservableProperty] private string tempsRestant;

        public IRelayCommand<object?> SuivantCommand { get; }

        public ExamViewModel(EmailService emailService, string userEmail)
        {
            _emailService = emailService;
            _userEmail = userEmail;
            _questionService = new DbQuestionService(App.AppHost.Services.GetRequiredService<Rditil.Data.AppDbContext>());
            _timer = new Timer(1000);
            SuivantCommand = new RelayCommand<object?>(PasserQuestionSuivante);

            // Chargement des questions depuis PostgreSQL
            var questions = _questionService.GetAllQuestions();

            // Mélanger et prendre les 40 premières (si assez)
            var questionsTirees = questions.OrderBy(q => Guid.NewGuid()).Take(40).ToList();
            QuestionsTirees = new ObservableCollection<Question>(questionsTirees);

            // Lancer l'examen
            ChargerQuestion(0);
            TempsRestant = _tempsRestant.ToString(@"mm\:ss");
            DémarrerChrono();
        }

        public void DémarrerChrono()
        {
            _timer.Elapsed += (s, e) =>
            {
                // Mettre à jour le temps restant
                _tempsRestant = _tempsRestant.Subtract(TimeSpan.FromSeconds(1));
                TempsRestant = _tempsRestant.ToString(@"mm\:ss");

                if (_tempsRestant.TotalSeconds <= 0)
                {
                    _timer.Stop();
                    Application.Current.Dispatcher.Invoke(async () => await FinirExamenAsync());
                }
            };
            _timer.Start();
        }

        public void ChargerQuestion(int index)
        {
            if (index < 0 || index >= QuestionsTirees.Count) return;

            QuestionEnCours = QuestionsTirees[index];
            ReponsesEnCours = new ObservableCollection<ReponseChoix>(
                QuestionEnCours.Reponses.Select(r => new ReponseChoix
                {
                    TextReponse = r.TextReponse,
                    EstCorrect = r.EstCorrect,
                    IsChoisie = false
                }));
        }

        private async void PasserQuestionSuivante(object? obj)
        {
            var reponseChoisie = ReponsesEnCours.FirstOrDefault(r => r.IsChoisie);
            if (reponseChoisie != null && reponseChoisie.EstCorrect)
            {
                _score++;
            }

            _index++;
            if (_index < QuestionsTirees.Count)
                ChargerQuestion(_index);
            else
                await FinirExamenAsync();
        }

        private async Task FinirExamenAsync()
        {
            _timer.Stop();

            try
            {
                await _emailService.SendExamResultAsync(
                    _userEmail,
                    _userEmail,
                    _score,
                    QuestionsTirees.Count
                );
            }
            catch
            {
                MessageBox.Show(
                    "Erreur lors de l'envoi de l'email.",
                    "Erreur",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }

            var mainWindow = Application.Current.Windows
                .OfType<Window>()
                .FirstOrDefault(w => w.Title == "MainWindow");

            if (mainWindow != null)
            {
                var frame = (System.Windows.Controls.Frame)mainWindow.FindName("MainFrame");
                frame?.Navigate(new EndPage(
                    new ResultViewModel(
                        _emailService,
                        _userEmail,
                        _score,
                        QuestionsTirees.Count
                    )
                ));
            }
        }
    }
}
