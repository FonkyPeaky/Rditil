<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
using Rditil.ViewModels;
=======
﻿using Rditil.ViewModels;
>>>>>>> 1908e362463a42654d4b699764460fd7abed45f8
using Rditil.Views;

namespace Rditil.Navigation
{
    public static class ViewModelPageMapper
    {
        private static readonly Dictionary<Type, Type> Map = new()
        {
            { typeof(WelcomeViewModel), typeof(WelcomePage) },
            { typeof(ExamViewModel), typeof(ExamenView) },
            { typeof(EndPageViewModel), typeof(EndPage) },
            { typeof(AdminPanelViewModel), typeof(AdminPanel) },
            { typeof(ProgressViewModel), typeof(ProgressPage) }
        };

        public static Type GetPageType(Type viewModelType) =>
            Map.TryGetValue(viewModelType, out var page)
                ? page
                : throw new InvalidOperationException(
                    $"Page non trouvée pour {viewModelType.Name}");
    }
}
