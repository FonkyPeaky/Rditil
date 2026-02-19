namespace Rditil.Services
{
    public class SmtpSettings
    {
        public string Host { get; set; } = "";
        public int Port { get; set; } = 587;
        public bool UseSsl { get; set; } = true;

        public string Username { get; set; } = "";
        public string Password { get; set; } = "";

        public string FromName { get; set; } = "RDITIL";
        public string FromEmail { get; set; } = "";
    }
}
