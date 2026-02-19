using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Rditil.Data;
using Rditil.Models;
<<<<<<< HEAD
using Rditil.Services;
using System.Collections.ObjectModel;
=======
>>>>>>> 1908e362463a42654d4b699764460fd7abed45f8
using System.Windows;

namespace Rditil.ViewModels
{
    public class AdminPanelViewModel : ViewModelBase
    {
        private readonly IDbContextFactory<AppDbContext> _dbFactory;
        private readonly IAuditLogger _audit;

        public event Action? RequestClose;

        private const string EmailDomain = "@randstaddigital.lu";
        private bool _isEmailManualEdit = false;

        private NPlusOneOption? _selectedNPlusOne;
        private string _motDePasse = "";

        public Utilisateur NouvelUtilisateur { get; set; } = new();

        public string MotDePasse
        {
            get => _motDePasse;
            set
            {
                _motDePasse = value;
                OnPropertyChanged(nameof(MotDePasse));
            }
        }
        public string Prenom
        {
            get => NouvelUtilisateur.Prenom ?? "";
            set
            {
                NouvelUtilisateur.Prenom = value;
                OnPropertyChanged(nameof(Prenom));
                UpdateEmailIfNeeded();
            }
        }

        public string Nom
        {
            get => NouvelUtilisateur.Nom ?? "";
            set
            {
                NouvelUtilisateur.Nom = value;
                OnPropertyChanged(nameof(Nom));
                UpdateEmailIfNeeded();
            }
        }

        public string Email
        {
            get => NouvelUtilisateur.Email ?? "";
            set
            {
                _isEmailManualEdit = true; // si on modifie l'email à la main, on stop l'auto
                NouvelUtilisateur.Email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public IAsyncRelayCommand AjouterCommand { get; }

        public AdminPanelViewModel(IDbContextFactory<AppDbContext> dbFactory, IAuditLogger audit)
        {
            _dbFactory = dbFactory;
            _audit = audit;

            AjouterCommand = new AsyncRelayCommand(AjouterAsync);

            // Marina par défaut
            SelectedNPlusOne = NPlusOneOptions.FirstOrDefault();
        }

        // -------------------------
        // N+1
        // -------------------------
        public class NPlusOneOption
        {
            public string Name { get; set; } = "";
            public string Email { get; set; } = "";
        }

        public ObservableCollection<NPlusOneOption> NPlusOneOptions { get; } = new()
        {
            new NPlusOneOption { Name = "Marina Bras",      Email = "Marina.Bras@randstaddigital.lu" },
            new NPlusOneOption { Name = "Sara Minhas",      Email = "Sara.Minhas@randstaddigital.lu" },
            new NPlusOneOption { Name = "Angelique Farenc", Email = "Angelique.Farenc@randstaddigital.lu" },
            new NPlusOneOption { Name = "Reda Qorchi",      Email = "Reda.Qorchi@randstaddigital.lu" },
            new NPlusOneOption { Name = "Maxence Berrien",  Email = "Maxence.Berrien@randstaddigital.lu" },
        };

        public NPlusOneOption? SelectedNPlusOne
        {
            get => _selectedNPlusOne;
            set
            {
                _selectedNPlusOne = value;
                OnPropertyChanged(nameof(SelectedNPlusOne));
            }
        }

        // -------------------------
        // Email auto
        // -------------------------
        private static string NormalizePart(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return "";

            var s = input.Trim()
                         .Replace(" ", "")
                         .Replace("'", "")
                         .Replace("-", "");

            return s.ToLowerInvariant();
        }

        private void UpdateEmailIfNeeded()
        {
            if (_isEmailManualEdit) return;

            var prenom = NormalizePart(NouvelUtilisateur.Prenom ?? "");
            var nom = NormalizePart(NouvelUtilisateur.Nom ?? "");

            // Si incomplet, on vide l'email
            if (string.IsNullOrWhiteSpace(prenom) || string.IsNullOrWhiteSpace(nom))
            {
                NouvelUtilisateur.Email = "";
                OnPropertyChanged(nameof(Email));
                return;
            }

            NouvelUtilisateur.Email = $"{prenom}.{nom}{EmailDomain}";
            OnPropertyChanged(nameof(Email));
        }

        // -------------------------
        // Ajouter
        // -------------------------
        private async Task AjouterAsync()
        {
            try
            {
                var email = (NouvelUtilisateur.Email ?? "").Trim().ToLowerInvariant();

                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(MotDePasse))
                {
                    MessageBox.Show("Email et mot de passe sont requis.", "Validation",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (MotDePasse.Length < 5 || !MotDePasse.Any(char.IsDigit))
                {
                    MessageBox.Show("Le mot de passe doit contenir au moins 5 caractères et un chiffre.", "Validation",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }


                await using var db = await _dbFactory.CreateDbContextAsync();

                var existe = await db.Utilisateurs
                    .AsNoTracking()
                    .AnyAsync(u => u.Email.ToLower() == email);

                if (existe)
                {
                    MessageBox.Show("Un utilisateur avec cet email existe déjà.", "Doublon",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                NouvelUtilisateur.Email = email;
                NouvelUtilisateur.PasswordHash = PasswordHelper.HashPassword(MotDePasse);

                // N+1 en base (email)
                NouvelUtilisateur.EmailNPlus1 = SelectedNPlusOne?.Email ?? "Marina.Bras@randstaddigital.lu";

                NouvelUtilisateur.Score = 0;
                NouvelUtilisateur.DernierExamen = DateTime.UtcNow;

                db.Utilisateurs.Add(NouvelUtilisateur);
                await db.SaveChangesAsync();

                // Audit
                var operatorName = $"{Environment.UserDomainName}\\{Environment.UserName}";
                await _audit.LogUserCreationAsync(operatorName, email);

                MessageBox.Show("Utilisateur ajouté avec succès", "Succès",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                // Reset propre
                NouvelUtilisateur = new Utilisateur();
                _isEmailManualEdit = false;
                SelectedNPlusOne = NPlusOneOptions.FirstOrDefault(); // Marina
                MotDePasse = "";

                OnPropertyChanged(nameof(Prenom));
                OnPropertyChanged(nameof(Nom));
                OnPropertyChanged(nameof(Email));
                OnPropertyChanged(nameof(SelectedNPlusOne));
                OnPropertyChanged(nameof(MotDePasse));

                // Ferme la fenêtre Admin après OK
                RequestClose?.Invoke();
            }
            catch (DbUpdateException ex)
            {
                var details = ex.InnerException?.Message ?? ex.Message;
                MessageBox.Show(details, "Erreur BDD", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
