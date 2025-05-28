using System;
using System.Windows.Input;
using Rditil.Models;
using Rditil.Services;
using CommunityToolkit.Mvvm.Input;

namespace Rditil.ViewModels
{
    public class AdminPanelViewModel : ViewModelBase
    {
        private readonly IExcelService _excelService;
        private User _nouvelUtilisateur;

        public User NouvelUtilisateur
        {
            get => _nouvelUtilisateur;
            set
            {
                _nouvelUtilisateur = value;
                OnPropertyChanged(nameof(NouvelUtilisateur));
            }
        }

        public ICommand AjouterCommand { get; }

        public AdminPanelViewModel(IExcelService excelService)
        {
            _excelService = excelService;
            NouvelUtilisateur = new User
            {
                DernierExamen = DateTime.MinValue,
                Score = 0
            };
            AjouterCommand = new RelayCommand<object?>(ExecuteAjouter);
        }

        private void ExecuteAjouter(object? parameter)
        {
            if (string.IsNullOrWhiteSpace(NouvelUtilisateur.Email) ||
                string.IsNullOrWhiteSpace(NouvelUtilisateur.Nom) ||
                string.IsNullOrWhiteSpace(NouvelUtilisateur.MotDePasse))
            {
                // Afficher un message d'erreur
                return;
            }

            try
            {
                _excelService.AddUser(NouvelUtilisateur);
                // Réinitialiser le formulaire
                NouvelUtilisateur = new User
                {
                    DernierExamen = DateTime.MinValue,
                    Score = 0
                };
            }
            catch (Exception ex)
            {
                // Gérer l'erreur
            }
        }
    }
} 