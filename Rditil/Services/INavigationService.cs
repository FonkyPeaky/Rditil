using System.Collections.Generic;
using System.Windows.Controls;

namespace Rditil.Services
{
    public interface INavigationService
    {
        void SetFrame(Frame frame);

        void NavigateTo<TViewModel>(Dictionary<string, object>? parameters = null)
            where TViewModel : class;

    }
}
