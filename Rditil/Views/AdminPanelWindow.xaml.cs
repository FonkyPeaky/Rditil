using System.Windows;
using Rditil.ViewModels;

namespace Rditil.Views
{
    public partial class AdminPanelWindow : Window
    {
        public AdminPanelWindow(AdminPanelViewModel vm)
        {
            InitializeComponent();

            var page = new AdminPanel(vm);
            MainFrame.Navigate(page);


            MainFrame.Navigate(page);
        }
    }

}
