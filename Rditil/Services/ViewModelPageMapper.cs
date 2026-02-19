using System;
using System.Collections.Generic;
using Rditil.ViewModels;
using Rditil.Views;

namespace Rditil.Navigation
{
    public static class ViewModelPageMapper
    {
        private static readonly Dictionary<Type, Type> Map = new()
        {
            { typeof(LoginViewModel), typeof(LoginPage) },
            { typeof(WelcomeViewModel), typeof(WelcomePage) },
            { typeof(ExamViewModel), typeof(ExamenView) },
            { typeof(EndPageViewModel), typeof(EndPage) },
            { typeof(AdminPanelViewModel), typeof(AdminPanel) }
        };

        public static Type GetPageType(Type viewModelType) =>
            Map.TryGetValue(viewModelType, out var page)
                ? page
                : throw new InvalidOperationException(
                    $"Page non trouvée pour {viewModelType.Name}");
    }
}
