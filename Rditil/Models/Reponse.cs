using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rditil.Models
{
    public class Reponse
    {
        public int Id_Reponse { get; set; }
        public string? TextReponse { get; set; }
        public bool EstCorrect { get; set; }

        // Unique FK
        public int Id_Question { get; set; }

        // Navigation
        public Question? Question { get; set; }
    }
}

