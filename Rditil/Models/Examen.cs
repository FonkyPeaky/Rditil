using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rditil.Models
{
    public class Examen
    {
        public int Id { get; set; }
        public DateTime DateExamen { get; set; }
        public int DureeExamen { get; set; } // en minutes
        public int Score { get; set; }

        public int UtilisateurId { get; set; }
        public Utilisateur Utilisateur { get; set; }
        public ICollection<ExamenQuestion> ExamenQuestions { get; set; }
    }

}
