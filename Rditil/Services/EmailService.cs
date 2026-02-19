using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Rditil.Models;

namespace Rditil.Services
{
    public interface IEmailService
    {
        Task SendExamReportAsync(
            string to,
            string? cc,
            string subject,
            string userFullName,
            int score,
            int total,
            TimeSpan duration,
            IReadOnlyList<ExamReportRow> rows);
    }

    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _smtp;

        public EmailService(IOptions<SmtpSettings> smtpOptions)
        {
            _smtp = smtpOptions.Value ?? new SmtpSettings();
        }

        public async Task SendExamReportAsync(
            string to,
            string? cc,
            string subject,
            string userFullName,
            int score,
            int total,
            TimeSpan duration,
            IReadOnlyList<ExamReportRow> rows)
        {
            if (!_smtp.Enabled)
                throw new InvalidOperationException("SMTP désactivé (enabled=false). Active-le dans appsettings.json.");

            if (string.IsNullOrWhiteSpace(_smtp.Host) || string.IsNullOrWhiteSpace(_smtp.FromEmail))
                throw new InvalidOperationException("SMTP incomplet : Host/FromEmail manquants.");

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_smtp.FromName ?? "RDITIL", _smtp.FromEmail));
            message.To.Add(MailboxAddress.Parse(to));

            if (!string.IsNullOrWhiteSpace(cc))
                message.Cc.Add(MailboxAddress.Parse(cc));

            message.Subject = subject;

            var htmlBody = BuildHtmlBody(userFullName, score, total, duration, rows);
            message.Body = new BodyBuilder { HtmlBody = htmlBody }.ToMessageBody();

            using var client = new MailKit.Net.Smtp.SmtpClient();

            // Choix TLS
            var secure =
                _smtp.UseSsl
                    ? MailKit.Security.SecureSocketOptions.SslOnConnect
                    : (_smtp.UseStartTls
                        ? MailKit.Security.SecureSocketOptions.StartTls
                        : MailKit.Security.SecureSocketOptions.StartTlsWhenAvailable);

            await client.ConnectAsync(_smtp.Host, _smtp.Port, secure);

            if (!string.IsNullOrWhiteSpace(_smtp.Username))
                await client.AuthenticateAsync(_smtp.Username, _smtp.Password);

            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }

        private static string BuildHtmlBody(
            string userFullName, int score, int total, TimeSpan duration, IReadOnlyList<ExamReportRow> rows)
        {
            static string H(string s) => WebUtility.HtmlEncode(s ?? "");

            var sb = new StringBuilder();
            sb.AppendLine("<html><body style='font-family:Segoe UI, Arial; font-size:14px;'>");
            sb.AppendLine("<h2 style='margin:0 0 6px 0;'>RDITIL — Résultats</h2>");
            sb.AppendLine($"<div><b>Utilisateur :</b> {H(userFullName)}</div>");
            sb.AppendLine($"<div><b>Score :</b> {score}/{total}</div>");
            sb.AppendLine($"<div><b>Durée :</b> {duration:hh\\:mm\\:ss}</div>");
            sb.AppendLine("<hr/>");

            sb.AppendLine("<table cellspacing='0' cellpadding='8' style='border-collapse:collapse; width:100%;'>");
            sb.AppendLine("<thead><tr style='background:#f3f6fb;'>");
            sb.AppendLine("<th align='left' style='border:1px solid #dbe3ef;'>#</th>");
            sb.AppendLine("<th align='left' style='border:1px solid #dbe3ef;'>Question</th>");
            sb.AppendLine("<th align='left' style='border:1px solid #dbe3ef;'>Choix</th>");
            sb.AppendLine("<th align='left' style='border:1px solid #dbe3ef;'>Résultat</th>");
            sb.AppendLine("</tr></thead><tbody>");

            for (int i = 0; i < rows.Count; i++)
            {
                var r = rows[i];
                var ok = r.IsCorrect ? "✅ Correct" : "❌ Faux";
                sb.AppendLine("<tr>");
                sb.AppendLine($"<td style='border:1px solid #dbe3ef;'>{i + 1}</td>");
                sb.AppendLine($"<td style='border:1px solid #dbe3ef;'>{H(r.Enonce)}</td>");
                sb.AppendLine($"<td style='border:1px solid #dbe3ef;'>{H(r.ChosenText)}</td>");
                sb.AppendLine($"<td style='border:1px solid #dbe3ef;'>{ok}");
                if (!r.IsCorrect && !string.IsNullOrWhiteSpace(r.CorrectText))
                    sb.AppendLine($"<br/><b>Bonne réponse :</b> {H(r.CorrectText)}");
                sb.AppendLine("</td></tr>");
            }

            sb.AppendLine("</tbody></table>");
            sb.AppendLine("</body></html>");
            return sb.ToString();
        }
    }
}
