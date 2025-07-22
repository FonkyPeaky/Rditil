using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Rditil.Data;
using Rditil.Models;
using System;
using System.IO;
using System.Windows;

namespace Rditil
{
    public partial class App : Application
    {
        public static AppDbContext DbContext { get; private set; }
        public static Utilisateur CurrentUser { get; set; }


        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Charger la configuration depuis appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Configurer le DbContext PostgreSQL
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));

            DbContext = new AppDbContext(optionsBuilder.Options);

            // Initialiser la base de données (facultatif : migrations ou EnsureCreated)
            DbContext.Database.EnsureCreated(); // ou .Migrate()

            // Lancer la première fenêtre (ex: LoginPage)
            var mainWindow = new Views.LoginPage(); // ou MainWindow, selon ta logique
            mainWindow.Show();
        }
    }
}
