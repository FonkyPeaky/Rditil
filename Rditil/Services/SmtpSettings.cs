namespace Rditil.Services
{
    // POCO de configuration pour l'Options pattern
    public sealed class SmtpSettings
    {
        public string Host { get; init; } = "";
        public int Port { get; init; }
        public bool EnableSsl { get; init; } = true;
        public string User { get; init; } = "";
        public string Password { get; init; } = "";
    }
}
