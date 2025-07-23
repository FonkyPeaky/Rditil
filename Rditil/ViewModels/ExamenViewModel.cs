using CommunityToolkit.Mvvm.Input;
using Rditil.Models;
using Rditil.Services;
using Rditil.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Rditil.ViewModels
{
    public class ExamenViewModel : ViewModelBase
    {        
        private readonly EmailService? _emailService;
        private readonly string? _userEmail;
        private readonly List<Question>? _questions;
        private int _currentQuestionIndex;
        private Question? _currentQuestion;
        private int _score;
        private List<Question> QuestionsTirees;


        public Question? QuestionActuelle
        {
            get => _currentQuestion;
            set
            {
                _currentQuestion = value;
                OnPropertyChanged(nameof(QuestionActuelle));
            }
        }

        public int NumeroQuestionActuelle => _currentQuestionIndex + 1;

        public ICommand? QuestionSuivanteCommand { get; }

        private void LoadCurrentQuestion()
        {
            if (_questions is not null)
            {
                QuestionActuelle = _questions[_currentQuestionIndex];
                OnPropertyChanged(nameof(NumeroQuestionActuelle));
            }
        }

        private bool CanExecuteQuestionSuivante(object? parameter)
        {
            return _currentQuestionIndex < _questions.Count;
        }

        private async Task ExecuteQuestionSuivante(object? parameter)
        {
            try
            {
                _currentQuestionIndex++;

                if (_currentQuestionIndex < _questions.Count)
                {
                    LoadCurrentQuestion();
                }
                else
                {
                    await FinirExamenAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la validation de la réponse : {ex.Message}");
            }
        }

        private async Task FinirExamenAsync()
        {

            var examResultService = new DbExamResultService(App.DbContext);
            examResultService.EnregistrerExamen(App.CurrentUser, _score, QuestionsTirees.ToList());

            // Envoyer email résultat
            try
            {
                await _emailService.SendExamResultAsync(
                      _userEmail,
                      _userEmail,
                      _score,
                      _questions.Count
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

            // Naviguer vers EndPage
            Application.Current.Dispatcher.Invoke(() =>
            {
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
                            _questions.Count
                        )
                    ));
                }
            });
        }
    }
}
