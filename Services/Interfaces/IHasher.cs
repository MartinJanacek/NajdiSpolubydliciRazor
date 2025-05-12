using System.Security.Cryptography;
using System.Text;

namespace NajdiSpolubydliciRazor.Services.Interfaces
{
    public interface IHasher
    {
        public string Hash(string stringToHash, out byte[] salt);

        public bool Verify(string stringToVerify, string hash, byte[] salt);
    }
}
