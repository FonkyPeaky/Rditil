using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rditil.Models
{
    [Table("Utilisateurs")]
    public class Utilisateur
    {
        [Key]
        [Column("Id_Utilisateur")]
        public int Id_Utilisateur { get; set; }

        [Column("Nom")]
        public string Nom { get; set; }

        [Column("Prenom")]
        public string Prenom { get; set; }

        [Column("Email")]
        public string Email { get; set; }

        [Column("Password")]
        public string Password { get; set; }

        [Column("Score")]
        public int Score { get; set; } = 0;

        [Column("DernierExamen")]
        public DateTime DernierExamen { get; set; } = DateTime.UtcNow;

    }
}
