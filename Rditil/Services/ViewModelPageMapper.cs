using System;
using System.Collections.Generic;
using Rditil.Views;
using Rditil.ViewModels;

namespace Rditil.Navigation
{
    public static class ViewModelPageMapper
    {
        private static readonly Dictionary<Type, Type> Map = new()
        {
            { typeof(LoginViewModel), typeof(LoginPage) },
            { typeof(WelcomeViewModel), typeof(WelcomePage) },
            { typeof(AdminPanelViewModel), typeof(AdminPanel) },
            { typeof(ExamenViewModel), typeof(ExamenView) },
            { typeof(QuestionViewModel), typeof(QuestionPage) },
            { typeof(ResultViewModel), typeof(EndPage) },
        };

        public static Type GetPageType(Type viewModelType) =>
            Map.TryGetValue(viewModelType, out var page)
                ? page
                : throw new InvalidOperationException($"Page non trouvée pour {viewModelType.Name}");
    }
}
