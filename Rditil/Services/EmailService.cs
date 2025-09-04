using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Options;

namespace Rditil.Services
{
    public interface IEmailService
    {
        Task SendExamResultAsync(string to, string? cc, int score, int total);
    }

    public sealed class EmailService : IEmailService
    {
        private readonly SmtpSettings _settings;

        public EmailService(IOptions<SmtpSettings> options)
        {
            _settings = options.Value;
        }

        public async Task SendExamResultAsync(string to, string? cc, int score, int total)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("ITIL Exams", _settings.User));
            message.To.Add(MailboxAddress.Parse(to));
            if (!string.IsNullOrWhiteSpace(cc))
                message.Cc.Add(MailboxAddress.Parse(cc));

            message.Subject = "Résultat examen ITIL";
            message.Body = new TextPart("plain")
            {
                Text = $"Score: {score}/{total}"
            };

            using var client = new SmtpClient();
            await client.ConnectAsync(_settings.Host, _settings.Port, _settings.EnableSsl);
            await client.AuthenticateAsync(_settings.User, _settings.Password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
