using System.ComponentModel.DataAnnotations;

namespace Rditil.Models
{
    public class SmtpSettings
    {
        [Key]
        public string Server { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FromEmail { get; set; }
        public bool EnableSsl { get; set; }
    }

}
