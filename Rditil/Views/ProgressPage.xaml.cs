using Rditil.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Rditil.Views
{
    public partial class ProgressPage : Page
    {
        public ProgressPage(ProgressViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Lancer les animations d'entrée
            if (Resources["EnterStoryboard"] is Storyboard enter)
                enter.Begin();

            if (Resources["LeftPanelSlideIn"] is Storyboard left)
                left.Begin();

            if (Resources["RightPanelSlideIn"] is Storyboard right)
                right.Begin();

            if (Resources["OrbFloat1"] is Storyboard orb1)
                orb1.Begin();

            if (Resources["OrbFloat2"] is Storyboard orb2)
                orb2.Begin();
        }
    }
}
