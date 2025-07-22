using System;
using System.Collections.Generic;

namespace Rditil.Models
{
    public class Utilisateur
    {
        public int Id_Utilisateur { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public ICollection<Examen> Examens { get; set; }
    }

}
