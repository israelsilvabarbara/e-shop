using System.Security.Cryptography;
using Identity.API.Models;

namespace Identity.API.Utils
{
    public static class KeyUtils
    {
        public static KeyPair GenerateKeys()
        {
            using var rsa = new RSACryptoServiceProvider(2048);
            var currentPrivateKey = Convert.ToBase64String(rsa.ExportRSAPrivateKey());
            var currentPublicKey = Convert.ToBase64String(rsa.ExportRSAPublicKey());

            return new KeyPair
            (
                PrivateKey: currentPrivateKey,
                PublicKey: currentPublicKey
            );
        }
    }
}
