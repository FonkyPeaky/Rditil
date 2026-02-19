using System.Threading.Tasks;

namespace Rditil.Services
{
    public interface IAuditLogger
    {
        Task LogUserCreationAsync(string operatorName, string createdUserEmail);
    }
}
