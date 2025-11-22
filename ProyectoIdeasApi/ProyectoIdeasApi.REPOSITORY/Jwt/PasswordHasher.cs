using ApiLibrosController.Interfaces.Jwt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ApiLibrosController.Infrastructure
{
    public class PasswordHasher: IPasswordHasher
    {
        // ⚠️ Ejemplo simple; después podés mejorarlo con más iteraciones/sal, etc.
        public string Hash(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password inválido.", nameof(password));

            // PBKDF2 con sal aleatoria
            using var rng = RandomNumberGenerator.Create();
            var salt = new byte[16];
            rng.GetBytes(salt);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA256);
            var hash = pbkdf2.GetBytes(32);

            // guardamos sal + hash en base64: SAL|HASH
            return $"{Convert.ToBase64String(salt)}|{Convert.ToBase64String(hash)}";
        }

        public bool Verify(string password, string stored)
        {
            if (string.IsNullOrWhiteSpace(stored)) return false;

            var parts = stored.Split('|');
            if (parts.Length != 2) return false;

            var salt = Convert.FromBase64String(parts[0]);
            var expectedHash = Convert.FromBase64String(parts[1]);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA256);
            var hash = pbkdf2.GetBytes(32);

            // comparamos byte a byte
            return CryptographicOperations.FixedTimeEquals(hash, expectedHash);
        }
    }
}
