using System.Security.Cryptography;
using System.Text;

public static class PasswordHelper
{
    public static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        byte[] inputBytes = Encoding.UTF8.GetBytes(password);
        byte[] hashBytes = sha256.ComputeHash(inputBytes);
        return Convert.ToBase64String(hashBytes);
    }

    public static bool VerifyPassword(string enteredPassword, string hashedPassword)
    {
        var enteredHash = HashPassword(enteredPassword);
        return enteredHash == hashedPassword;
    }
}
