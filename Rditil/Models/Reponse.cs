namespace Rditil.Models
{
    public class Reponse
    {
        public int Id_Reponse { get; set; }
        public string? TextReponse { get; set; }
        public bool EstCorrect { get; set; }

        // Unique FK
        public int Id_Question { get; set; }

        // Navigation
        public Question? Question { get; set; }
    }
}

