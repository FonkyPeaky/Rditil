using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rditil.Models
{
    public class ExamenQuestion
    {
        public int Id { get; set; }
        public int ExamenId { get; set; }
        public Examen Examen { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        public int? ReponseChoisieId { get; set; } // optionnel pour stocker la réponse choisie
        public Reponse ReponseChoisie { get; set; }
    }
}
