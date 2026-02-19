using System;

namespace Rditil.Data.Entities
{
    public class SmtpSettingEntity
    {
        public int Id { get; set; }
        public string Host { get; set; } = "";
        public int Port { get; set; } = 587;
        public bool UseStartTls { get; set; } = true;
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public string FromEmail { get; set; } = "";
        public bool Enabled { get; set; } = true;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}
