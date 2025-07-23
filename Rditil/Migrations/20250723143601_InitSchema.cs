using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Rditil.Migrations
{
    /// <inheritdoc />
    public partial class InitSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id_Question = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Enonce = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id_Question);
                });

            migrationBuilder.CreateTable(
                name: "Utilisateurs",
                columns: table => new
                {
                    Id_Utilisateur = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nom = table.Column<string>(type: "text", nullable: false),
                    Prenom = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utilisateurs", x => x.Id_Utilisateur);
                });

            migrationBuilder.CreateTable(
                name: "Reponses",
                columns: table => new
                {
                    Id_Reponse = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TextReponse = table.Column<string>(type: "text", nullable: false),
                    EstCorrect = table.Column<bool>(type: "boolean", nullable: false),
                    Id_Question = table.Column<int>(type: "integer", nullable: false),
                    QuestionId_Question = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reponses", x => x.Id_Reponse);
                    table.ForeignKey(
                        name: "FK_Reponses_Questions_QuestionId_Question",
                        column: x => x.QuestionId_Question,
                        principalTable: "Questions",
                        principalColumn: "Id_Question",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Examens",
                columns: table => new
                {
                    Id_Examen = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DateExamen = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DureeExamen = table.Column<TimeSpan>(type: "interval", nullable: false),
                    SCORE = table.Column<int>(type: "integer", nullable: false),
                    Id_Utilisateur = table.Column<int>(type: "integer", nullable: false),
                    UtilisateurId_Utilisateur = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Examens", x => x.Id_Examen);
                    table.ForeignKey(
                        name: "FK_Examens_Utilisateurs_UtilisateurId_Utilisateur",
                        column: x => x.UtilisateurId_Utilisateur,
                        principalTable: "Utilisateurs",
                        principalColumn: "Id_Utilisateur",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExamenQuestions",
                columns: table => new
                {
                    Id_Examen = table.Column<int>(type: "integer", nullable: false),
                    Id_Question = table.Column<int>(type: "integer", nullable: false),
                    ExamenId_Examen = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamenQuestions", x => new { x.Id_Examen, x.Id_Question });
                    table.ForeignKey(
                        name: "FK_ExamenQuestions_Examens_ExamenId_Examen",
                        column: x => x.ExamenId_Examen,
                        principalTable: "Examens",
                        principalColumn: "Id_Examen");
                    table.ForeignKey(
                        name: "FK_ExamenQuestions_Examens_Id_Examen",
                        column: x => x.Id_Examen,
                        principalTable: "Examens",
                        principalColumn: "Id_Examen",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExamenQuestions_Questions_Id_Question",
                        column: x => x.Id_Question,
                        principalTable: "Questions",
                        principalColumn: "Id_Question",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExamenQuestions_ExamenId_Examen",
                table: "ExamenQuestions",
                column: "ExamenId_Examen");

            migrationBuilder.CreateIndex(
                name: "IX_ExamenQuestions_Id_Question",
                table: "ExamenQuestions",
                column: "Id_Question");

            migrationBuilder.CreateIndex(
                name: "IX_Examens_UtilisateurId_Utilisateur",
                table: "Examens",
                column: "UtilisateurId_Utilisateur");

            migrationBuilder.CreateIndex(
                name: "IX_Reponses_QuestionId_Question",
                table: "Reponses",
                column: "QuestionId_Question");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExamenQuestions");

            migrationBuilder.DropTable(
                name: "Reponses");

            migrationBuilder.DropTable(
                name: "Examens");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Utilisateurs");
        }
    }
}
