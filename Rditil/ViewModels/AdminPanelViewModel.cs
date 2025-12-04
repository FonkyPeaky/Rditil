using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Rditil.Data;
using Rditil.Models;
using System.Windows;

namespace Rditil.ViewModels
{
    public class AdminPanelViewModel : ViewModelBase
    {
        private readonly AppDbContext _dbContext;

        public Utilisateur NouvelUtilisateur { get; set; } = new();

        private string _motDePasse = "";
        public string MotDePasse
        {
            get => _motDePasse;
            set { _motDePasse = value; OnPropertyChanged(nameof(MotDePasse)); }
        }

        public IAsyncRelayCommand AjouterCommand { get; }

        public AdminPanelViewModel(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            AjouterCommand = new AsyncRelayCommand(AjouterAsync);
        }

        private async Task AjouterAsync()
        {
            try
            {
                var email = (NouvelUtilisateur.Email ?? "").Trim().ToLower();

                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(MotDePasse))
                {
                    MessageBox.Show("Email et mot de passe sont requis.", "Validation", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                var existe = await _dbContext.Utilisateurs
                    .AsNoTracking()
                    .AnyAsync(u => u.Email.ToLower() == email);

                if (existe)
                {
                    MessageBox.Show("Un utilisateur avec cet email existe déjà.", "Doublon", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                NouvelUtilisateur.Email = email;
                NouvelUtilisateur.PasswordHash = PasswordHelper.HashPassword(MotDePasse);
                NouvelUtilisateur.Score = 0;
                NouvelUtilisateur.DernierExamen = DateTime.UtcNow;

                _dbContext.Utilisateurs.Add(NouvelUtilisateur);
                await _dbContext.SaveChangesAsync();

                MessageBox.Show("Utilisateur ajouté avec succès ✅", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);

                // reset du formulaire
                NouvelUtilisateur = new Utilisateur();
                MotDePasse = "";
                OnPropertyChanged(nameof(NouvelUtilisateur));
                OnPropertyChanged(nameof(MotDePasse));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'ajout : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
