using Rditil.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Rditil.Views
{
    public partial class ExamenView : Page
    {
        private readonly ExamViewModel _vm;
        private bool _initialized;

        public ExamenView(ExamViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            DataContext = _vm;

            Loaded += ExamView_Loaded;
        }

        private void ExamView_Loaded(object sender, RoutedEventArgs e)
        {
            if (_initialized) return;
            _initialized = true;

            _vm.OnNavigatedTo();
        }
    }
}
