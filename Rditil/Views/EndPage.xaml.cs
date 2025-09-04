using System.Windows;
using Rditil.ViewModels;

namespace Rditil.Views
{
    /// <summary>
    /// Interaction logic for EndPage.xaml
    /// </summary>
    public partial class EndPage : Window
    {
        public EndPage(ResultViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
        private void Quitter_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Resources["EnterStoryboard"] is System.Windows.Media.Animation.Storyboard sb)
                sb.Begin();
        }


    }
}
