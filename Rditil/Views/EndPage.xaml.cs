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
    }
}
