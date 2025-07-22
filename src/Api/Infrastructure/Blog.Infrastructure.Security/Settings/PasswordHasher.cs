using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Infrastructure.Security.Settings;


public static class PasswordHasher
{
    private const int SaltSize = 16;   // 128 bit
    private const int KeySize = 32;    // 256 bit
    private const int Iterations = 10000; // PBKDF2 iterasyon sayısı


    public static string HashPassword(string password)
    {
        using var algorithm = new Rfc2898DeriveBytes(
            password,
            SaltSize,
            Iterations,
            HashAlgorithmName.SHA256);

        var key = Convert.ToBase64String(algorithm.GetBytes(KeySize));
        var salt = Convert.ToBase64String(algorithm.Salt);

        return $"{salt}.{key}";
    }


    public static bool VerifyPassword(string hashedPassword, string inputPassword)
    {
        var parts = hashedPassword.Split('.');
        if (parts.Length != 2)
        {
            // Format hatası
            return false;
        }

        var salt = Convert.FromBase64String(parts[0]);
        var key = Convert.FromBase64String(parts[1]);

        using var algorithm = new Rfc2898DeriveBytes(
            inputPassword,
            salt,
            Iterations,
            HashAlgorithmName.SHA256);

        var keyToCheck = algorithm.GetBytes(KeySize);

        // İki hash değeri byte byte aynı mı kontrol ediyoruz.
        return CryptographicOperations.FixedTimeEquals(keyToCheck, key);
    }
}

