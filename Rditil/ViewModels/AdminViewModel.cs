using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Rditil.Data;
using Rditil.Models;
using System.Collections.ObjectModel;

namespace Rditil.ViewModels
{
    public partial class AdminViewModel : ObservableObject
    {
        private readonly AppDbContext _context;

        [ObservableProperty]
        private ObservableCollection<Utilisateur> utilisateurs;

        [ObservableProperty]
        private Utilisateur utilisateurSelectionne;

        [ObservableProperty]
        private Utilisateur nouvelUtilisateur = new();

        public IRelayCommand<object?> AjouterCommand { get; }
        public IRelayCommand<object?> ModifierCommand { get; }
        public IRelayCommand<object?> SupprimerCommand { get; }

        public AdminViewModel(AppDbContext context)
        {
            _context = context;

            AjouterCommand = new RelayCommand<object?>(AjouterUtilisateur);
            ModifierCommand = new RelayCommand<object?>(ModifierUtilisateur);
            SupprimerCommand = new RelayCommand<object?>(SupprimerUtilisateur);

            ChargerUtilisateurs();
        }
        private void ChargerUtilisateurs()
        {
            Utilisateurs = new ObservableCollection<Utilisateur>(_context.Utilisateurs.ToList());
        }
        private void AjouterUtilisateur(object? obj)
        {
            if (!string.IsNullOrWhiteSpace(NouvelUtilisateur.Email) &&
                !string.IsNullOrWhiteSpace(NouvelUtilisateur.Password))
            {
                _context.Utilisateurs.Add(NouvelUtilisateur);
                _context.SaveChanges();
                ChargerUtilisateurs();
                // Réinitialise le formulaire
                NouvelUtilisateur = new Utilisateur();
            }
        }
        private void ModifierUtilisateur(object? obj)
        {
            if (UtilisateurSelectionne != null)
            {
                _context.Utilisateurs.Update(UtilisateurSelectionne);
                _context.SaveChanges();
                ChargerUtilisateurs();
            }
        }
        private void SupprimerUtilisateur(object? obj)
        {
            if (UtilisateurSelectionne != null)
            {
                _context.Utilisateurs.Remove(UtilisateurSelectionne);
                _context.SaveChanges();
                ChargerUtilisateurs();
            }
        }
    }
}
