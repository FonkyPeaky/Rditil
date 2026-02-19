using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Rditil.Services
{
    public class FileAuditLogger : IAuditLogger
    {
        private readonly string _logPath;

        public FileAuditLogger()
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var dir = Path.Combine(appData, "Rditil");
            Directory.CreateDirectory(dir);
            _logPath = Path.Combine(dir, "audit.log");
        }

        public Task LogUserCreationAsync(string operatorName, string createdUserEmail)
        {
            var line = $"{DateTime.UtcNow:O}\tUSER_CREATE\toperator={operatorName}\ttarget={createdUserEmail}";
            return File.AppendAllTextAsync(_logPath, line + Environment.NewLine, Encoding.UTF8);
        }
    }
}
