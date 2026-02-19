using Microsoft.EntityFrameworkCore;
using Rditil.Models;

namespace Rditil.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // ✅ DbSet utilisés par tes Services / VM
        public DbSet<Utilisateur> Utilisateurs => Set<Utilisateur>();
        public DbSet<Question> Questions => Set<Question>();
        public DbSet<Reponse> Reponses => Set<Reponse>();
        public DbSet<ExamAttempt> ExamAttempts => Set<ExamAttempt>();

        // Si ton code a DbExamResultService / Examen etc.
        public DbSet<Examen> Examens => Set<Examen>();

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    // --------------------------
        //    // Utilisateurs
        //    // --------------------------
        //    modelBuilder.Entity<Utilisateur>(e =>
        //    {
        //        e.ToTable("Utilisateurs");
        //        e.HasKey(x => x.Id_Utilisateur);

        //        // Si tes propriétés s’appellent pareil que les colonnes => pas obligatoire,
        //        // mais je préfère être explicite :
        //        e.Property(x => x.Id_Utilisateur).HasColumnName("Id_Utilisateur");
        //        e.Property(x => x.Email).HasColumnName("Email");
        //        e.Property(x => x.PasswordHash).HasColumnName("PasswordHash");
        //        e.Property(x => x.Nom).HasColumnName("Nom");
        //        e.Property(x => x.Prenom).HasColumnName("Prenom");
        //        e.Property(x => x.Score).HasColumnName("Score");
        //        e.Property(x => x.DernierExamen).HasColumnName("DernierExamen");
        //        e.Property(x => x.EmailNPlus1).HasColumnName("EmailNPlus1");
        //    });

        //    // --------------------------
        //    // Questions
        //    // --------------------------
        //    modelBuilder.Entity<Question>(e =>
        //    {
        //        e.ToTable("Questions");
        //        e.HasKey(x => x.Id_Question);

        //        e.Property(x => x.Id_Question).HasColumnName("Id_Question");

        //        // ✅ LA correction importante : Intitule (C#) => Enonce (DB)
        //        e.Property(x => x.Intitule).HasColumnName("Enonce");

        //        e.HasMany(x => x.Reponses)
        //         .WithOne(r => r.Question)
        //         .HasForeignKey(r => r.Id_Question);
        //    });

        //    // --------------------------
        //    // Reponses
        //    // --------------------------
        //    modelBuilder.Entity<Reponse>(e =>
        //    {
        //        e.ToTable("Reponses");
        //        e.HasKey(x => x.Id_Reponse);

        //        e.Property(x => x.Id_Reponse).HasColumnName("Id_Reponse");
        //        e.Property(x => x.TextReponse).HasColumnName("TextReponse");
        //        e.Property(x => x.EstCorrect).HasColumnName("EstCorrect");
        //        e.Property(x => x.Id_Question).HasColumnName("Id_Question");

        //        // Si tu as aussi une colonne "QuestionId_Question" en plus,
        //        // c’est souvent un “doublon” créé par EF.
        //        // On la laisse tranquille si elle existe, mais on n’en a pas besoin.
        //    });

        //    // --------------------------
        //    // Examens
        //    // --------------------------
        //    modelBuilder.Entity<Examen>(e =>
        //    {
        //        e.ToTable("Examens");
        //        // ⚠️ adapte le nom de la clé si ce n’est pas ça chez toi
        //        e.HasKey(x => x.Id_Examen);
        //    });

        //    // --------------------------
        //    // ExamAttemp
        //    // --------------------------
        //    modelBuilder.Entity<ExamAttempt>(e =>
        //    {
        //        e.ToTable("ExamAttempts");
        //        e.HasKey(x => x.Id_ExamAttempt);

        //        e.Property(x => x.Id_ExamAttempt).HasColumnName("Id_ExamAttempt");
        //        e.Property(x => x.Id_Utilisateur).HasColumnName("Id_Utilisateur");

        //        e.HasOne(x => x.Utilisateur)
        //         .WithMany()
        //         .HasForeignKey(x => x.Id_Utilisateur);
        //    });


        //}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Examen_Question>(e =>
            {
                e.ToTable("Exam_Question");

                // ✅ clé composite
                e.HasKey(x => new { x.Id_Exam, x.Id_Question });

                e.Property(x => x.Id_Exam).HasColumnName("Id_Exam");
                e.Property(x => x.Id_Question).HasColumnName("Id_Question");

                e.HasOne(x => x.Examen)
                 .WithMany(x => x.ExamenQuestions)
                 .HasForeignKey(x => x.Id_Exam);

                e.HasOne(x => x.Question)
                 .WithMany(x => x.ExamenQuestions)
                 .HasForeignKey(x => x.Id_Question);
            });
        }

    }
}
