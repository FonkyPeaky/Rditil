using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Rditil.Services
{
    public interface INavigationService
    {
        void Initialize(Frame frame);
        void NavigateTo<TViewModel>(Dictionary<string, object?>? parameters = null) where TViewModel : class;
        void NavigateTo(Type viewModelType, Dictionary<string, object?>? parameters = null);
    }
}
