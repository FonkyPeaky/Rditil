using BCrypt.Net;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Rditil.Data;
using Rditil.Models;
using Rditil.Services;
using System;
using System.Diagnostics;
using System.Windows.Input;


namespace Rditil.ViewModels
{
    public class AdminPanelViewModel : ViewModelBase
    {
        private readonly AppDbContext _dbContext;

        public Utilisateur NouvelUtilisateur { get; set; } = new();

        public string MotDePasse { get; set; } = "";

        public ICommand AjouterCommand { get; }

        public Utilisateur NewUser { get; set; } = new Utilisateur();
        public ICommand AddUserCommand { get; }


        public AdminPanelViewModel(AppDbContext dbContext)
        {
            
            _dbContext = dbContext;
            AjouterCommand = new RelayCommand(ExecuteAjouter);
            AddUserCommand = new RelayCommand(AddUser);

        }

        // ExecuteAjouter est appelé lorsque l'utilisateur clique sur le bouton "Ajouter"
        private async void ExecuteAjouter()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(NouvelUtilisateur.Email) || string.IsNullOrWhiteSpace(MotDePasse))
                {
                    Console.WriteLine("⚠️ Champs requis manquants.");
                    return;
                }

                var email = NouvelUtilisateur.Email.Trim().ToLower();

                var utilisateurExistant = await _dbContext.Utilisateurs
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Email.ToLower() == email);

                if (utilisateurExistant != null)
                {
                    Console.WriteLine("❌ Un utilisateur avec cet email existe déjà.");
                    return;
                }

                // Hash du mot de passe
                NouvelUtilisateur.Password = PasswordHelper.HashPassword(MotDePasse);

                // ✅ Valeurs par défaut nécessaires
                NouvelUtilisateur.Score = 0;
                NouvelUtilisateur.DernierExamen = DateTime.UtcNow;

                // Ajout à la base
                _dbContext.Utilisateurs.Add(NouvelUtilisateur);
                await _dbContext.SaveChangesAsync();

                Console.WriteLine("✅ Utilisateur ajouté avec succès.");

                // Reset des champs
                NouvelUtilisateur = new Utilisateur();
                MotDePasse = "";
                OnPropertyChanged(nameof(NouvelUtilisateur));
                OnPropertyChanged(nameof(MotDePasse));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Erreur lors de l'ajout : {ex.Message}");
            }
        }
        private void AddUser()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(NewUser.Password))
                {
                    NewUser.Password = BCrypt.Net.BCrypt.HashPassword(NewUser.Password);
                    NewUser.DernierExamen = DateTime.UtcNow;
                    NewUser.Score = 0;

                    _dbContext.Utilisateurs.Add(NewUser);
                    _dbContext.SaveChanges();

                    Debug.WriteLine($"✅ Utilisateur ajouté : {NewUser.Email}");
                    NewUser = new Utilisateur(); // Reset form
                    OnPropertyChanged(nameof(NewUser));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Erreur d'ajout utilisateur : {ex.Message}");
            }
        }


    }
}
