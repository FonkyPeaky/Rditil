using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using Rditil.Navigation; // pour ViewModelPageMapper

namespace Rditil.Services
{

    public sealed class NavigationService : INavigationService
    {
        private readonly IServiceProvider _sp;
        private Frame? _frame;

        public NavigationService(IServiceProvider sp)
        {
            _sp = sp;
        }

        public void SetFrame(Frame frame) => _frame = frame;

        // Implémente la signature attendue par TON interface
        public void NavigateTo<TViewModel>(Dictionary<string, object?>? parameters = null) where TViewModel : class
        {
            NavigateTo(typeof(TViewModel), parameters);
        }


        // Surcharge interne pratique (ta/ton interface n'a pas besoin de la déclarer)
        private void NavigateTo(Type viewModelType, Dictionary<string, object?>? parameters = null)
        {
            if (_frame is null)
                throw new InvalidOperationException("Frame non initialisée. Appelle SetFrame() avant NavigateTo().");

            // 1) Trouver la Page associée à la VM
            var pageType = ViewModelPageMapper.GetPageType(viewModelType);

            // 2) Résoudre la VM via DI
            var vm = _sp.GetRequiredService(viewModelType);

            // 3) Appliquer les 'parameters' sur la VM si fournis (property bag)
            if (parameters is not null)
            {
                foreach (var (key, value) in parameters)
                {
                    var prop = viewModelType.GetProperty(key, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
                    if (prop is { CanWrite: true })
                    {
                        prop.SetValue(vm, value);
                    }
                }
            }

            // 4) Créer la Page et fixer DataContext
            var page = (Page?)ActivatorUtilities.CreateInstance(_sp, pageType);
            if (page is null) throw new InvalidOperationException($"Impossible d'instancier {pageType.Name}");

            page.DataContext = vm;
            _frame.Navigate(page);
        }
    }
}
