using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rditil.Models;

namespace Rditil.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Utilisateur> Utilisateurs { get; set; }
        public DbSet<Examen> Examens { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Reponse> Reponses { get; set; }
        public DbSet<ExamenQuestion> ExamenQuestions { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuration de la clé composite si nécessaire
            modelBuilder.Entity<ExamenQuestion>()
                .HasOne(eq => eq.Examen)
                .WithMany(e => e.ExamenQuestions)
                .HasForeignKey(eq => eq.ExamenId);

            modelBuilder.Entity<ExamenQuestion>()
                .HasOne(eq => eq.Question)
                .WithMany(q => q.ExamenQuestions)
                .HasForeignKey(eq => eq.QuestionId);

            modelBuilder.Entity<Reponse>()
                .HasOne(r => r.Question)
                .WithMany(q => q.Reponses)
                .HasForeignKey(r => r.QuestionId);

            modelBuilder.Entity<Utilisateur>().HasData(
            new Utilisateur
                {
                    Id = 1,
                    Nom = "Admin",
                    Prenom = "Super",
                    Email = "admin@examen.com",
                    Password = "pipicaca", // À sécuriser plus tard !
                    IsAdmin = true
                }
            );

        }



    }
}

