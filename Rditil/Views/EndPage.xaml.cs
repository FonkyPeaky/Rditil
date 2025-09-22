using System.Windows.Controls;
using Rditil.ViewModels;

namespace Rditil.Views
{
    public partial class EndPage : Page
    {
        public EndPage(ResultViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
