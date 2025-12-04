using CommunityToolkit.Mvvm.ComponentModel;

namespace Rditil.ViewModels
{
    public partial class ResultViewModel : ObservableObject
    {
        [ObservableProperty] private int score;
        [ObservableProperty] private int total;
        [ObservableProperty] private TimeSpan timeUsed;
        [ObservableProperty] private bool timeExpired;

        public string ScoreText => $"{Score}/{Total}";
        public string TimeUsedText => string.Format("{0:00}:{1:00}:{2:00}",
            (int)TimeUsed.TotalHours, TimeUsed.Minutes, TimeUsed.Seconds);

        public string Title => TimeExpired ? "Temps imparti !" : "Examen terminé";
        public string Subtitle => TimeExpired
            ? "Le temps est écoulé. Votre score ci-dessous a été comptabilisé."
            : "Félicitations ! Voici votre score et votre temps.";

        public ResultViewModel(int score, int total, TimeSpan used, bool expired)
        {
            Score = score; Total = total; TimeUsed = used; TimeExpired = expired;
        }
    }
}
