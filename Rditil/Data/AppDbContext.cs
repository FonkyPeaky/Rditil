using Microsoft.EntityFrameworkCore;
using Rditil.Models;
using Rditil.Data.Entities;


namespace Rditil.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Utilisateur> Utilisateurs => Set<Utilisateur>();
    public DbSet<Examen> Examens => Set<Examen>();
    public DbSet<Question> Questions => Set<Question>();
    public DbSet<Reponse> Reponses => Set<Reponse>();
    public DbSet<ExamAttempt> ExamAttempts => Set<ExamAttempt>();
    public DbSet<ExamAnswer> ExamAnswers => Set<ExamAnswer>();
    public DbSet<SmtpSettingEntity> SmtpSettings => Set<SmtpSettingEntity>();




    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ===== Utilisateur =====
        modelBuilder.Entity<Utilisateur>(e =>
        {
            e.ToTable("utilisateurs");
            e.HasKey(x => x.Id_Utilisateur);
            e.Property(x => x.Id_Utilisateur).HasColumnName("id_utilisateur");
        });
        modelBuilder.Entity<SmtpSettingEntity>(e =>
        {
            e.ToTable("smtp_settings", "public");

            e.HasKey(x => x.Id);

            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.Host).HasColumnName("host");
            e.Property(x => x.Port).HasColumnName("port");
            e.Property(x => x.UseStartTls).HasColumnName("use_starttls");
            e.Property(x => x.Username).HasColumnName("username");
            e.Property(x => x.Password).HasColumnName("password");
            e.Property(x => x.FromEmail).HasColumnName("from_email");
            e.Property(x => x.Enabled).HasColumnName("enabled");
            e.Property(x => x.UpdatedAt).HasColumnName("updated_at");
        });


        // ===== Examen =====
        modelBuilder.Entity<Examen>(e =>
        {
            e.ToTable("examens");
            e.HasKey(x => x.Id_Examen);
            e.Property(x => x.Id_Examen).HasColumnName("id_examen");
            e.Property(x => x.DateExamen).HasColumnName("DateExamen");
            e.Property(x => x.DureeExamen).HasColumnName("DureeExamen");
            e.Property(x => x.Score).HasColumnName("Score");
            e.Property(x => x.Id_Utilisateur).HasColumnName("Id_Utilisateur");

            e.HasOne(x => x.Utilisateur)
             .WithMany()
             .HasForeignKey("UtilisateurId_Utilisateur")
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ===== Question =====
        modelBuilder.Entity<Question>(e =>
        {
            e.ToTable("questions");
            e.HasKey(q => q.Id);

            e.Property(q => q.Id).HasColumnName("id_question");
            e.Property(q => q.Intitule).HasColumnName("titre");
            e.Property(q => q.Enonce).HasColumnName("enonce");

            // 1..N Réponses
            e.HasMany(q => q.Reponses)
             .WithOne(r => r.Question)
             .HasForeignKey(r => r.Id_Question)
             .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Reponse>(entity =>
        {
            entity.ToTable("reponses");

            entity.HasKey(e => e.Id_Reponse).HasName("reponses_pkey");

            entity.Property(e => e.Id_Reponse).HasColumnName("id_reponse");
            entity.Property(e => e.Id_Question).HasColumnName("id_question");

            entity.Property(e => e.TextReponse).HasColumnName("TextReponse");
            entity.Property(e => e.EstCorrect).HasColumnName("EstCorrect");
        });


        // ===== ExamAttempt =====
        modelBuilder.Entity<ExamAttempt>(e =>
        {
            e.ToTable("exam_attempts");
            e.HasKey(a => a.Id_ExamAttempt);
            e.Property(a => a.Id_ExamAttempt)
             .HasColumnName("id_exam_attempt")
             .ValueGeneratedOnAdd();
            e.Property(a => a.Id_Utilisateur).HasColumnName("id_utilisateur");

            // User
            e.HasOne(a => a.Utilisateur)
             .WithMany()
             .HasForeignKey(a => a.Id_Utilisateur)
             .OnDelete(DeleteBehavior.Restrict);

            // Examen 
            e.HasOne(a => a.Examen)
             .WithMany()
             .HasForeignKey(a => a.ExamenId)
             .OnDelete(DeleteBehavior.SetNull);

            // Attempt 1..N Answers
            e.HasMany(a => a.Answers)
             .WithOne(x => x.ExamAttempt)
             .HasForeignKey(x => x.ExamAttemptId)
             .HasPrincipalKey(a => a.Id_ExamAttempt)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ===== ExamAnswer =====
        modelBuilder.Entity<ExamAnswer>(e =>
        {
            e.ToTable("exam_answers");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).ValueGeneratedOnAdd();

            e.Property(x => x.Id)
             .HasColumnName("Id")
             .ValueGeneratedOnAdd();

            e.HasOne(x => x.Question)
             .WithMany()
             .HasForeignKey(x => x.QuestionId)
             .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(x => x.SelectedReponse)
             .WithMany()
             .HasForeignKey(x => x.SelectedReponseId)
             .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
