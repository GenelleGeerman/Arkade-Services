using System.Security.Cryptography;

namespace BusinessLayer.Services;

public class EncryptionService
{
    private const int KEY_SIZE = 64;

    public static string EncryptPassword(string password, out byte[] salt)
    {
        salt = RandomNumberGenerator.GetBytes(KEY_SIZE);
        return HashString(password, salt);
    }

    public static bool IsMatching(string encryptedPassword, string plainPassword, byte[] salt)
    {
        string password = HashString(plainPassword, salt);
        return encryptedPassword.Equals(password);
    }

    private static string HashString(string password, byte[] salt)
    {
        byte[] combinedBytes = new byte[password.Length + salt.Length];
        Buffer.BlockCopy(password.ToArray(), 0, combinedBytes, 0, password.Length);
        Buffer.BlockCopy(salt, 0, combinedBytes, password.Length, salt.Length);
        byte[] hash = SHA256.HashData(combinedBytes);
        return Convert.ToHexString(hash);
    }
}
