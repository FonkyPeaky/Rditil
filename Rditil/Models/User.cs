using System;

namespace Rditil.Models
{
    public class User
    {
        public string Email { get; set; }
        public string Nom { get; set; }
        public string MotDePasse { get; set; }
        public int Score { get; set; }
        public DateTime DernierExamen { get; set; }
    }
} 