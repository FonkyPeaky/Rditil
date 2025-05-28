using System.Windows;
using System.Windows.Input;

namespace Rditil.Views
{
    public partial class ExamenView : Window
    {
        public ExamenView()
        {
            InitializeComponent();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
} 