using Rditil.Models;
using Rditil.Views;
using System.Linq;
using System.Windows;

namespace Rditil.Views
{
    public partial class LoginPage : Window
    {
        public string Email { get; set; }
        public string ErrorMessage { get; set; }

        public LoginPage()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void OnLoginClick(object sender, RoutedEventArgs e)
        {
            string email = Email?.Trim();
            string password = PasswordBox.Password;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ErrorMessage = "Veuillez remplir tous les champs.";
                DataContext = null; DataContext = this;
                return;
            }

            var utilisateur = App.DbContext.Utilisateurs
                .FirstOrDefault(u => u.Email == email && u.Password == password);

            if (utilisateur == null)
            {
                ErrorMessage = "Email ou mot de passe incorrect.";
                DataContext = null; DataContext = this;
                return;
            }

            App.CurrentUser = utilisateur;

            // Redirection vers l'examen
            var examPage = new ExamenView(Email);
            examPage.Show();
            this.Close();
        }
    }
}
