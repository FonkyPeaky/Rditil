using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rditil.Migrations
{
    /// <inheritdoc />
    public partial class SecurityAndManagerEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // --- Utilisateurs: colonnes de sécurité/manager ---

            // Ajoute PasswordHash si absent (non null, défaut = "")
            migrationBuilder.Sql(@"
        ALTER TABLE ""Utilisateurs""
        ADD COLUMN IF NOT EXISTS ""PasswordHash"" text NOT NULL DEFAULT '';
    ");

            // Copie éventuelle depuis 'Password' si elle existe ET ressemble déjà à du BCrypt
            // (bcrypt commence souvent par $2a$, $2b$, $2y$)
            migrationBuilder.Sql(@"
        DO $$
        BEGIN
            IF EXISTS (
                SELECT 1 FROM information_schema.columns
                WHERE table_name = 'Utilisateurs' AND column_name = 'Password'
            ) THEN
                UPDATE ""Utilisateurs""
                SET ""PasswordHash"" = ""Password""
                WHERE COALESCE(""PasswordHash"", '') = ''
                  AND ""Password"" ~ '^\$2[aby]\$';
            END IF;
        END
        $$;
    ");

            // Ajoute EmailNPlus1 si absent (nullable, 255)
            migrationBuilder.Sql(@"
        ALTER TABLE ""Utilisateurs""
        ADD COLUMN IF NOT EXISTS ""EmailNPlus1"" varchar(255) NULL;
    ");

            // Supprime l’ancienne colonne Password si elle existe
            migrationBuilder.Sql(@"
        ALTER TABLE ""Utilisateurs""
        DROP COLUMN IF EXISTS ""Password"";
    ");

            // --- Examens: SCORE -> Score (rename si 'SCORE' existe) ---
            migrationBuilder.Sql(@"
        DO $$
        BEGIN
            IF EXISTS (
                SELECT 1 FROM information_schema.columns
                WHERE table_name = 'Examens' AND column_name = 'SCORE'
            ) THEN
                EXECUTE 'ALTER TABLE ""Examens"" RENAME COLUMN ""SCORE"" TO ""Score""';
            END IF;
        END
        $$;
    ");

            // Optionnel: s’assure que Score existe (au cas où la colonne n’existait pas)
            migrationBuilder.Sql(@"
        DO $$
        BEGIN
            IF NOT EXISTS (
                SELECT 1 FROM information_schema.columns
                WHERE table_name = 'Examens' AND column_name = 'Score'
            ) THEN
                EXECUTE 'ALTER TABLE ""Examens"" ADD COLUMN ""Score"" integer NOT NULL DEFAULT 0';
            END IF;
        END
        $$;
    ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Recrée Password si manquant (texte, défaut = '')
            migrationBuilder.Sql(@"
        ALTER TABLE ""Utilisateurs""
        ADD COLUMN IF NOT EXISTS ""Password"" text NOT NULL DEFAULT '';
    ");

            // Récupère depuis PasswordHash si présent (attention: ça remet le hash tel quel)
            migrationBuilder.Sql(@"
        UPDATE ""Utilisateurs""
        SET ""Password"" = ""PasswordHash""
        WHERE ""PasswordHash"" IS NOT NULL AND ""PasswordHash"" <> '';
    ");

            // Supprime EmailNPlus1 si présent
            migrationBuilder.Sql(@"
        ALTER TABLE ""Utilisateurs""
        DROP COLUMN IF EXISTS ""EmailNPlus1"";
    ");

            // Supprime PasswordHash si présent
            migrationBuilder.Sql(@"
        ALTER TABLE ""Utilisateurs""
        DROP COLUMN IF EXISTS ""PasswordHash"";
    ");

            // Renomme Score -> SCORE si Score existe et SCORE pas encore là
            migrationBuilder.Sql(@"
        DO $$
        BEGIN
            IF EXISTS (
                SELECT 1 FROM information_schema.columns
                WHERE table_name = 'Examens' AND column_name = 'Score'
            )
            AND NOT EXISTS (
                SELECT 1 FROM information_schema.columns
                WHERE table_name = 'Examens' AND column_name = 'SCORE'
            ) THEN
                EXECUTE 'ALTER TABLE ""Examens"" RENAME COLUMN ""Score"" TO ""SCORE""';
            END IF;
        END
        $$;
    ");
        }

    }
}
