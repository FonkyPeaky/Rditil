using CommunityToolkit.Mvvm.ComponentModel;

namespace Rditil.Models
{
    public partial class ReponseChoix : ObservableObject
    {
        public int Id { get; set; }
        public string TextReponse { get; set; } = string.Empty;
        public bool EstCorrect { get; set; }

        [ObservableProperty]
        private bool isChoisie;
    }
}


