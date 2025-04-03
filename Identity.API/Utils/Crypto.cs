namespace Identity.API.Utils
{
    public static class Crypto
    {
        public static (string privateKey, string publicKey) GenerateKeyPair()
        {
            using var rsa = RSA.Create();
            var privateKey = Convert.ToBase64String(rsa.ExportRSAPrivateKey());
            var publicKey = Convert.ToBase64String(rsa.ExportRSAPublicKey());
            return (privateKey, publicKey);
        }

        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool VerifyPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }

    }