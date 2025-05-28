using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Rditil.Data;
using Rditil.Models;
using Rditil.Services;
using System.Configuration;
using System.Data;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System;

namespace Rditil
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Utilisateur? CurrentUser { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                base.OnStartup(e);

                // Vérifier si le fichier de configuration existe
                if (!File.Exists("appsettings.json"))
                {
                    MessageBox.Show("Le fichier appsettings.json est introuvable.", "Erreur de Configuration", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Charger la configuration SMTP
                var configText = File.ReadAllText("appsettings.json");
                var config = JsonSerializer.Deserialize<ConfigRoot>(configText);
                
                if (config?.SmtpSettings == null)
                {
                    MessageBox.Show("La configuration SMTP est manquante dans appsettings.json.", "Erreur de Configuration", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var smtpSettings = config.SmtpSettings;
                var emailService = new EmailService(smtpSettings);

                // Envoyer un email de test
                Task.Run(async () =>
                {
                    try
                    {
                        await emailService.SendExamResultAsync(
                            "issam.elafi@randstaddigital.lu",
                            "Test SMTP Outlook",
                            40,
                            40
                        );
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            MessageBox.Show("Email de test envoyé avec succès !", "SMTP Test", MessageBoxButton.OK, MessageBoxImage.Information);
                        });
                    }
                    catch (Exception ex)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            MessageBox.Show($"Erreur lors de l'envoi de l'email : {ex.Message}\n\nDétails : {ex.InnerException?.Message}", 
                                "Erreur SMTP", MessageBoxButton.OK, MessageBoxImage.Error);
                        });
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur au démarrage de l'application : {ex.Message}\n\nDétails : {ex.InnerException?.Message}", 
                    "Erreur Critique", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    public class ConfigRoot
    {
        public SmtpSettings SmtpSettings { get; set; }
    }
}
