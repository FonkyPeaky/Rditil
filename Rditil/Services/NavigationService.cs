using Microsoft.Extensions.DependencyInjection;
using Rditil.Navigation;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Controls;

namespace Rditil.Services
{
    public sealed class NavigationService : INavigationService
    {
        private readonly IServiceProvider _sp;
        private Frame? _frame;

        public NavigationService(IServiceProvider sp) => _sp = sp;

        public void Initialize(Frame frame) => _frame = frame;

        public void NavigateTo<TViewModel>(Dictionary<string, object?>? parameters = null) where TViewModel : class
            => NavigateTo(typeof(TViewModel), parameters);

        public void NavigateTo(Type viewModelType, Dictionary<string, object?>? parameters = null)
        {
            if (_frame == null)
                throw new InvalidOperationException("Frame non initialisée. Appelle Initialize(frame) au démarrage.");

            var pageType = ViewModelPageMapper.GetPageType(viewModelType);
            var page = (Page)ActivatorUtilities.CreateInstance(_sp, pageType);
            var vm = page.DataContext ?? _sp.GetRequiredService(viewModelType);

            if (parameters != null)
            {
                foreach (var (key, value) in parameters)
                {
                    var prop = viewModelType.GetProperty(key, BindingFlags.Public | BindingFlags.Instance);
                    if (prop != null && prop.CanWrite)
                        prop.SetValue(vm, value);
                }
            }

            if (vm is INavigable nav)
                nav.OnNavigatedTo(parameters);

            if (page.DataContext == null)
                page.DataContext = vm;
            _frame.Navigate(page);

        }
    }
}
