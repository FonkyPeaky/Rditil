using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Rditil.Data;
using Rditil.Models;
using System.Collections.ObjectModel;
using System.Timers;
using System.Windows;
using System.Windows.Threading;
using Timer = System.Timers.Timer;

namespace Rditil.ViewModels
{
    public class QuestionViewModel : ObservableObject
    {
        private Timer _timer;
        private TimeSpan _tempsRestant;
        private int _index;

        public ObservableCollection<Question> QuestionsTirees { get; set; }
        public Question QuestionEnCours { get; set; }
        public ObservableCollection<ReponseChoix> ReponsesEnCours { get; set; }

        private string _tempsRestantAffiche;
        public string TempsRestantAffiche
        {
            get => _tempsRestantAffiche;
            set => SetProperty(ref _tempsRestantAffiche, value);
        }

        public QuestionViewModel()
        {
            QuestionsTirees = new ObservableCollection<Question>();
            ReponsesEnCours = new ObservableCollection<ReponseChoix>();

            _tempsRestant = TimeSpan.FromMinutes(60);
            TempsRestantAffiche = _tempsRestant.ToString(@"mm\:ss");

            _timer = new Timer(1000);
            _timer.Elapsed += (sender, e) =>
            {
                _tempsRestant = _tempsRestant.Subtract(TimeSpan.FromSeconds(1));

                TempsRestantAffiche = _tempsRestant.ToString(@"mm\:ss");
                OnPropertyChanged(nameof(TempsRestantAffiche));

                if (_tempsRestant <= TimeSpan.Zero)
                {
                    _timer.Stop();
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show("Temps écoulé !", "Fin de l'examen", MessageBoxButton.OK, MessageBoxImage.Information);
                        // Logique pour terminer l'examen
                    });
                }
            };
            _timer.Start();
        }

        private void ChargerQuestion(int index)
        {
            if (index < 0 || index >= QuestionsTirees.Count) return;
            QuestionEnCours = QuestionsTirees[index];
            ReponsesEnCours.Clear();
            foreach (var reponse in QuestionEnCours.Reponses)
            {
                ReponsesEnCours.Add(new ReponseChoix { Id = reponse.Id, TextReponse = reponse.TextReponse });
            }
        }

        private void PasserQuestionSuivante()
        {
            if (_index < QuestionsTirees.Count - 1)
            {
                _index++;
                ChargerQuestion(_index);
            }
            else
            {
                _timer.Stop();
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show("Examen terminé !", "Fin de l'examen", MessageBoxButton.OK, MessageBoxImage.Information);
                    // Logique pour terminer l'examen
                });
            }
        }
    }
}
