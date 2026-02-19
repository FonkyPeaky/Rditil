using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rditil.Models
{
    public class ExamResultDto
    {
        public string Candidat { get; set; } = "";
        public DateTime Date { get; set; }
        public int Score { get; set; }
        public int Total { get; set; }
        public TimeSpan TempsPasse { get; set; }

        public List<QuestionResultDto> Questions { get; set; } = new();
    }
}
