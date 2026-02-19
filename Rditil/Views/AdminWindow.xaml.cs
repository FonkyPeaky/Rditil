using System;
using System.Windows;
using Rditil.Services;

namespace Rditil.Views
{
    public partial class AdminWindow : Window
    {
        private readonly IServiceProvider _sp;

        public AdminWindow(IServiceProvider sp)
        {
            InitializeComponent();
            _sp = sp;
        }

        public void NavigateToAdminPanel()
        {
            var page = (AdminPanel)_sp.GetService(typeof(AdminPanel))!;
            RootFrame.Navigate(page);
        }
    }
}
