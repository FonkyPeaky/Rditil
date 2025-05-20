using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rditil.Models
{
    public class Reponse
    {
        public int Id { get; set; }
        public string TextReponse { get; set; }
        public bool EstCorrect { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
    }
}
