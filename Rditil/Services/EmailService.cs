using System;
using System.Threading.Tasks;
using Rditil.Models;
using System.Diagnostics;
using System.Windows;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace Rditil.Services
{
    public class EmailService
    {
        private readonly SmtpSettings _settings;

        public EmailService(SmtpSettings settings)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            Debug.WriteLine($"[EmailService] Configuration SMTP :\n" +
                          $"Server: {_settings.Server}\n" +
                          $"Port: {_settings.Port}\n" +
                          $"Username: {_settings.Username}\n" +
                          $"FromEmail: {_settings.FromEmail}\n" +
                          $"EnableSsl: {_settings.EnableSsl}");
        }

        public async Task SendExamResultAsync(string toEmail, string userName, int score, int totalQuestions)
        {
            try
            {
                Debug.WriteLine($"[EmailService] Préparation de l'envoi à {toEmail}");

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("ITIL Exam", _settings.FromEmail));
                message.To.Add(MailboxAddress.Parse(toEmail));
                message.Subject = "Résultat de l'examen ITIL";
                message.Body = new TextPart("plain")
                {
                    Text = $@"Bonjour {userName},

                            Voici le résultat de votre examen ITIL :
                            Score : {score}/{totalQuestions}
                            Pourcentage : {(score * 100.0 / totalQuestions):F2}%

                            Cordialement,
                            L'équipe RD"
                };

                using (var client = new SmtpClient())
                {
                    Debug.WriteLine("[EmailService] Connexion au serveur SMTP...");
                    await client.ConnectAsync(_settings.Server, _settings.Port, SecureSocketOptions.StartTls);
                    Debug.WriteLine("[EmailService] Authentification...");
                    await client.AuthenticateAsync(_settings.Username, _settings.Password);
                    Debug.WriteLine("[EmailService] Envoi du message...");
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                    Debug.WriteLine("[EmailService] Email envoyé avec succès !");
                }
            }
            catch (Exception ex)
            {
                string inner = ex.InnerException != null ? $"\nDétails : {ex.InnerException.Message}" : "";
                MessageBox.Show($"Erreur lors de l'envoi de l'email : {ex.Message}{inner}", "Erreur SMTP", MessageBoxButton.OK, MessageBoxImage.Error);
                Debug.WriteLine($"[EmailService] Erreur : {ex.Message}{inner}");
                throw;
            }
        }
    }
} 