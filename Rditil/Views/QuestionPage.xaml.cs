using Rditil.Data;
using Rditil.ViewModels;
using System.Windows.Controls;

namespace Rditil.Views
{
    public partial class QuestionPage : Page
    {
        public QuestionPage()
        {
            InitializeComponent();
            var context = new AppDbContextFactory().CreateDbContext(Array.Empty<string>());
            DataContext = new ExamViewModel(context);
        }
    }
}
