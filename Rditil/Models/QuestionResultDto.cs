using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rditil.Models
{
    public class QuestionResultDto
    {
        public string Enonce { get; set; } = "";
        public string ReponseChoisie { get; set; } = "";
        public bool EstCorrecte { get; set; }
    }
}
