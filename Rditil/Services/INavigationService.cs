<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
using System.Windows.Controls;
=======
﻿using System.Windows.Controls;
>>>>>>> 1908e362463a42654d4b699764460fd7abed45f8

namespace Rditil.Services
{
    public interface INavigationService
    {
        void Initialize(Frame frame);
        void NavigateTo<TViewModel>(Dictionary<string, object?>? parameters = null) where TViewModel : class;
        void NavigateTo(Type viewModelType, Dictionary<string, object?>? parameters = null);
    }
}
