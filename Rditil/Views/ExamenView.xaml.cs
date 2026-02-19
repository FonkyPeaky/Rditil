<<<<<<< HEAD
﻿using Rditil.ViewModels;
using System.Windows;
using System.Windows.Controls;
=======
﻿using Microsoft.Extensions.DependencyInjection;
using Rditil.Data;
using Rditil.Services;
using Rditil.ViewModels;
using System.Windows;
>>>>>>> 1908e362463a42654d4b699764460fd7abed45f8

namespace Rditil.Views
{
    public partial class ExamenView : Page
    {
        public ExamenView(ExamViewModel vm)
        {
            InitializeComponent();
            System.Diagnostics.Debug.WriteLine($"[ExamenView] Constructor - VM instance: {vm.GetHashCode()}");
            DataContext = vm;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"[ExamenView] Page_Loaded - DataContext: {DataContext?.GetType().Name}");
            if (DataContext is ExamViewModel examVm)
            {
                System.Diagnostics.Debug.WriteLine($"[ExamenView] Calling EnsureLoadedAsync manually");
                _ = examVm.EnsureLoadedAsync();
            }
        }
    }
}
