using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace RRHH_WEB_API._Common
{
    public class Seguridad
    {
        private const int SaltSize = 16; // 128 bit
        private const int KeySize = 32;  // 256 bit
        private const int Iterations = 10000;

        public byte[] GenerarHash(string pin)
        {
            if (string.IsNullOrEmpty(pin)) throw new ArgumentNullException(nameof(pin));

            byte[] salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            byte[] hash = KeyDerivation.Pbkdf2(
                password: pin,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: Iterations,
                numBytesRequested: KeySize);

            byte[] hashBytes = new byte[SaltSize + KeySize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, KeySize);

            return hashBytes;
        }

        public bool VerificarHash(string pin, byte[] hashAlmacenado)
        {
            if (string.IsNullOrEmpty(pin)) return false;
            if (hashAlmacenado == null || hashAlmacenado.Length != SaltSize + KeySize) return false;

            byte[] salt = new byte[SaltSize];
            Array.Copy(hashAlmacenado, 0, salt, 0, SaltSize);

            byte[] hashEsperado = KeyDerivation.Pbkdf2(
                password: pin,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: Iterations,
                numBytesRequested: KeySize);

            for (int i = 0; i < KeySize; i++)
            {
                if (hashAlmacenado[SaltSize + i] != hashEsperado[i]) return false;
            }

            return true;
        }
    }
}
