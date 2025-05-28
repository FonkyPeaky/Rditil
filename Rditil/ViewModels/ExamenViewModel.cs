using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Rditil.Models;
using Rditil.Services;
using CommunityToolkit.Mvvm.Input;

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

            QuestionSuivanteCommand = new RelayCommand<object?>(ExecuteQuestionSuivante, CanExecuteQuestionSuivante);

            LoadCurrentQuestion();
        }

        private void LoadCurrentQuestion()
        {
            QuestionActuelle = _questions[_currentQuestionIndex];
            OnPropertyChanged(nameof(NumeroQuestionActuelle));
        }

        private bool CanExecuteQuestionSuivante(object? parameter)
        {
            return _currentQuestionIndex < _questions.Count - 1;
        }

        private async void ExecuteQuestionSuivante(object? parameter)
        {
            // Vérifier la réponse
            var reponseSelectionnee = QuestionActuelle.Reponses.FirstOrDefault(r => r.IsSelected);
            if (reponseSelectionnee != null && reponseSelectionnee.EstCorrect)
            {
                _score++;
            }

            if (_currentQuestionIndex < _questions.Count - 1)
            {
                _currentQuestionIndex++;
                LoadCurrentQuestion();
            }
            else
            {
                // Fin de l'examen
                _excelService.SaveUserResult(_userEmail, _score);
                
                // Envoyer l'email avec les résultats
                await _emailService.SendExamResultAsync(
                    "issam.elafi@randstaddigital.lu",
                    _userEmail,
                    _score,
                    _questions.Count
                );

                // Fermer la fenêtre
                Application.Current.Windows.OfType<Window>()
                    .FirstOrDefault(w => w.DataContext == this)?.Close();
            }
        }
    }
} 