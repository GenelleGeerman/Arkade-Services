using System.Security.Cryptography;

namespace BusinessLayer.Services;

public class EncryptionService
{
    private const int KEY_SIZE = 64;

    public string EncryptPassword(string password, out byte[] salt)
    {
        salt = RandomNumberGenerator.GetBytes(KEY_SIZE);
        byte[] hash = Hash(password, salt);
        return Convert.ToHexString(hash);
    }

    public bool IsMatching(string encryptedPassword, string plainPassword, byte[] salt)
    {
        byte[] hash = Hash(plainPassword, salt);
        string password = Convert.ToHexString(hash);

        return encryptedPassword.Equals(password);
    }

    private byte[] Hash(string password, byte[] salt)
    {
        byte[] combinedBytes = new byte[password.Length + salt.Length];
        Buffer.BlockCopy(password.ToArray(), 0, combinedBytes, 0, password.Length);
        Buffer.BlockCopy(salt, 0, combinedBytes, password.Length, salt.Length);

        // Compute hash
        return SHA256.HashData(combinedBytes);
    }
}
