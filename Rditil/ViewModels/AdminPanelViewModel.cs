using System;
using System.Windows.Input;
using Rditil.Models;
using Rditil.Services;
using CommunityToolkit.Mvvm.Input;

namespace Rditil.ViewModels
{
    public class AdminPanelViewModel : ViewModelBase
    {
        
        private Utilisateur _nouvelUtilisateur;

        public Utilisateur NouvelUtilisateur
        {
            get => _nouvelUtilisateur;
            set
            {
                _nouvelUtilisateur = value;
                OnPropertyChanged(nameof(NouvelUtilisateur));
            }
        }

        public ICommand AjouterCommand { get; }

        /*public AdminPanelViewModel(IExcelService excelService)
        {
            _excelService = excelService;
            NouvelUtilisateur = new Utilisateur
            {
                DernierExamen = DateTime.MinValue,
                Score = 0
            };
            AjouterCommand = new RelayCommand<object?>(ExecuteAjouter);
        }*/

    }
} 