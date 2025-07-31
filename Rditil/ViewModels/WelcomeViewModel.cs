using Rditil.Models;
using Rditil.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Rditil.Services;
using Rditil.Views;
using Rditil.ViewModels;

namespace Rditil.ViewModels
{
    public class WelcomeViewModel : INotifyPropertyChanged, INavigable
    {
        private string _nomUtilisateur;
        public static object CurrentUser;
        public string NomUtilisateur
        {
            get => _nomUtilisateur;
            set
            {
                _nomUtilisateur = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        // Cette méthode est appelée automatiquement après la navigation
        public void OnNavigatedTo(Dictionary<string, object> parameters = null)
        {
            
            if (App.CurrentUser != null)
            {
                NomUtilisateur = App.CurrentUser.Prenom;
            }
            else
            {
                NomUtilisateur = "Utilisateur";
            }
        }
    }
}
