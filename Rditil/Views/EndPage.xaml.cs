using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Rditil.Views
{
    public partial class EndPage : Page
    {
        public EndPage()
        {
            InitializeComponent();
        }

        private void Card_Loaded(object sender, RoutedEventArgs e)
        {
            if (FindResource("FadeSlideIn") is Storyboard sb)
            {
                sb.Begin((FrameworkElement)sender);
            }
        }
    }
}
