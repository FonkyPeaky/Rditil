using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Rditil.Data;
using Rditil.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timer = System.Timers.Timer;
using System.Timers;
using Microsoft.EntityFrameworkCore;
using System.Windows;

namespace Rditil.ViewModels
{
    public partial class ExamViewModel : ObservableObject
    {
        private readonly AppDbContext _context;
        private int _index = 0;
        private readonly Timer _timer;
        private TimeSpan _tempsRestant = TimeSpan.FromMinutes(60);
        public Models.ReponseChoix rch = new();

        public ObservableCollection<Question> QuestionsTirees { get; set; }
        [ObservableProperty] private Question questionEnCours;
        [ObservableProperty] private ObservableCollection<ReponseChoix> reponsesEnCours;
        [ObservableProperty] private string tempsRestant;

        public IRelayCommand SuivantCommand { get; }

        public ExamViewModel(AppDbContext context)
        {
            _context = context;

            // Initialize non-nullable fields
            _timer = new Timer(1000);
            questionEnCours = new Question();
            reponsesEnCours = new ObservableCollection<ReponseChoix>();
            tempsRestant = string.Empty;

            // Tirage aléatoire de 40 questions
            QuestionsTirees = new ObservableCollection<Question>(
                _context.Questions
                        .Include(q => q.Reponses)
                        .OrderBy(q => Guid.NewGuid())
                        .Take(40)
                        .ToList());

            SuivantCommand = new RelayCommand(PasserQuestionSuivante);

            DémarrerChrono();
            ChargerQuestion(0);
        }

        public void DémarrerChrono()
        {
            _timer.Elapsed += (s, e) =>
            {
                _tempsRestant = _tempsRestant.Subtract(TimeSpan.FromSeconds(1));
                TempsRestant = _tempsRestant.ToString(@"mm\:ss");

                if (_tempsRestant.TotalSeconds <= 0)
                {
                    _timer.Stop();
                    FinirExamen();
                }
            };
            _timer.Start();
        }

        public void ChargerQuestion(int index)
        {
            QuestionEnCours = QuestionsTirees[index];
            ReponsesEnCours = new ObservableCollection<ReponseChoix>(
                QuestionEnCours.Reponses.Select(r => new ReponseChoix
                {
                    Id = r.Id,
                    TextReponse = r.TextReponse,
                    EstCorrect = r.EstCorrect,
                    IsChoisie = false
                }));
        }

        private void PasserQuestionSuivante()
        {
            // Enregistrer la réponse sélectionnée
            var reponseChoisie = ReponsesEnCours.FirstOrDefault(r => r.IsChoisie);
            // (optionnel : stocker la réponse dans un dictionnaire pour le score)

            _index++;
            if (_index < QuestionsTirees.Count)
                ChargerQuestion(_index);
            else
                FinirExamen();
        }

        private void FinirExamen()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                _timer.Stop();
                MessageBox.Show("Examen terminé !");
                // TODO: Calcul du score, redirection, sauvegarde
            });
        }
    }
}
