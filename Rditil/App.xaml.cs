using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rditil;
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
        public static IHost AppHost { get; private set; }

        // 🔧 AJOUTER CETTE PROPRIÉTÉ :
        public static Utilisateur CurrentUser { get; set; }

        public App()
        {
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddDbContext<AppDbContext>(options =>
                        options.UseNpgsql(ConfigHelper.LoadConfiguration().GetConnectionString("DefaultConnection")));

                    services.AddSingleton<MainWindow>();
                    services.AddTransient<LoginPage>();
                    services.AddTransient<LoginViewModel>();
                    services.AddTransient<WelcomePage>();
                    services.AddTransient<WelcomeViewModel>();
                    services.AddSingleton<INavigationService, NavigationService>();
                })
                .Build();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            AppHost.Start();
            var mainWindow = AppHost.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
            base.OnStartup(e);
        }
    }

}
