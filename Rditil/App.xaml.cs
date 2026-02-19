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
using static System.Formats.Asn1.AsnWriter;

namespace Rditil
{
    public partial class App : Application
    {
        public static IHost AppHost { get; private set; } = null!;
        public IServiceProvider Services => AppHost.Services;   // raccourci pratique


        // ✅ utilisateur courant accessible globalement
        public static Utilisateur? CurrentUser { get; set; }

        // ✅ helpers pour la page de fin / reporting email
        public static string CurrentUserFullName => CurrentUser?.Nom ?? string.Empty;
        public static string ManagerEmail => CurrentUser?.EmailNPlus1 ?? string.Empty;

        // Résumé du dernier examen (utilisé par EndPage)
        public static Models.ExamResultSummary? LastExamResult { get; set; }

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
                    services.AddTransient<ExamViewModel>();
                    services.AddTransient<QuestionViewModel>();
                    services.AddTransient<ResultViewModel>();
                    services.AddTransient<EndPageViewModel>();


                    // ✅ Pages
                    services.AddTransient<LoginPage>();
                    services.AddTransient<WelcomePage>();
                    services.AddTransient<AdminPanel>();
                    services.AddTransient<ExamenView>();
                    services.AddTransient<QuestionPage>();
                    services.AddTransient<EndPage>();

                    // ✅ Fenêtre principale
                    services.AddSingleton(sp =>
                    {
                        var cfg = sp.GetRequiredService<IConfiguration>();
                        return cfg.GetSection("Smtp").Get<SmtpSettings>() ?? new SmtpSettings();
                    });
                })
                .Build();


        }


        protected override void OnStartup(StartupEventArgs e)
        {
            var cfg = AppHost.Services.GetRequiredService<IConfiguration>();
            var cs = cfg.GetConnectionString("DefaultConnection") ?? "(null)";
            System.Diagnostics.Debug.WriteLine("CS USED: " + cs.Replace("Password=", "Password=***"));

            AppHost.Start();

            var mainWindow = AppHost.Services.GetRequiredService<MainWindow>();
            Current.MainWindow = mainWindow;
            mainWindow.Show();

            base.OnStartup(e);

            using (var scope = AppHost.Services.CreateScope())
            {
                var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                ctx.Database.EnsureCreated(); // ou ctx.Database.Migrate();
                //DbSeeder.Seed(ctx);
            }


        }

        protected override async void OnExit(ExitEventArgs e)
        {
            if (AppHost is not null)
                await AppHost.StopAsync();

            base.OnExit(e);
        }
    }
}
