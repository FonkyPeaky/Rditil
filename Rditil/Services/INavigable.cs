using System.Collections.Generic;

namespace Rditil.Services
{
    public interface INavigable
    {
        void OnNavigatedTo(Dictionary<string, object?>? parameters);
    }
}

