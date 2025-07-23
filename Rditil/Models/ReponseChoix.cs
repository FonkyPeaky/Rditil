using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Rditil.Models
{
    public partial class ReponseChoix : ObservableObject
    {
        [Key]
        public int Id { get; set; }
        public string TextReponse { get; set; } = string.Empty;
        public bool EstCorrect { get; set; }

        [ObservableProperty]
        private bool isChoisie;
    }
}


