using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Rditil.Data;
using System.Windows;

namespace Rditil.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var configuration = ConfigHelper.LoadConfiguration();
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));

            using var context = new AppDbContext(optionsBuilder.Options);
            context.Database.Migrate(); // optionnel si tu veux migrer automatiquement

            var window = new LoginPage();
            window.Show();
        }
    }
}
