using Rditil.Data;
using Rditil.ViewModels;
using System.Windows.Controls;

namespace Rditil.Views
{
    public partial class AdminPanel : Page
    {
        private readonly AdminViewModel _viewModel;

        public AdminPanel()
        {
            InitializeComponent();
            var context = new AppDbContextFactory().CreateDbContext(Array.Empty<string>());
            _viewModel = new AdminViewModel(context);
            DataContext = _viewModel;
        }

        private void PasswordBox_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            _viewModel.NouvelUtilisateur.Password = ((PasswordBox)sender).Password;
        }
    }
}
