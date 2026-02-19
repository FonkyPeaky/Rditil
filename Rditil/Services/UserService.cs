using Microsoft.EntityFrameworkCore;
using Rditil.Data;
using Rditil.Models;
using System.Threading.Tasks;

namespace Rditil.Services
{
    public interface IUserService
    {
        Task<Utilisateur?> GetByIdAsync(int id);
        Task<Utilisateur?> GetByEmailAsync(string email);
        Task AddAsync(Utilisateur user);
        Task SaveAsync();
    }

    public class UserService : IUserService
    {
        private readonly IDbContextFactory<AppDbContext> _dbFactory;

        public UserService(IDbContextFactory<AppDbContext> dbFactory)
            => _dbFactory = dbFactory;

        public async Task<Utilisateur?> GetByIdAsync(int id)
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            return await db.Utilisateurs.FirstOrDefaultAsync(u => u.Id_Utilisateur == id);
        }

        public async Task<Utilisateur?> GetByEmailAsync(string email)
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            return await db.Utilisateurs.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task AddAsync(Utilisateur user)
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            await db.Utilisateurs.AddAsync(user);
            await db.SaveChangesAsync();
        }

        public async Task SaveAsync()
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            await db.SaveChangesAsync();
        }
    }
}
