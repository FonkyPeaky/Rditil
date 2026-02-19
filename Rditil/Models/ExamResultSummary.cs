namespace Rditil.Models
{
    /// <summary>
    /// Petit DTO en mémoire pour afficher la page de fin et préparer l'email.
    /// (On évite de dépendre d'une structure DB spécifique.)
    /// </summary>
    public class ExamResultSummary
    {
        public string UserFullName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public string ManagerEmail { get; set; } = string.Empty;

        public int Score { get; set; }
        public int Total { get; set; }

        /// <summary>
        /// Début d'examen (UTC). Utilisé par l'UI.
        /// </summary>
        public DateTime StartedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Fin d'examen (UTC). Utilisé par l'UI.
        /// </summary>
        public DateTime FinishedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Texte de durée formaté (hh:mm:ss) pour l'affichage.
        /// </summary>
        public string DurationText { get; set; } = string.Empty;

        // Ancienne propriété conservée pour compatibilité si elle est encore bindée quelque part.
        public DateTime FinishedAtUtc
        {
            get => FinishedAt;
            set => FinishedAt = value;
        }
    }
}
