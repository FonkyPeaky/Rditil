using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rditil.Migrations
{
    /// <inheritdoc />
    public partial class InitSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExamenQuestions_Examens_ExamenId",
                table: "ExamenQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_ExamenQuestions_Questions_QuestionId",
                table: "ExamenQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_ExamenQuestions_Reponses_ReponseChoisieId",
                table: "ExamenQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_Examens_Utilisateurs_UtilisateurId",
                table: "Examens");

            migrationBuilder.DropForeignKey(
                name: "FK_Reponses_Questions_QuestionId",
                table: "Reponses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExamenQuestions",
                table: "ExamenQuestions");

            migrationBuilder.DropIndex(
                name: "IX_ExamenQuestions_ExamenId",
                table: "ExamenQuestions");

            migrationBuilder.DeleteData(
                table: "Utilisateurs",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "IsAdmin",
                table: "Utilisateurs");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ExamenQuestions");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Utilisateurs",
                newName: "Id_Utilisateur");

            migrationBuilder.RenameColumn(
                name: "QuestionId",
                table: "Reponses",
                newName: "QuestionId_Question");

            migrationBuilder.RenameColumn(
                name: "IsSelected",
                table: "Reponses",
                newName: "Id_Question");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Reponses",
                newName: "Id_Reponse");

            migrationBuilder.RenameIndex(
                name: "IX_Reponses_QuestionId",
                table: "Reponses",
                newName: "IX_Reponses_QuestionId_Question");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Questions",
                newName: "Id_Question");

            migrationBuilder.RenameColumn(
                name: "Score",
                table: "Examens",
                newName: "SCORE");

            migrationBuilder.RenameColumn(
                name: "UtilisateurId",
                table: "Examens",
                newName: "UtilisateurId_Utilisateur");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Examens",
                newName: "Id_Examen");

            migrationBuilder.RenameIndex(
                name: "IX_Examens_UtilisateurId",
                table: "Examens",
                newName: "IX_Examens_UtilisateurId_Utilisateur");

            migrationBuilder.RenameColumn(
                name: "ReponseChoisieId",
                table: "ExamenQuestions",
                newName: "ExamenId_Examen");

            migrationBuilder.RenameColumn(
                name: "QuestionId",
                table: "ExamenQuestions",
                newName: "Id_Question");

            migrationBuilder.RenameColumn(
                name: "ExamenId",
                table: "ExamenQuestions",
                newName: "Id_Examen");

            migrationBuilder.RenameIndex(
                name: "IX_ExamenQuestions_ReponseChoisieId",
                table: "ExamenQuestions",
                newName: "IX_ExamenQuestions_ExamenId_Examen");

            migrationBuilder.RenameIndex(
                name: "IX_ExamenQuestions_QuestionId",
                table: "ExamenQuestions",
                newName: "IX_ExamenQuestions_Id_Question");

            migrationBuilder.AlterColumn<string>(
                name: "TextReponse",
                table: "Reponses",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "DureeExamen",
                table: "Examens",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<int>(
                name: "Id_Utilisateur",
                table: "Examens",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExamenQuestions",
                table: "ExamenQuestions",
                columns: new[] { "Id_Examen", "Id_Question" });

            migrationBuilder.AddForeignKey(
                name: "FK_ExamenQuestions_Examens_ExamenId_Examen",
                table: "ExamenQuestions",
                column: "ExamenId_Examen",
                principalTable: "Examens",
                principalColumn: "Id_Examen");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamenQuestions_Examens_Id_Examen",
                table: "ExamenQuestions",
                column: "Id_Examen",
                principalTable: "Examens",
                principalColumn: "Id_Examen",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExamenQuestions_Questions_Id_Question",
                table: "ExamenQuestions",
                column: "Id_Question",
                principalTable: "Questions",
                principalColumn: "Id_Question",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Examens_Utilisateurs_UtilisateurId_Utilisateur",
                table: "Examens",
                column: "UtilisateurId_Utilisateur",
                principalTable: "Utilisateurs",
                principalColumn: "Id_Utilisateur",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reponses_Questions_QuestionId_Question",
                table: "Reponses",
                column: "QuestionId_Question",
                principalTable: "Questions",
                principalColumn: "Id_Question",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExamenQuestions_Examens_ExamenId_Examen",
                table: "ExamenQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_ExamenQuestions_Examens_Id_Examen",
                table: "ExamenQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_ExamenQuestions_Questions_Id_Question",
                table: "ExamenQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_Examens_Utilisateurs_UtilisateurId_Utilisateur",
                table: "Examens");

            migrationBuilder.DropForeignKey(
                name: "FK_Reponses_Questions_QuestionId_Question",
                table: "Reponses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExamenQuestions",
                table: "ExamenQuestions");

            migrationBuilder.DropColumn(
                name: "Id_Utilisateur",
                table: "Examens");

            migrationBuilder.RenameColumn(
                name: "Id_Utilisateur",
                table: "Utilisateurs",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "QuestionId_Question",
                table: "Reponses",
                newName: "QuestionId");

            migrationBuilder.RenameColumn(
                name: "Id_Question",
                table: "Reponses",
                newName: "IsSelected");

            migrationBuilder.RenameColumn(
                name: "Id_Reponse",
                table: "Reponses",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Reponses_QuestionId_Question",
                table: "Reponses",
                newName: "IX_Reponses_QuestionId");

            migrationBuilder.RenameColumn(
                name: "Id_Question",
                table: "Questions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "SCORE",
                table: "Examens",
                newName: "Score");

            migrationBuilder.RenameColumn(
                name: "UtilisateurId_Utilisateur",
                table: "Examens",
                newName: "UtilisateurId");

            migrationBuilder.RenameColumn(
                name: "Id_Examen",
                table: "Examens",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Examens_UtilisateurId_Utilisateur",
                table: "Examens",
                newName: "IX_Examens_UtilisateurId");

            migrationBuilder.RenameColumn(
                name: "ExamenId_Examen",
                table: "ExamenQuestions",
                newName: "ReponseChoisieId");

            migrationBuilder.RenameColumn(
                name: "Id_Question",
                table: "ExamenQuestions",
                newName: "QuestionId");

            migrationBuilder.RenameColumn(
                name: "Id_Examen",
                table: "ExamenQuestions",
                newName: "ExamenId");

            migrationBuilder.RenameIndex(
                name: "IX_ExamenQuestions_Id_Question",
                table: "ExamenQuestions",
                newName: "IX_ExamenQuestions_QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_ExamenQuestions_ExamenId_Examen",
                table: "ExamenQuestions",
                newName: "IX_ExamenQuestions_ReponseChoisieId");

            migrationBuilder.AddColumn<bool>(
                name: "IsAdmin",
                table: "Utilisateurs",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "TextReponse",
                table: "Reponses",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "DureeExamen",
                table: "Examens",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "TEXT");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ExamenQuestions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExamenQuestions",
                table: "ExamenQuestions",
                column: "Id");

            migrationBuilder.InsertData(
                table: "Utilisateurs",
                columns: new[] { "Id", "Email", "IsAdmin", "Nom", "Password", "Prenom" },
                values: new object[] { 1, "admin@examen.com", true, "Admin", "pipicaca", "Super" });

            migrationBuilder.CreateIndex(
                name: "IX_ExamenQuestions_ExamenId",
                table: "ExamenQuestions",
                column: "ExamenId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamenQuestions_Examens_ExamenId",
                table: "ExamenQuestions",
                column: "ExamenId",
                principalTable: "Examens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExamenQuestions_Questions_QuestionId",
                table: "ExamenQuestions",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExamenQuestions_Reponses_ReponseChoisieId",
                table: "ExamenQuestions",
                column: "ReponseChoisieId",
                principalTable: "Reponses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Examens_Utilisateurs_UtilisateurId",
                table: "Examens",
                column: "UtilisateurId",
                principalTable: "Utilisateurs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reponses_Questions_QuestionId",
                table: "Reponses",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
