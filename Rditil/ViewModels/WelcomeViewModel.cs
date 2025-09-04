using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Rditil.Models;

namespace Rditil.ViewModels
{
    public interface INavigable
    {
        void OnNavigatedTo(Dictionary<string, object>? parameters = null);
    }

    public class WelcomeViewModel : INotifyPropertyChanged, INavigable
    {
        private string _nomUtilisateur = "Utilisateur";
        public string NomUtilisateur
        {
            get => _nomUtilisateur;
            set { _nomUtilisateur = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public void OnNavigatedTo(Dictionary<string, object>? parameters = null)
        {
            // Récupère depuis App.CurrentUser (défini dans App.xaml.cs)
            var u = App.CurrentUser;
            NomUtilisateur = u?.Prenom ?? "Utilisateur";
        }
    }
}
