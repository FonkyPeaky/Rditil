using Rditil.Data;
using Rditil.Models;
using System.Linq;

namespace Rditil.Services
{
    public class DbUserService
    {
        private readonly AppDbContext _context;

        public DbUserService(AppDbContext context)
        {
            _context = context;
        }

        public Utilisateur GetUserByEmailAndPassword(string email, string password)
        {
            return _context.Utilisateurs.FirstOrDefault(u => u.Email == email && u.Password == password);
        }

        public bool EmailExists(string email)
        {
            return _context.Utilisateurs.Any(u => u.Email == email);
        }

        public void Register(Utilisateur user)
        {
            _context.Utilisateurs.Add(user);
            _context.SaveChanges();
        }
    }
}
