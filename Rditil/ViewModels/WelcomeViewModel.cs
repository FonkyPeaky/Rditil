using System.ComponentModel;
using System.Runtime.CompilerServices;
<<<<<<< HEAD
using Rditil.Models;
using Rditil.Services;
=======
>>>>>>> 1908e362463a42654d4b699764460fd7abed45f8

namespace Rditil.ViewModels
{
    public interface INavigable
    {
        void OnNavigatedTo(Dictionary<string, object>? parameters = null);
    }

    public class WelcomeViewModel : INotifyPropertyChanged, INavigable
    {
        private readonly IAppState _appState;

        private string _nomUtilisateur = "Utilisateur";
        public string NomUtilisateur
        {
            get => _nomUtilisateur;
            set { _nomUtilisateur = value; OnPropertyChanged(); }
        }

        public Utilisateur? Utilisateur => _appState.CurrentUser;

        public WelcomeViewModel(IAppState appState)
        {
            _appState = appState;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public void OnNavigatedTo(Dictionary<string, object>? parameters = null)
        {
            var u = _appState.CurrentUser;
            NomUtilisateur = u?.Prenom ?? "Utilisateur";
        }
    }
}
