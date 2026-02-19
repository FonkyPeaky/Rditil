using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rditil.Models;

public class Question
{
    [Key]
    public int Id { get; set; }

    public string Intitule { get; set; } = string.Empty;

    public string Enonce { get; set; } = string.Empty;

    public ICollection<Reponse> Reponses { get; set; } = new List<Reponse>();

    [NotMapped]
    public string DisplayText => string.IsNullOrWhiteSpace(Intitule) ? Enonce : Intitule;
}
