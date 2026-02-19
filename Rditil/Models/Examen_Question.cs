<<<<<<< HEAD
﻿using System.ComponentModel.DataAnnotations;

namespace Rditil.Models
=======
﻿namespace Rditil.Models
>>>>>>> 1908e362463a42654d4b699764460fd7abed45f8
{
    public class Examen_Question
    {
        [Key]
        public int Id_Exam { get; set; }
        public int Id_Question { get; set; }

        public Examen? Examen { get; set; }
        public Question? Question { get; set; }
    }
}
