using Rditil.ViewModels;
using Rditil.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rditil.Services
{
    public static class ViewModelPageMapper
    {
        private static readonly Dictionary<Type, Type> Map = new()
        {
            { typeof(LoginViewModel), typeof(LoginPage) },
            { typeof(WelcomeViewModel), typeof(WelcomePage) },
        };

        public static Type GetPageTypeForViewModel(Type viewModelType)
        {
            return Map.TryGetValue(viewModelType, out var pageType)
                ? pageType
                : throw new ArgumentException($"No page found for {viewModelType.Name}");
        }
    }
}
