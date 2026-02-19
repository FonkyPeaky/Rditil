using Microsoft.EntityFrameworkCore;
using Rditil.Data;
using Rditil.Models;
using Rditil.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Timer = System.Timers.Timer;

namespace Rditil.ViewModels
{
    /// <summary>
    /// ViewModel de l'examen.
    /// - Charge 40 questions aléatoires
    /// - Gère le timer 1h
    /// - Conserve les sélections utilisateur
    /// - Produit un rapport pour la EndPage
    /// </summary>
    public class ExamViewModel : ViewModelBase, INavigable
    {
        private readonly IDbContextFactory<AppDbContext> _dbFactory;
        private readonly INavigationService _navigation;
        private readonly IAppState _appState;

        private Task? _loadTask;
        private bool _isLoaded;

        private readonly Timer _timer;
        private DateTime _examEndUtc;
        private DateTime _startedAtUtc;

        private int _index;
        private ObservableCollection<Question> _questions = new();

        private readonly Dictionary<int, int?> _chosenByQuestionId = new();

        public ObservableCollection<Question> Questions
        {
            get => _questions;
            private set { _questions = value; OnPropertyChanged(); }
        }

        private Question? _currentQuestion;
        public Question? CurrentQuestion
        {
            get => _currentQuestion;
            private set
            {
                _currentQuestion = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Enonce));
            }
        }

        public string QuestionNumeroText => Questions.Count == 0
            ? ""
            : $"Question {_index + 1}/{Questions.Count}";

        public string Enonce => CurrentQuestion?.Enonce ?? "";

        private ObservableCollection<AnswerChoiceVm> _reponsesChoix = new();
        public ObservableCollection<AnswerChoiceVm> ReponsesChoix
        {
            get => _reponsesChoix;
            private set { _reponsesChoix = value; OnPropertyChanged(); }
        }

        private string _tempsRestantText = "--:--:--";
        public string TempsRestantText
        {
            get => _tempsRestantText;
            set { _tempsRestantText = value; OnPropertyChanged(); }
        }

        public RelayCommand NextQuestionCommand { get; }
        public RelayCommand PrevQuestionCommand { get; }
        public RelayCommand FinishExamCommand { get; }

        public ExamViewModel(
            IDbContextFactory<AppDbContext> dbFactory,
            INavigationService navigation,
            IAppState appState)
        {
            _dbFactory = dbFactory;
            _navigation = navigation;
            _appState = appState;

            NextQuestionCommand = new RelayCommand(NextQuestion);
            PrevQuestionCommand = new RelayCommand(PrevQuestion);
            FinishExamCommand = new RelayCommand(ConfirmAndFinishExam);

            _timer = new Timer(250);
            _timer.Elapsed += (_, __) =>
            {
                var remaining = _examEndUtc - DateTime.UtcNow;
                if (remaining < TimeSpan.Zero) remaining = TimeSpan.Zero;

                App.Current.Dispatcher.Invoke(() =>
                {
                    TempsRestantText = remaining.ToString(@"hh\:mm\:ss");
                });

                if (remaining == TimeSpan.Zero)
                {
                    _timer.Stop();
                    App.Current.Dispatcher.Invoke(FinishExam);
                }
            };
        }

        public void OnNavigatedTo(Dictionary<string, object?>? parameters)
{
    System.Diagnostics.Debug.WriteLine("[ExamViewModel] OnNavigatedTo -> EnsureLoadedAsync()");
    _ = EnsureLoadedAsync();
}


        public Task EnsureLoadedAsync() => _loadTask ??= LoadQuestionsAsync();

        private async Task LoadQuestionsAsync()
        {
            if (_isLoaded) return;

            _startedAtUtc = DateTime.UtcNow;
            _examEndUtc = _startedAtUtc.AddHours(1);
            TempsRestantText = TimeSpan.FromHours(1).ToString(@"hh\:mm\:ss");
            _timer.Start();

            try
            {
                await using var db = await _dbFactory.CreateDbContextAsync();

                var list = await db.Questions
                    .Include(q => q.Reponses)
                    .OrderBy(_ => Guid.NewGuid())
                    .Take(40)
                    .ToListAsync();

                App.Current.Dispatcher.Invoke(() =>
                {
                    Questions = new ObservableCollection<Question>(list);
                    _index = 0;
                    _isLoaded = true;
                    ShowCurrentQuestion();
                });
            }
            catch (Exception ex)
            {
                _timer.Stop();
                System.Diagnostics.Debug.WriteLine(ex);

                App.Current.Dispatcher.Invoke(() =>
                {
                    TempsRestantText = "--:--:--";
                    MessageBox.Show(
                        "Impossible de charger les questions depuis la base de données.\n\n" +
                        "Cause probable : mapping EF / colonnes DB (ex: reponses.texte / reponses.est_correcte).\n\n" +
                        ex.Message,
                        "Erreur chargement examen",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                });
            }
        }

        private void ShowCurrentQuestion()
        {
            if (Questions.Count == 0)
            {
                CurrentQuestion = null;
                ReponsesChoix = new ObservableCollection<AnswerChoiceVm>();
                TempsRestantText = "00:00:00";
                OnPropertyChanged(nameof(QuestionNumeroText));
                return;
            }

            if (_index < 0) _index = 0;
            if (_index >= Questions.Count) _index = Questions.Count - 1;

            var q = Questions[_index];
            CurrentQuestion = q;
            OnPropertyChanged(nameof(QuestionNumeroText));

            _chosenByQuestionId.TryGetValue(q.Id, out var chosenId);

            var choices = q.Reponses
                .Select(r => new AnswerChoiceVm(
                    id: r.Id_Reponse,
                    text: r.TextReponse ?? "",
                    isCorrect: r.EstCorrect,
                    isSelected: chosenId.HasValue && chosenId.Value == r.Id_Reponse,
                    onSelected: () => SelectSingleChoice(q.Id, r.Id_Reponse)))
                .ToList();

            ReponsesChoix = new ObservableCollection<AnswerChoiceVm>(choices);
        }

        private void SelectSingleChoice(int questionId, int responseId)
        {
            _chosenByQuestionId[questionId] = responseId;

            // Radio behaviour (1 seul choix)
            foreach (var c in ReponsesChoix)
                c.SetSelectedSilently(c.Id == responseId);
        }

        private void NextQuestion()
        {
            if (!_isLoaded || Questions.Count == 0) return;
            _index++;
            if (_index >= Questions.Count) _index = Questions.Count - 1;
            ShowCurrentQuestion();
        }

        private void PrevQuestion()
        {
            if (!_isLoaded || Questions.Count == 0) return;
            _index--;
            if (_index < 0) _index = 0;
            ShowCurrentQuestion();
        }

        private void ConfirmAndFinishExam()
        {
            if (!_isLoaded) return;

            var r = MessageBox.Show(
                "Vous êtes sûr de vouloir terminer l'examen ?",
                "Confirmer",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (r == MessageBoxResult.Yes)
                FinishExam();
        }

        private async void FinishExam()
        {
            if (!_isLoaded) return;
            _timer.Stop();

            var finishedAtUtc = DateTime.UtcNow;

            var rows = new List<ExamReportRow>();
            int score = 0;
            int total = Questions.Count;

            for (int i = 0; i < Questions.Count; i++)
            {
                var q = Questions[i];
                _chosenByQuestionId.TryGetValue(q.Id, out var chosenId);

                var correct = q.Reponses.FirstOrDefault(r => r.EstCorrect);
                var chosen = chosenId.HasValue ? q.Reponses.FirstOrDefault(r => r.Id_Reponse == chosenId.Value) : null;

                var isCorrect = chosen != null && correct != null && chosen.Id_Reponse == correct.Id_Reponse;
                if (isCorrect) score++;

                rows.Add(new ExamReportRow
                {
                    Index = i + 1,
                    Enonce = q.Enonce,
                    ChosenText = chosen?.TextReponse ?? "(aucun choix)",
                    CorrectText = correct?.TextReponse ?? "",
                    IsCorrect = isCorrect
                });
            }

            var u = _appState.CurrentUser;
            var fullName = u == null ? "" : $"{u.Prenom} {u.Nom}".Trim();
            try
            {
                if (u != null)
                {
                    await using var db = await _dbFactory.CreateDbContextAsync();

                    var examen = new Examen
                    {
                        DateExamen = finishedAtUtc,
                        DureeExamen = finishedAtUtc - _startedAtUtc,
                        Score = score,
                        Id_Utilisateur = u.Id_Utilisateur,

                    };

                    db.Examens.Add(examen);

                    var fkProp = db.Entry(examen).Metadata.FindProperty("UtilisateurId_Utilisateur");
                    if (fkProp != null)
                    {
                        db.Entry(examen).Property("UtilisateurId_Utilisateur").CurrentValue = u.Id_Utilisateur;
                    }

                    await db.SaveChangesAsync();

                    var attempt = new ExamAttempt
                    {
                        ExamenId = examen.Id_Examen,
                        ExamenId_Examen = examen.Id_Examen,

                        Id_Utilisateur = u.Id_Utilisateur,
                        StartedAt = _startedAtUtc,
                        FinishedAt = finishedAtUtc,
                        Score = score,
                        TotalQuestions = total,
                        UserFullName = fullName
                    };

                    db.ExamAttempts.Add(attempt);
                    await db.SaveChangesAsync();

                    foreach (var row in rows)
                    {
                        var q = Questions[row.Index - 1];
                        _chosenByQuestionId.TryGetValue(q.Id, out var chosenId);

                        db.ExamAnswers.Add(new ExamAnswer
                        {
                            ExamAttemptId = attempt.Id_ExamAttempt,
                            QuestionId = q.Id,
                            SelectedReponseId = chosenId,
                            IsCorrect = row.IsCorrect
                        });
                    }

                    var userEntity = await db.Utilisateurs.FirstOrDefaultAsync(x => x.Id_Utilisateur == u.Id_Utilisateur);
                    if (userEntity != null)
                    {
                        userEntity.Score = score;
                        userEntity.DernierExamen = finishedAtUtc;
                    }

                    await db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            _appState.LastExamRows = rows;
            _appState.LastExamResult = new ExamResultSummary
            {
                UserFullName = fullName,
                UserEmail = u?.Email ?? "",
                ManagerEmail = _appState.ManagerEmail ?? u?.EmailNPlus1 ?? "",
                Score = score,
                Total = total,
                StartedAt = _startedAtUtc,
                FinishedAt = finishedAtUtc
            };

            _navigation.NavigateTo<EndPageViewModel>();
        }
    }

    public sealed class AnswerChoiceVm : ViewModelBase
    {
        private readonly Action _onSelected;

        public int Id { get; }
        public string Text { get; }
        public bool IsCorrect { get; }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected == value) return;
                _isSelected = value;
                OnPropertyChanged();
                if (value) _onSelected();
            }
        }

        public AnswerChoiceVm(int id, string text, bool isCorrect, bool isSelected, Action onSelected)
        {
            Id = id;
            Text = text;
            IsCorrect = isCorrect;
            _isSelected = isSelected;
            _onSelected = onSelected;
        }

        public void SetSelectedSilently(bool value)
        {
            if (_isSelected == value) return;
            _isSelected = value;
            OnPropertyChanged(nameof(IsSelected));
        }
    }
}
