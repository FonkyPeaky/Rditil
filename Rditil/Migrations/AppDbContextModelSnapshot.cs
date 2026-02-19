using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Rditil.Data;

#nullable disable

namespace Rditil.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Rditil.Models.ExamAnswer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ExamAttemptId")
                        .HasColumnType("integer");

                    b.Property<bool>("IsCorrect")
                        .HasColumnType("boolean");

                    b.Property<int>("QuestionId")
                        .HasColumnType("integer");

                    b.Property<int?>("SelectedReponseId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ExamAttemptId");

                    b.HasIndex("QuestionId");

                    b.HasIndex("SelectedReponseId");

                    b.ToTable("exam_answers", (string)null);
                });

            modelBuilder.Entity("Rditil.Models.ExamAttempt", b =>
                {
                    b.Property<int>("Id_ExamAttempt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id_exam_attempt");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id_ExamAttempt"));

                    b.Property<int>("ExamenId")
                        .HasColumnType("integer");

                    b.Property<int?>("ExamenId_Examen")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("FinishedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Id_Utilisateur")
                        .HasColumnType("integer")
                        .HasColumnName("id_utilisateur");

                    b.Property<int>("Score")
                        .HasColumnType("integer");

                    b.Property<DateTime>("StartedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("TotalQuestions")
                        .HasColumnType("integer");

                    b.Property<string>("UserFullName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id_ExamAttempt");

                    b.HasIndex("ExamenId");

                    b.HasIndex("ExamenId_Examen");

                    b.HasIndex("Id_Utilisateur");

                    b.ToTable("exam_attempts", (string)null);
                });

            modelBuilder.Entity("Rditil.Models.Examen", b =>
                {
                    b.Property<int>("Id_Examen")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id_examen");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id_Examen"));

                    b.Property<DateTime>("DateExamen")
                        .HasColumnType("timestamp with time zone");

                    b.Property<TimeSpan>("DureeExamen")
                        .HasColumnType("interval");

                    b.Property<int>("Id_Utilisateur")
                        .HasColumnType("integer");

                    b.Property<int>("Score")
                        .HasColumnType("integer");

                    b.Property<int>("UtilisateurId_Utilisateur")
                        .HasColumnType("integer");

                    b.HasKey("Id_Examen");

                    b.HasIndex("UtilisateurId_Utilisateur");

                    b.ToTable("examens", (string)null);
                });

            modelBuilder.Entity("Rditil.Models.Examen_Question", b =>
                {
                    b.Property<int>("Id_Exam")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id_Exam"));

                    b.Property<int?>("ExamenId_Examen")
                        .HasColumnType("integer");

                    b.Property<int>("Id_Question")
                        .HasColumnType("integer");

                    b.Property<int?>("QuestionId")
                        .HasColumnType("integer");

                    b.HasKey("Id_Exam");

                    b.HasIndex("ExamenId_Examen");

                    b.HasIndex("QuestionId");

                    b.ToTable("Examen_Question");
                });

            modelBuilder.Entity("Rditil.Models.Question", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id_question");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Enonce")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("enonce");

                    b.Property<string>("Intitule")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("titre");

                    b.HasKey("Id");

                    b.ToTable("questions", (string)null);
                });

            modelBuilder.Entity("Rditil.Models.Reponse", b =>
                {
                    b.Property<int>("Id_Reponse")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id_reponse");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id_Reponse"));

                    b.Property<bool>("EstCorrect")
                        .HasColumnType("boolean");

                    b.Property<int>("Id_Question")
                        .HasColumnType("integer")
                        .HasColumnName("id_question");

                    b.Property<string>("TextReponse")
                        .HasColumnType("text");

                    b.HasKey("Id_Reponse");

                    b.HasIndex("Id_Question");

                    b.ToTable("reponses", (string)null);
                });

            modelBuilder.Entity("Rditil.Models.Utilisateur", b =>
                {
                    b.Property<int>("Id_Utilisateur")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id_utilisateur");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id_Utilisateur"));

                    b.Property<DateTime>("DernierExamen")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("EmailNPlus1")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Prenom")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<int>("Score")
                        .HasColumnType("integer");

                    b.HasKey("Id_Utilisateur");

                    b.ToTable("utilisateurs", (string)null);
                });

            modelBuilder.Entity("Rditil.Models.ExamAnswer", b =>
                {
                    b.HasOne("Rditil.Models.ExamAttempt", "ExamAttempt")
                        .WithMany("Answers")
                        .HasForeignKey("ExamAttemptId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Rditil.Models.Question", "Question")
                        .WithMany()
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Rditil.Models.Reponse", "SelectedReponse")
                        .WithMany()
                        .HasForeignKey("SelectedReponseId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("ExamAttempt");

                    b.Navigation("Question");

                    b.Navigation("SelectedReponse");
                });

            modelBuilder.Entity("Rditil.Models.ExamAttempt", b =>
                {
                    b.HasOne("Rditil.Models.Examen", "Examen")
                        .WithMany()
                        .HasForeignKey("ExamenId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.HasOne("Rditil.Models.Examen", null)
                        .WithMany("Attempts")
                        .HasForeignKey("ExamenId_Examen");

                    b.HasOne("Rditil.Models.Utilisateur", "Utilisateur")
                        .WithMany()
                        .HasForeignKey("Id_Utilisateur")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Examen");

                    b.Navigation("Utilisateur");
                });

            modelBuilder.Entity("Rditil.Models.Examen", b =>
                {
                    b.HasOne("Rditil.Models.Utilisateur", "Utilisateur")
                        .WithMany()
                        .HasForeignKey("UtilisateurId_Utilisateur")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Utilisateur");
                });

            modelBuilder.Entity("Rditil.Models.Examen_Question", b =>
                {
                    b.HasOne("Rditil.Models.Examen", "Examen")
                        .WithMany("ExamenQuestions")
                        .HasForeignKey("ExamenId_Examen");

                    b.HasOne("Rditil.Models.Question", "Question")
                        .WithMany()
                        .HasForeignKey("QuestionId");

                    b.Navigation("Examen");

                    b.Navigation("Question");
                });

            modelBuilder.Entity("Rditil.Models.Reponse", b =>
                {
                    b.HasOne("Rditil.Models.Question", "Question")
                        .WithMany("Reponses")
                        .HasForeignKey("Id_Question")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Question");
                });

            modelBuilder.Entity("Rditil.Models.ExamAttempt", b =>
                {
                    b.Navigation("Answers");
                });

            modelBuilder.Entity("Rditil.Models.Examen", b =>
                {
                    b.Navigation("Attempts");

                    b.Navigation("ExamenQuestions");
                });

            modelBuilder.Entity("Rditil.Models.Question", b =>
                {
                    b.Navigation("Reponses");
                });
#pragma warning restore 612, 618
        }
    }
}
