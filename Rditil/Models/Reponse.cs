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
        [Key]
        public int Id_Reponse { get; set; }
        public string? TextReponse { get; set; }
        public bool EstCorrect { get; set; }

        public int Id_Question { get; set; }
        public Question? Question { get; set; }
    }

}
