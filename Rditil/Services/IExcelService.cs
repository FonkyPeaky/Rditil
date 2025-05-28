using System.Collections.Generic;
using Rditil.Models;

namespace Rditil.Services
{
    public interface IExcelService
    {
        List<Question> GetRandomQuestions(int count);
        void SaveUserResult(string email, int score);
        List<User> GetAllUsers();
        void AddUser(User user);
        void UpdateUser(User user);
        void DeleteUser(string email);
    }
} 