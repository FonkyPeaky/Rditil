using Microsoft.EntityFrameworkCore;
using Rditil.Models;

namespace Rditil.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Utilisateur> Utilisateurs => Set<Utilisateur>();
        public DbSet<Question> Questions => Set<Question>();
        public DbSet<Reponse> Reponses => Set<Reponse>();
        public DbSet<Examen> Examens => Set<Examen>();
        public DbSet<Examen_Question> ExamenQuestions => Set<Examen_Question>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Clés
            modelBuilder.Entity<Question>().HasKey(q => q.Id_Question);
            modelBuilder.Entity<Reponse>().HasKey(r => r.Id_Reponse);

            // 1 seule relation claire Question (1) -> Reponses (n)
            modelBuilder.Entity<Reponse>()
                .HasOne(r => r.Question)
                .WithMany(q => q.Reponses)
                .HasForeignKey(r => r.Id_Question)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Si tu utilises la table de jointure Examen_Question
            modelBuilder.Entity<Examen_Question>()
                .HasKey(eq => new { eq.Id_Examen, eq.Id_Question });

            modelBuilder.Entity<Examen_Question>()
                .HasOne(eq => eq.Examen)
                .WithMany(e => e.ExamenQuestions)
                .HasForeignKey(eq => eq.Id_Examen)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Utilisateur>()
                .HasIndex(u => u.Email)
                .IsUnique();

        }
    }
}
