using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rditil.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Enonce { get; set; }
        public ICollection<Reponse> Reponses { get; set; }
        public ICollection<ExamenQuestion> ExamenQuestions { get; set; }
    }
}
