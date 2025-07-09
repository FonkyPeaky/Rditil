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
        private readonly IExcelService _excelService;
        private readonly EmailService _emailService;
        private readonly string _userEmail;
        private readonly List<Question> _questions;
        private int _currentQuestionIndex;
        private Question _currentQuestion;
        private int _score;

        public Question QuestionActuelle
        {
            get => _currentQuestion;
            set
            {
                _currentQuestion = value;
                OnPropertyChanged(nameof(QuestionActuelle));
            }
        }

        public int NumeroQuestionActuelle => _currentQuestionIndex + 1;

        public ICommand QuestionSuivanteCommand { get; }

        public ExamenViewModel(IExcelService excelService, EmailService emailService, string userEmail)
        {
            _excelService = excelService;
            _emailService = emailService;
            _userEmail = userEmail;
            _questions = _excelService.GetRandomQuestions(40);
            _currentQuestionIndex = 0;
            _score = 0;

            QuestionSuivanteCommand = new RelayCommand<object?>(async (param) => await ExecuteQuestionSuivante(param), CanExecuteQuestionSuivante);

            LoadCurrentQuestion();
        }

        private void LoadCurrentQuestion()
        {
            QuestionActuelle = _questions[_currentQuestionIndex];
            OnPropertyChanged(nameof(NumeroQuestionActuelle));
        }

        private bool CanExecuteQuestionSuivante(object? parameter)
        {
            return _currentQuestionIndex < _questions.Count;
        }

        private async Task ExecuteQuestionSuivante(object? parameter)
        {
            try
            {
                // Vérifier la réponse
                var reponseSelectionnee = QuestionActuelle.Reponses.FirstOrDefault(r => r.IsSelected);
                if (reponseSelectionnee != null && reponseSelectionnee.EstCorrect)
                {
                    _score++;
                }

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
            // Sauvegarder le score
            _excelService.SaveUserResult(_userEmail, _score);

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
