using System.ComponentModel.DataAnnotations;

namespace Rditil.Models
{
    public class Reponse
    {
        [Key]
        public int Id_Reponse { get; set; }
        public string? TextReponse { get; set; }
        public bool EstCorrect { get; set; }

        public int Id_Question { get; set; }
        public Question? Question { get; set; }
    }
}
