using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rditil.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Utilisateurs",
                columns: new[] { "Id", "Email", "IsAdmin", "Nom", "Password", "Prenom" },
                values: new object[] { 1, "admin@examen.com", true, "Admin", "pipicaca", "Super" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Utilisateurs",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
