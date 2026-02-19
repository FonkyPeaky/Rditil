<<<<<<< HEAD
﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Threading;
using Rditil;

=======
﻿using CommunityToolkit.Mvvm.ComponentModel;
using Rditil.Models;
using System.Collections.ObjectModel;
using System.Windows;
using Timer = System.Timers.Timer;
>>>>>>> 1908e362463a42654d4b699764460fd7abed45f8

namespace Rditil.ViewModels
{
    public class QuestionChoiceVM : INotifyPropertyChanged
    {
        private bool _isSelected;

        public int Id { get; set; }
        public string Text { get; set; } = "";

        public bool IsSelected
        {
            get => _isSelected;
            set { _isSelected = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? n = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
    }

    public class QuestionViewModel : INotifyPropertyChanged
    {
        private readonly DispatcherTimer _timer = new() { Interval = TimeSpan.FromSeconds(1) };
        private TimeSpan _remaining = TimeSpan.FromHours(1);

        private string _currentQuestionText = "";
        private int _currentIndex = 1;
        private int _totalQuestions = 40;

        public ObservableCollection<QuestionChoiceVM> Choices { get; } = new();

        public string CurrentQuestionText
        {
            get => _currentQuestionText;
            set { _currentQuestionText = value; OnPropertyChanged(); }
        }

        public int CurrentIndex
        {
            get => _currentIndex;
            set { _currentIndex = value; OnPropertyChanged(); }
        }

        public int TotalQuestions
        {
            get => _totalQuestions;
            set { _totalQuestions = value; OnPropertyChanged(); }
        }

        public string RemainingTimeText => _remaining.ToString(@"hh\:mm\:ss");

        public bool CanGoNext => Choices.Any(c => c.IsSelected);

        public string NextButtonText => (CurrentIndex >= TotalQuestions) ? "Terminer" : "Suivant";

        public ICommand NextCommand { get; }

        public QuestionViewModel()
        {
            NextCommand = new RelayCommand(_ => GoNext(), _ => CanGoNext);

            _timer.Tick += (_, __) =>
            {
                if (_remaining > TimeSpan.Zero)
                {
                    _remaining = _remaining.Subtract(TimeSpan.FromSeconds(1));
                    OnPropertyChanged(nameof(RemainingTimeText));
                }
                else
                {
                    _timer.Stop();
                }
            };

            Choices.CollectionChanged += (_, __) => RefreshCanExecute();
        }

        //public void StartTimer()
        //{
        //    _timer.Start();
        //    OnPropertyChanged(nameof(RemainingTimeText));
        //}

        public void StopTimer() => _timer.Stop();

        public void LoadQuestion(string question, (int id, string txt)[] answers, int index, int total)
        {
            CurrentQuestionText = question;
            CurrentIndex = index;
            TotalQuestions = total;

            Choices.Clear();
            foreach (var a in answers)
                Choices.Add(new QuestionChoiceVM { Id = a.id, Text = a.txt });

            foreach (var c in Choices)
                c.PropertyChanged += (_, e) =>
                {
                    if (e.PropertyName == nameof(QuestionChoiceVM.IsSelected))
                        RefreshCanExecute();
                };

            RefreshCanExecute();
            OnPropertyChanged(nameof(NextButtonText));
        }

        private void GoNext()
        {
            var selected = Choices.Where(c => c.IsSelected).ToList();
        }

        private void RefreshCanExecute()
        {
            OnPropertyChanged(nameof(CanGoNext));
            OnPropertyChanged(nameof(NextButtonText));

            if (NextCommand is RelayCommand rc)
                rc.RaiseCanExecuteChanged();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? n = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
    }
}
