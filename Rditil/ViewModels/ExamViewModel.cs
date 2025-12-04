using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Rditil.Data;
using Rditil.Models;
using Rditil.Services;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace Rditil.ViewModels
{
    public class ExamFinishedEventArgs : EventArgs
    {
        public int Score { get; init; }
        public int Total { get; init; }
        public TimeSpan TimeUsed { get; init; }
        public bool TimeExpired { get; init; }
    }

    public partial class ExamViewModel : ObservableObject
    {
        private readonly AppDbContext _ctx;
        private readonly IEmailService _emailService;
        private readonly string _managerEmail;
        private readonly string _userEmail;

        private readonly DispatcherTimer _timer;
        private DateTime _startUtc;
        private TimeSpan _remaining = TimeSpan.FromHours(1);

        [ObservableProperty] private string tempsRestantText = "01:00:00";
        [ObservableProperty] private Question questionEnCours = null!;
        [ObservableProperty] private ObservableCollection<ReponseChoix> reponsesChoix = new();

        public string? UserFullName { get; set; }

        private List<Question> _questions = new();
        private int _index = 0;
        private int _score = 0;

        public event EventHandler<ExamFinishedEventArgs> ExamFinished;


        // 🌟 AJOUT : Progression → utilisée par la ProgressBar
        public double Progression
        {
            get
            {
                if (_questions == null || _questions.Count == 0)
                    return 0;

                return (_index / (double)_questions.Count) * 100.0;
            }
        }


        public IAsyncRelayCommand QuestionSuivanteCommand { get; }
        public IAsyncRelayCommand DemarrerCommand { get; }

        public ExamViewModel(AppDbContext ctx, IEmailService emailService, string userEmail, string managerEmail)
        {
            _ctx = ctx;
            _emailService = emailService;
            _userEmail = userEmail;
            _managerEmail = managerEmail;

            QuestionSuivanteCommand = new AsyncRelayCommand(ValiderEtSuivantAsync);
            DemarrerCommand = new AsyncRelayCommand(DemarrerAsync);

            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _timer.Tick += (_, __) =>
            {
                _remaining -= TimeSpan.FromSeconds(1);
                TempsRestantText = _remaining.ToString(@"hh\:mm\:ss");

                if (_remaining <= TimeSpan.Zero)
                {
                    _timer.Stop();
                    Finish(timeExpired: true);
                }
            };
        }


        // Méthode déclenchée depuis ExamenView.xaml.cs
        public async void Demarrer() => await DemarrerAsync();


        private async Task DemarrerAsync()
        {
            _startUtc = DateTime.UtcNow;
            _remaining = TimeSpan.FromHours(1);
            TempsRestantText = _remaining.ToString(@"hh\:mm\:ss");

            _score = 0;
            _index = 0;

            // 🔥 IMPORTANT : notifier la ProgressBar
            OnPropertyChanged(nameof(Progression));

            // Charger 40 questions tirées au hasard
            var all = await _ctx.Questions
                .Include(q => q.Reponses)
                .ToListAsync();

            _questions = all.OrderBy(_ => Guid.NewGuid()).Take(40).ToList();

            ChargerQuestion(_index);
            _timer.Start();
        }


        private void ChargerQuestion(int i)
        {
            if (i < 0 || i >= _questions.Count)
                return;

            QuestionEnCours = _questions[i];

            var items = QuestionEnCours.Reponses
                .Select(r => new ReponseChoix
                {
                    Id = r.Id_Reponse,
                    TextReponse = r.TextReponse ?? string.Empty,
                    EstCorrect = r.EstCorrect
                })
                .OrderBy(_ => Guid.NewGuid())
                .ToList();

            ReponsesChoix = new ObservableCollection<ReponseChoix>(items);
        }


        private async Task ValiderEtSuivantAsync()
        {
            // Vérifier la bonne réponse
            var selected = ReponsesChoix.Where(x => x.IsChoisie).Select(x => x.Id).ToHashSet();
            var correct = ReponsesChoix.Where(x => x.EstCorrect).Select(x => x.Id).ToHashSet();

            if (selected.SetEquals(correct))
                _score++;

            // Passer à la question suivante
            _index++;

            // 🔥 IMPORTANT : notifier la progression
            OnPropertyChanged(nameof(Progression));

            if (_index < _questions.Count)
            {
                ChargerQuestion(_index);
            }
            else
            {
                _timer.Stop();
                Finish(timeExpired: false);
            }

            await Task.CompletedTask;
        }


        private void Finish(bool timeExpired)
        {
            var used = DateTime.UtcNow - _startUtc;

            try
            {
                _ = _emailService.SendExamResultAsync(_managerEmail, _userEmail, _score, _questions.Count);
            }
            catch
            {
                // On ne bloque pas si l'email échoue
            }

            ExamFinished?.Invoke(this, new ExamFinishedEventArgs
            {
                Score = _score,
                Total = _questions.Count,
                TimeUsed = used,
                TimeExpired = timeExpired
            });
        }
    }
}
