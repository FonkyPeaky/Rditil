using System.Windows;
using Rditil.Services;
using Rditil.ViewModels;

namespace Rditil
{
    public partial class MainWindow : Window
    {
        private readonly INavigationService _navigation;

        public MainWindow(INavigationService navigation)
        {
            InitializeComponent();
            _navigation = navigation;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _navigation.SetFrame(MainFrame);
            _navigation.NavigateTo<LoginViewModel>(null);
        }
    }
}
