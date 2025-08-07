using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Rditil.Services;
using Rditil.ViewModels;
using Rditil.Views;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;


namespace Rditil.Services
{
    public class NavigationService : INavigationService
    {
        
        private Frame _mainFrame;
        private readonly IServiceProvider _serviceProvider;

        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public void SetFrame(Frame frame)
        {
            _mainFrame = frame;
        }

        public void NavigateTo<TViewModel>(Dictionary<string, object> parameters = null)
            where TViewModel : class
        {
            if (_mainFrame == null)
                throw new InvalidOperationException("Main frame has not been set.");

            var viewModel = _serviceProvider.GetService<TViewModel>();
            if (viewModel == null)
                throw new InvalidOperationException($"ViewModel {typeof(TViewModel).Name} is not registered in DI.");

            var pageType = ViewModelPageMapper.GetPageTypeForViewModel(typeof(TViewModel));
            var pageObj = _serviceProvider.GetService(pageType);

            if (pageObj is not Page page)
                throw new InvalidCastException($"The resolved type {pageType.Name} is not a Page.");

            page.DataContext = viewModel;
            _mainFrame.Navigate(page);
        }

    }
}

