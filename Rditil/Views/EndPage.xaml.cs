using Rditil.ViewModels;
using System.Windows.Controls;

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
