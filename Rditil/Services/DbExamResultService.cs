using Rditil.Data;
using Rditil.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rditil.Services
{
    public class DbExamResultService
    {
        private readonly AppDbContext _context;

        public DbExamResultService(AppDbContext context)
        {
            _context = context;
        }

        public void EnregistrerExamen(Utilisateur utilisateur, int score, List<Question> questions)
        {
            var examen = new Examen
            {
                DateExamen = DateTime.Now,
                DureeExamen = TimeSpan.FromMinutes(60),
                SCORE = score,
                Id_Utilisateur = utilisateur.Id_Utilisateur,
                ExamenQuestions = new List<Examen_Question>()
            };

            foreach (var question in questions)
            {
                examen.ExamenQuestions.Add(new Examen_Question
                {
                    Id_Question = question.Id_Question,
                    Examen = examen
                });
            }

            _context.Examens.Add(examen);
            _context.SaveChanges();
        }
    }
}
