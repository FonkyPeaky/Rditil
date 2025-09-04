using Microsoft.EntityFrameworkCore;
using Rditil.Data;
using Rditil.Models;
//using Rditil.AppDbContext; // adapte si ton DbContext est dans un autre namespace
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
        private readonly AppDbContext _ctx;
        public UserService(AppDbContext ctx) => _ctx = ctx;

        public Task<Utilisateur?> GetByIdAsync(int id) =>
            _ctx.Utilisateurs.FirstOrDefaultAsync(u => u.Id_Utilisateur == id);

        public Task<Utilisateur?> GetByEmailAsync(string email) =>
            _ctx.Utilisateurs.FirstOrDefaultAsync(u => u.Email == email);

        public async Task AddAsync(Utilisateur user)
        {
            await _ctx.Utilisateurs.AddAsync(user);
            await _ctx.SaveChangesAsync();
        }

        public Task SaveAsync() => _ctx.SaveChangesAsync();
    }
}
