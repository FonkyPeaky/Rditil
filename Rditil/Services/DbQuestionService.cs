using Microsoft.EntityFrameworkCore;
using Rditil.Data;
using Rditil.Models;
using System.Collections.Generic;
using System.Linq;

namespace Rditil.Services
{
    public class DbQuestionService
    {
        private readonly AppDbContext _context;

        public DbQuestionService(AppDbContext context)
        {
            _context = context;
        }

        public List<Question> GetAllQuestions()
        {
            return _context.Questions
                .Include(q => q.Reponses)
                .ToList();
        }

        public Question GetQuestionById(int id)
        {
            return _context.Questions
                .Include(q => q.Reponses)
                .FirstOrDefault(q => q.Id_Question == id);
        }
    }
}
