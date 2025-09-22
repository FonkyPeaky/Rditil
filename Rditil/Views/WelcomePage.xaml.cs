using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Rditil.Views
{
    public partial class WelcomePage : Page
    {
        public WelcomePage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (Resources["EnterStoryboard"] is Storyboard sb)
                sb.Begin();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            // Récup des infos depuis le VM si dispo
            string userEmail = null;
            string managerEmail = null;
            string userFullName = null;

            dynamic vm = DataContext; // permissif pour tes différents VM
            try
            {
                userEmail = vm?.Utilisateur?.Email ?? vm?.Email;
                managerEmail = vm?.Utilisateur?.ManagerEmail ?? vm?.ManagerEmail;
                userFullName = vm?.NomUtilisateur ??
                               ((vm?.Utilisateur?.Prenom + " " + vm?.Utilisateur?.Nom)?.Trim());
            }
            catch { }

            userEmail ??= "test@example.com";
            userFullName ??= "candidat";

            // Vers la page QCM
            NavigationService?.Navigate(new ExamenView(userEmail, managerEmail, userFullName));
        }
    }
}
