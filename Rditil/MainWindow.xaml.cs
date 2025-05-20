using Microsoft.EntityFrameworkCore;
using Rditil.Data;
using Rditil.Views;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Rditil
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var options = new DbContextOptionsBuilder<AppDbContext>().UseSqlite("Data Source=examen.db").Options;

            using var context = new AppDbContext(options); context.Database.Migrate(); // Applique les migrations (crée la BDD si elle n'existe pas)

            MainFrame.Navigate(new AdminPanel());
        }
    }
}