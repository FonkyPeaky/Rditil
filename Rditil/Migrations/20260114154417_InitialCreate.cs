using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Rditil.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "questions",
                columns: table => new
                {
                    id_question = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    titre = table.Column<string>(type: "text", nullable: false),
                    enonce = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_questions", x => x.id_question);
                });

            migrationBuilder.CreateTable(
                name: "utilisateurs",
                columns: table => new
                {
                    id_utilisateur = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nom = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Prenom = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    EmailNPlus1 = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Score = table.Column<int>(type: "integer", nullable: false),
                    DernierExamen = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_utilisateurs", x => x.id_utilisateur);
                });

            migrationBuilder.CreateTable(
                name: "reponses",
                columns: table => new
                {
                    id_reponse = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TextReponse = table.Column<string>(type: "text", nullable: true),
                    EstCorrect = table.Column<bool>(type: "boolean", nullable: false),
                    id_question = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reponses", x => x.id_reponse);
                    table.ForeignKey(
                        name: "FK_reponses_questions_id_question",
                        column: x => x.id_question,
                        principalTable: "questions",
                        principalColumn: "id_question",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "examens",
                columns: table => new
                {
                    id_examen = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DateExamen = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DureeExamen = table.Column<TimeSpan>(type: "interval", nullable: false),
                    Score = table.Column<int>(type: "integer", nullable: false),
                    Id_Utilisateur = table.Column<int>(type: "integer", nullable: false),
                    UtilisateurId_Utilisateur = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_examens", x => x.id_examen);
                    table.ForeignKey(
                        name: "FK_examens_utilisateurs_UtilisateurId_Utilisateur",
                        column: x => x.UtilisateurId_Utilisateur,
                        principalTable: "utilisateurs",
                        principalColumn: "id_utilisateur",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "exam_attempts",
                columns: table => new
                {
                    id_exam_attempt = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExamenId = table.Column<int>(type: "integer", nullable: false),
                    id_utilisateur = table.Column<int>(type: "integer", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FinishedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Score = table.Column<int>(type: "integer", nullable: false),
                    TotalQuestions = table.Column<int>(type: "integer", nullable: false),
                    UserFullName = table.Column<string>(type: "text", nullable: false),
                    ExamenId_Examen = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exam_attempts", x => x.id_exam_attempt);
                    table.ForeignKey(
                        name: "FK_exam_attempts_examens_ExamenId",
                        column: x => x.ExamenId,
                        principalTable: "examens",
                        principalColumn: "id_examen",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_exam_attempts_examens_ExamenId_Examen",
                        column: x => x.ExamenId_Examen,
                        principalTable: "examens",
                        principalColumn: "id_examen");
                    table.ForeignKey(
                        name: "FK_exam_attempts_utilisateurs_id_utilisateur",
                        column: x => x.id_utilisateur,
                        principalTable: "utilisateurs",
                        principalColumn: "id_utilisateur",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Examen_Question",
                columns: table => new
                {
                    Id_Exam = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Id_Question = table.Column<int>(type: "integer", nullable: false),
                    ExamenId_Examen = table.Column<int>(type: "integer", nullable: true),
                    QuestionId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Examen_Question", x => x.Id_Exam);
                    table.ForeignKey(
                        name: "FK_Examen_Question_examens_ExamenId_Examen",
                        column: x => x.ExamenId_Examen,
                        principalTable: "examens",
                        principalColumn: "id_examen");
                    table.ForeignKey(
                        name: "FK_Examen_Question_questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "questions",
                        principalColumn: "id_question");
                });

            migrationBuilder.CreateTable(
                name: "exam_answers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExamAttemptId = table.Column<int>(type: "integer", nullable: false),
                    QuestionId = table.Column<int>(type: "integer", nullable: false),
                    SelectedReponseId = table.Column<int>(type: "integer", nullable: true),
                    IsCorrect = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exam_answers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exam_answers_exam_attempts_ExamAttemptId",
                        column: x => x.ExamAttemptId,
                        principalTable: "exam_attempts",
                        principalColumn: "id_exam_attempt",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_exam_answers_questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "questions",
                        principalColumn: "id_question",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_exam_answers_reponses_SelectedReponseId",
                        column: x => x.SelectedReponseId,
                        principalTable: "reponses",
                        principalColumn: "id_reponse",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_exam_answers_ExamAttemptId",
                table: "exam_answers",
                column: "ExamAttemptId");

            migrationBuilder.CreateIndex(
                name: "IX_exam_answers_QuestionId",
                table: "exam_answers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_exam_answers_SelectedReponseId",
                table: "exam_answers",
                column: "SelectedReponseId");

            migrationBuilder.CreateIndex(
                name: "IX_exam_attempts_ExamenId",
                table: "exam_attempts",
                column: "ExamenId");

            migrationBuilder.CreateIndex(
                name: "IX_exam_attempts_ExamenId_Examen",
                table: "exam_attempts",
                column: "ExamenId_Examen");

            migrationBuilder.CreateIndex(
                name: "IX_exam_attempts_id_utilisateur",
                table: "exam_attempts",
                column: "id_utilisateur");

            migrationBuilder.CreateIndex(
                name: "IX_Examen_Question_ExamenId_Examen",
                table: "Examen_Question",
                column: "ExamenId_Examen");

            migrationBuilder.CreateIndex(
                name: "IX_Examen_Question_QuestionId",
                table: "Examen_Question",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_examens_UtilisateurId_Utilisateur",
                table: "examens",
                column: "UtilisateurId_Utilisateur");

            migrationBuilder.CreateIndex(
                name: "IX_reponses_id_question",
                table: "reponses",
                column: "id_question");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "exam_answers");

            migrationBuilder.DropTable(
                name: "Examen_Question");

            migrationBuilder.DropTable(
                name: "exam_attempts");

            migrationBuilder.DropTable(
                name: "reponses");

            migrationBuilder.DropTable(
                name: "examens");

            migrationBuilder.DropTable(
                name: "questions");

            migrationBuilder.DropTable(
                name: "utilisateurs");
        }
    }
}
