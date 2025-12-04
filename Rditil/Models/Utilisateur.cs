using System.ComponentModel.DataAnnotations;

namespace Rditil.Models
{
    public class Utilisateur
    {
        [Key]
        public int Id_Utilisateur { get; set; }

        [Required, MaxLength(100)]
        public string Nom { get; set; } = "";

        [Required, MaxLength(100)]
        public string Prenom { get; set; } = "";

        [Required, EmailAddress, MaxLength(255)]
        public string Email { get; set; } = "";

        [Required]
        public string PasswordHash { get; set; } = "";

        [EmailAddress, MaxLength(255)]
        public string? EmailNPlus1 { get; set; }

        public int Score { get; set; } = 0;

        public DateTime DernierExamen { get; set; } = DateTime.UtcNow;
    }
}
