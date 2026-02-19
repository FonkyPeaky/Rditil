using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Rditil.Models;

namespace Rditil.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _smtp;

        public EmailService(SmtpSettings smtp)
        {
            _smtp = smtp;
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
            if (string.IsNullOrWhiteSpace(_smtp.Host))
                throw new InvalidOperationException("SMTP Host manquant (appsettings.json).");
            if (string.IsNullOrWhiteSpace(_smtp.FromEmail))
                throw new InvalidOperationException("SMTP FromEmail manquant (appsettings.json).");
            if (string.IsNullOrWhiteSpace(to))
                throw new ArgumentException("Destinataire (to) vide.");

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_smtp.FromName, _smtp.FromEmail));
            message.To.Add(MailboxAddress.Parse(to));

            if (!string.IsNullOrWhiteSpace(cc))
                message.Cc.Add(MailboxAddress.Parse(cc));

            message.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = BuildHtml(userFullName, score, total, duration, rows)
            };

            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();

            // 587 -> StartTls le plus souvent
            var socketOptions = _smtp.UseSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.None;

            await client.ConnectAsync(_smtp.Host, _smtp.Port, socketOptions);

            if (!string.IsNullOrWhiteSpace(_smtp.Username))
                await client.AuthenticateAsync(_smtp.Username, _smtp.Password);

            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }

        private static string BuildHtml(
            string userFullName,
            int score,
            int total,
            TimeSpan duration,
            IReadOnlyList<ExamReportRow> rows)
        {
            static string Html(string? s) => System.Net.WebUtility.HtmlEncode(s ?? "");

            var sb = new StringBuilder();

            sb.AppendLine("<html><body style='font-family:Segoe UI, Arial; font-size:14px;'>");
            sb.AppendLine("<h2 style='margin:0;'>RDITIL - Rapport d'examen</h2>");
            sb.AppendLine("<div style='margin-top:10px;'>");
            sb.AppendLine($"<b>Candidat :</b> {Html(userFullName)}<br/>");
            sb.AppendLine($"<b>Score :</b> {score}/{total}<br/>");
            sb.AppendLine($"<b>Durée :</b> {duration:hh\\:mm\\:ss}");
            sb.AppendLine("</div>");

            sb.AppendLine("<hr style='margin:16px 0;border:none;border-top:1px solid #eee;'/>");

            sb.AppendLine("<table cellpadding='8' cellspacing='0' style='border-collapse:collapse; width:100%;'>");
            sb.AppendLine("<tr style='background:#e9f7ff;'>");
            sb.AppendLine("<th align='left' style='border:1px solid #cfefff;'>#</th>");
            sb.AppendLine("<th align='left' style='border:1px solid #cfefff;'>Question</th>");
            sb.AppendLine("<th align='left' style='border:1px solid #cfefff;'>Réponse choisie</th>");
            sb.AppendLine("<th align='left' style='border:1px solid #cfefff;'>Résultat</th>");
            sb.AppendLine("</tr>");

            for (int i = 0; i < rows.Count; i++)
            {
                var r = rows[i];
                var status = r.IsCorrect ? "✅ Correct" : "❌ Faux";

                sb.AppendLine("<tr>");
                sb.AppendLine($"<td style='border:1px solid #eee;'>{i + 1}</td>");
                sb.AppendLine($"<td style='border:1px solid #eee;'>{Html(r.Enonce)}</td>");
                sb.AppendLine($"<td style='border:1px solid #eee;'>{Html(r.ChosenText)}</td>");

                var correctPart = string.IsNullOrWhiteSpace(r.CorrectText)
                    ? ""
                    : "<br/><b>Bonne réponse :</b> " + Html(r.CorrectText);

                sb.AppendLine($"<td style='border:1px solid #eee;'>{status}{correctPart}</td>");
                sb.AppendLine("</tr>");
            }

            sb.AppendLine("</table>");
            sb.AppendLine("</body></html>");

            return sb.ToString();
        }
    }
}
