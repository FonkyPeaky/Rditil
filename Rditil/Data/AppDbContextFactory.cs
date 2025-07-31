using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.EntityFrameworkCore.Design;

namespace Rditil.Data
{
    // db context factory pour la migration
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            // Configuration de la connexion à la base de données Postgre a remplacer par la nouvelle de randtad 
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=Test;Username=postgres;Password=Chacal");
            return new AppDbContext(optionsBuilder.Options); 
        }
    }
}


// Supprimer le dossier Migrations pour repartir de zéro
// dotnet ef migrations add InitialCreate
// dotnet ef database update pour appliquer la migration
// Assurez-vous d'avoir installé les packages NuGet nécessaires :

// Microsoft.EntityFrameworkCore.Design
// Npgsql.EntityFrameworkCore.PostgreSQL
// Npgsql.EntityFrameworkCore.PostgreSQL.Design

// Pour la configuration, tu peux utiliser un fichier appsettings.json ou une autre méthode de configuration
// pour charger la chaîne de connexion.
// Assurez-vous que la chaîne de connexion est correcte et que la base de données PostgreSQL est en cours d'exécution !!!!!