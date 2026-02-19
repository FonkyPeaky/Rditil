<<<<<<< HEAD
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rditil.Models;

public class Question
=======
﻿namespace Rditil.Models
>>>>>>> 1908e362463a42654d4b699764460fd7abed45f8
{
    [Key]
    public int Id { get; set; }

<<<<<<< HEAD
    public string Intitule { get; set; } = string.Empty;

    public string Enonce { get; set; } = string.Empty;

    public ICollection<Reponse> Reponses { get; set; } = new List<Reponse>();

    [NotMapped]
    public string DisplayText => string.IsNullOrWhiteSpace(Intitule) ? Enonce : Intitule;
=======
        public ICollection<Reponse> Reponses { get; set; } = new List<Reponse>();
    }
>>>>>>> 1908e362463a42654d4b699764460fd7abed45f8
}
