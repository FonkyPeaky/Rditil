<<<<<<< HEAD
﻿using System.ComponentModel.DataAnnotations;

namespace Rditil.Models
=======
﻿namespace Rditil.Models
>>>>>>> 1908e362463a42654d4b699764460fd7abed45f8
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
