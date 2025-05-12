using NajdiSpolubydliciRazor.Services.Interfaces;
using System.Security.Cryptography;

namespace NajdiSpolubydliciRazor.Services
{
    public class OneTimeCode : IOneTimeCode
    {
        public string GenerateCode()
        {
            byte[] four_bytes = new byte[4];
            RandomNumberGenerator.Create().GetBytes(four_bytes);

            return $"{BitConverter.ToUInt32(four_bytes, 0)}";
        }

        public bool IsTooOld(DateTime lastOperation)
        {
            return DateTime.UtcNow - lastOperation > TimeSpan.FromMinutes(15);
        }
    }
}
