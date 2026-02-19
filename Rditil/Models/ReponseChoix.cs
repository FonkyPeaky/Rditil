namespace Rditil.Models
{
    public partial class ReponseChoix
    {
        public int Id { get; set; }
        public string? Texte { get; set; }
        public bool EstCorrecte { get; set; }

        public bool IsSelected { get; set; }
    }
}
