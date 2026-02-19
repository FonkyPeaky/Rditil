<<<<<<< HEAD
﻿using System.Windows;
using System.Windows.Input;
using Rditil.Services;
=======
﻿using Rditil.Services;
using Rditil.ViewModels;
using System.Windows;
>>>>>>> 1908e362463a42654d4b699764460fd7abed45f8

namespace Rditil
{
    public partial class MainWindow : Window
    {
        public MainWindow(INavigationService navigation)
        {
            InitializeComponent();

            navigation.Initialize(MainFrame);

            ActivateFocusMode();
        }

        private void ActivateFocusMode()
        {
            WindowStyle = WindowStyle.None;
            ResizeMode = ResizeMode.NoResize;
            WindowState = WindowState.Maximized;
        }

        private void ExitFocusMode()
        {
            ResizeMode = ResizeMode.CanResize;
            WindowState = WindowState.Normal;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                ExitFocusMode();
                // Close();
            }
        }
    }
}
