using System.ComponentModel.DataAnnotations;

namespace Rditil.Models
{
    public class Examen_Question
    {
        [Key]
        public int Id_Exam { get; set; }
        public int Id_Question { get; set; }

        public Examen? Examen { get; set; }
        public Question? Question { get; set; }
    }
}
