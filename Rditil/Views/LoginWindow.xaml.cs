using System.Windows;
using System.Windows.Media.Animation;

namespace Rditil.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Card_Loaded(object sender, RoutedEventArgs e)
        {
            var sb = (Storyboard)FindResource("FadeSlideIn");
            sb.Begin((FrameworkElement)sender);
        }
    }
}
