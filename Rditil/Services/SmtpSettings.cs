namespace Rditil.Services
{
    public class SmtpSettings
    {
        public bool Enabled { get; set; } = false;
        public int Id { get; set; }
        public string Host { get; set; } = "";
        public int Port { get; set; } = 587;

        public bool UseSsl { get; set; } = false;
        public bool UseStartTls { get; set; } = true;

        public string Username { get; set; } = "";
        public string Password { get; set; } = "";

        public string FromEmail { get; set; } = "";
        public string FromName { get; set; } = "RDITIL";
    }
}
