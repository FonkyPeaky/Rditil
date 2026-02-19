<<<<<<< HEAD
﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
=======
﻿using Rditil.ViewModels;
using System.Windows.Controls;
>>>>>>> 1908e362463a42654d4b699764460fd7abed45f8

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
