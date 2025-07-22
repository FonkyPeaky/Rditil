using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rditil.Models;

namespace Rditil.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Utilisateur> Utilisateurs { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Reponse> Reponses { get; set; }
        public DbSet<Examen> Examens { get; set; }
        public DbSet<Examen_Question> ExamenQuestions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Examen_Question>()
                .HasOne(eq => eq.Examen)
                .WithMany(e => e.ExamenQuestions)
                .HasForeignKey(eq => eq.Id_Examen);

            modelBuilder.Entity<Examen_Question>()
                .HasOne(eq => eq.Question)
                .WithMany(q => q.ExamenQuestions)
                .HasForeignKey(eq => eq.Id_Question);
        }
    }

}
