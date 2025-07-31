using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rditil.Services
{
    public interface INavigable
    {
        void OnNavigatedTo(Dictionary<string, object> parameters);
    }
}

