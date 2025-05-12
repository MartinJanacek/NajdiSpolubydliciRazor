using System.Security.Cryptography;
using System.Text;
using NajdiSpolubydliciRazor.Services.Interfaces;

namespace NajdiSpolubydliciRazor.Services
{
    public class Hasher : IHasher
    {
        private const int keySize = 64;
        private const int iterations = 350000;
        private readonly HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

        public string Hash(string stringToHash, out byte[] salt)
        {
            salt = RandomNumberGenerator.GetBytes(keySize);

            var hash = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(stringToHash), salt, iterations, hashAlgorithm, keySize);

            return Convert.ToHexString(hash);
        }

        public bool Verify(string stringToVerify, string hash, byte[] salt)
        {
            var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(stringToVerify, salt, iterations, hashAlgorithm, keySize);

            return CryptographicOperations.FixedTimeEquals(hashToCompare, Convert.FromHexString(hash));
        }
    }
}
