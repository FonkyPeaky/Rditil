using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rditil.Data;
using Rditil.Models;
using Microsoft.EntityFrameworkCore;

namespace Rditil.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;

        public AuthService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Utilisateur?> LoginAsync(string email, string password)
        {
            return await _context.Utilisateurs
                .FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
        }
    }
}

