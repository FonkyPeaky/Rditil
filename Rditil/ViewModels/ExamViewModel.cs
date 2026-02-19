using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Rditil.Data;
using Rditil.Models;
using Rditil.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Threading;

namespace Rditil.ViewModels
{
    public partial class ExamViewModel : ObservableObject
    {
        private readonly AppDbContext _ctx;
        private readonly IEmailService _emailService;
        private readonly INavigationService _navigationService;

        public Utilisateur CurrentUser { get; set; } = null!;
        public string UserEmail { get; set; } = "";
        public string ManagerEmail { get; set; } = "";

        private readonly DispatcherTimer _timer;
        private TimeSpan _remaining = TimeSpan.FromHours(1);
        private DateTime _startedAtUtc;

        private List<Question> _questions = new();
        private int _index;
        private int _score;

        [ObservableProperty] private string tempsRestantText = "01:00:00";
        [ObservableProperty] private Question questionEnCours = null!;
        [ObservableProperty] private ObservableCollection<ReponseChoix> reponsesChoix = new();

        // ✅ Pour l’affichage en haut (gauche) : "RDITIL • Question 3/40"
        public string ProgressText => _questions.Count == 0 ? "0/0" : $"{Math.Min(_index + 1, _questions.Count)}/{_questions.Count}";
        public string HeaderLeftText => $"RDITIL  •  Question {ProgressText}";

        public IAsyncRelayCommand QuestionSuivanteCommand { get; }

        public ExamViewModel(
            AppDbContext ctx,
            IEmailService emailService,
            INavigationService navigationService)
        {
            _ctx = ctx;
            _emailService = emailService;
            _navigationService = navigationService;

            QuestionSuivanteCommand = new AsyncRelayCommand(ValiderEtSuivantAsync);

            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _timer.Tick += (_, __) =>
            {
                _remaining -= TimeSpan.FromSeconds(1);
                TempsRestantText = _remaining.ToString(@"hh\:mm\:ss");

                if (_remaining <= TimeSpan.Zero)
                {
                    _timer.Stop();
                    FinishExam();
                }
            };
        }

        public async void OnNavigatedTo()
        {
            _timer.Stop();

            // ✅ infos utilisateur (pour l'écran de fin + email)
            CurrentUser = App.CurrentUser ?? new Utilisateur();
            UserEmail = CurrentUser.Email;
            ManagerEmail = CurrentUser.EmailNPlus1;
            

            _remaining = TimeSpan.FromHours(1);
            TempsRestantText = _remaining.ToString(@"hh\:mm\:ss");

            var all = await _ctx.Questions
                .Include(q => q.Reponses)
                .ToListAsync();

            _questions = all.OrderBy(_ => Guid.NewGuid()).Take(40).ToList();
            _index = 0;
            _score = 0;

            OnPropertyChanged(nameof(ProgressText));
            OnPropertyChanged(nameof(HeaderLeftText));

            ChargerQuestion();
            _startedAtUtc = DateTime.UtcNow;
            _timer.Start();
        }

        private void ChargerQuestion()
        {
            if (_questions.Count == 0) return;

            QuestionEnCours = _questions[_index];

            ReponsesChoix = new ObservableCollection<ReponseChoix>(
                QuestionEnCours.Reponses.Select(r => new ReponseChoix
                {
                    Id = r.Id_Reponse,
                    TextReponse = r.TextReponse ?? "",
                    EstCorrect = r.EstCorrect
                }));

            OnPropertyChanged(nameof(ProgressText));
            OnPropertyChanged(nameof(HeaderLeftText));
        }

        private async System.Threading.Tasks.Task ValiderEtSuivantAsync()
        {
            var selected = ReponsesChoix.Where(r => r.IsChoisie).Select(r => r.Id).ToHashSet();
            var correct = ReponsesChoix.Where(r => r.EstCorrect).Select(r => r.Id).ToHashSet();

            if (selected.SetEquals(correct))
                _score++;

            _index++;

            if (_index < _questions.Count)
                ChargerQuestion();
            else
                FinishExam();

            await System.Threading.Tasks.Task.CompletedTask;
        }

        private void FinishExam()
        {
            _timer.Stop();

            // ✅ On garde le résultat en mémoire (la EndPage va l'afficher + permettre l'envoi mail)
            App.LastExamResult = new Models.ExamResultSummary
            {
                UserFullName = CurrentUser?.Nom ?? string.Empty,
                UserEmail = UserEmail ?? string.Empty,
                ManagerEmail = ManagerEmail ?? string.Empty,
                Score = _score,
                Total = _questions.Count,
                StartedAt = _startedAtUtc,
                FinishedAt = DateTime.UtcNow
            };

            _navigationService.NavigateTo<EndPageViewModel>();
        }
    }
}
