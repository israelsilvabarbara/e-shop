using System.Security.Cryptography;
using Identity.KeyGen.Service.Models;


namespace Identity.KeyGen.Service.Services
{
    public class KeyGenerator
    {
        public KeyPair GenerateKeyPair()
        {
            using var rsa = new RSACryptoServiceProvider(2048);
            var privateKey = Convert.ToBase64String(rsa.ExportRSAPrivateKey());
            var publicKey = Convert.ToBase64String(rsa.ExportRSAPublicKey());
            return new KeyPair(privateKey, publicKey);
        }
    }
}
