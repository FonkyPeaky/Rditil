using Microsoft.EntityFrameworkCore; // ✅ ajouté
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rditil.Data;
using Rditil.Models;
using Rditil.Services;
using Rditil.ViewModels;
using Rditil.Views;
using System.Windows;

namespace Rditil
{
    public partial class App : Application
    {
        public static IHost AppHost { get; private set; } = null!;

        // ✅ utilisateur courant accessible globalement
        public static Utilisateur? CurrentUser { get; set; }

        public App()
        {
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((ctx, cfg) =>
                {
                    cfg.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
#if DEBUG
                    cfg.AddUserSecrets<App>(optional: true);
#endif
                    cfg.AddEnvironmentVariables();
                })
                .ConfigureServices((ctx, services) =>
                {
                    // ✅ DbContext (PostgreSQL avec Npgsql)
                    services.AddDbContext<AppDbContext>(options =>
                        options.UseNpgsql(ctx.Configuration.GetConnectionString("DefaultConnection")));

                    // ✅ Services applicatifs
                    services.AddSingleton<INavigationService, NavigationService>();
                    services.AddSingleton<IEmailService, EmailService>();
                    services.Configure<SmtpSettings>(ctx.Configuration.GetSection("Smtp"));

                    // ✅ IUserService (ton service métier pour gérer les utilisateurs)
                    services.AddScoped<IUserService, UserService>();

                    // ✅ ViewModels
                    services.AddTransient<LoginViewModel>();
                    services.AddTransient<WelcomeViewModel>();
                    services.AddTransient<AdminPanelViewModel>();
                    services.AddTransient<ExamenViewModel>();
                    services.AddTransient<QuestionViewModel>();
                    services.AddTransient<ResultViewModel>();

                    // ✅ Pages
                    services.AddTransient<LoginPage>();
                    services.AddTransient<WelcomePage>();
                    services.AddTransient<AdminPanel>();
                    services.AddTransient<ExamenView>();
                    services.AddTransient<QuestionPage>();
                    services.AddTransient<EndPage>();

                    // ✅ Fenêtre principale
                    services.AddSingleton<MainWindow>();
                })
                .Build();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            AppHost.Start();

            var mainWindow = AppHost.Services.GetRequiredService<MainWindow>();
            Current.MainWindow = mainWindow;
            mainWindow.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            if (AppHost is not null)
                await AppHost.StopAsync();

            base.OnExit(e);
        }
    }
}
