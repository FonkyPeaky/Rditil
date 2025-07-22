using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rditil.Models
{
    public class Examen
    {
        public int Id_Examen { get; set; }
        public DateTime DateExamen { get; set; }
        public TimeSpan DureeExamen { get; set; }
        public int SCORE { get; set; }

        public int Id_Utilisateur { get; set; }
        public Utilisateur Utilisateur { get; set; }

        public ICollection<Examen_Question> ExamenQuestions { get; set; }
    }


}
