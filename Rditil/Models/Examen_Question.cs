namespace Rditil.Models
{
    public class Examen_Question
    {
        public int Id_Examen { get; set; }
        public Examen Examen { get; set; }
        public int Id_Question { get; set; }
        public Question Question { get; set; }
    }
}

