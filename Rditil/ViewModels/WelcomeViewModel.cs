using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Rditil.Models;
using Rditil.Services;
using System.Collections.Generic;

namespace Rditil.ViewModels
{
    public class WelcomeViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService;

        public Utilisateur CurrentUser { get; set; } = null!;
        public string ManagerEmail { get; set; } = "";

        public string UserFullName =>
            $"{CurrentUser.Prenom} {CurrentUser.Nom}";

        public IRelayCommand StartExamCommand { get; }

        public WelcomeViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            StartExamCommand = new RelayCommand(StartExam);
        }

        private void StartExam()
        {
            _navigationService.NavigateTo<ExamViewModel>(
                new Dictionary<string, object?>
                {
                    ["CurrentUser"] = CurrentUser,
                    ["UserEmail"] = CurrentUser.Email,
                    ["ManagerEmail"] = ManagerEmail
                });
        }
    }
}
