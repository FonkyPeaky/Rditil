using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rditil.Data;
using Rditil.Services;
using Rditil.ViewModels;
using Rditil.Views;
using System.Windows;

namespace Rditil
{
    public partial class App : Application
    {
        public static IHost AppHost { get; private set; } = null!;

        public App()
        {
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    var config = context.Configuration;

                    services.AddDbContextFactory<AppDbContext>(options =>
                        options.UseNpgsql(config.GetConnectionString("DefaultConnection")));

                    // Services
                    services.AddSingleton<INavigationService, NavigationService>();
                    services.AddSingleton<IAppState, AppState>();
                    services.AddSingleton<IUserService, UserService>();
                    services.AddSingleton<IAuditLogger, FileAuditLogger>();

                    services.Configure<SmtpSettings>(context.Configuration.GetSection("Smtp"));
                    services.AddSingleton<IEmailService, EmailService>();

                    // ViewModels
                    services.AddTransient<LoginViewModel>();
                    services.AddTransient<WelcomeViewModel>();
                    services.AddTransient<ExamViewModel>();
                    services.AddTransient<EndPageViewModel>();
                    services.AddTransient<AdminPanelViewModel>();
                    services.AddTransient<ProgressViewModel>();

                    // Pages
                    services.AddTransient<WelcomePage>();
                    services.AddTransient<ExamenView>();
                    services.AddTransient<EndPage>();
                    services.AddTransient<AdminPanel>();
                    services.AddTransient<ProgressPage>();

                    // Windows
                    services.AddTransient<LoginWindow>();
                    services.AddTransient<AdminPanelWindow>();
                    services.AddSingleton<MainWindow>();


                })
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ShutdownMode = ShutdownMode.OnExplicitShutdown;

            await AppHost.StartAsync();

            var loginWindow = AppHost.Services.GetRequiredService<LoginWindow>();
            var ok = loginWindow.ShowDialog();

            if (ok != true)
            {
                Shutdown();
                return;
            }

            var mainWindow = AppHost.Services.GetRequiredService<MainWindow>();
            Current.MainWindow = mainWindow;
            mainWindow.Show();
            mainWindow.Activate();

            var nav = AppHost.Services.GetRequiredService<INavigationService>();
            nav.NavigateTo<WelcomeViewModel>();

            ShutdownMode = ShutdownMode.OnMainWindowClose;
        }

    }
}
