using Microsoft.Extensions.DependencyInjection;
using Rditil.Navigation;
using Rditil.ViewModels;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Controls;

namespace Rditil.Services
{
    public class NavigationService : INavigationService
    {
        private readonly IServiceProvider _sp;
        private Frame? _frame;

        public NavigationService(IServiceProvider sp)
        {
            _sp = sp;
        }

        public void SetFrame(Frame frame)
        {
            _frame = frame;
        }

        public void NavigateTo<TViewModel>(Dictionary<string, object?>? parameters = null)
            where TViewModel : class
        {
            NavigateTo(typeof(TViewModel), parameters);
        }

        private void NavigateTo(Type viewModelType, Dictionary<string, object?>? parameters)
        {
            if (_frame == null)
                throw new InvalidOperationException("Frame non initialisée");

            // 1️⃣ Page
            var pageType = ViewModelPageMapper.GetPageType(viewModelType);
            var page = (Page)ActivatorUtilities.CreateInstance(_sp, pageType);

            // 2️⃣ ViewModel
            var vm = _sp.GetRequiredService(viewModelType);

            // 3️⃣ Property bag
            if (parameters != null)
            {
                foreach (var (key, value) in parameters)
                {
                    var prop = viewModelType.GetProperty(key,
                        BindingFlags.Public | BindingFlags.Instance);

                    prop?.SetValue(vm, value);
                }
            }

            // 4️⃣ Hook navigation (examen)
            if (vm is ExamViewModel examVm)
            {
                examVm.OnNavigatedTo();
            }

            page.DataContext = vm;
            _frame.Navigate(page);
        }
    }
}
