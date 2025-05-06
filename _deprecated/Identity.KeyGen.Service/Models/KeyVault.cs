namespace Identity.KeyGen.Service.Models
{
    public class KeyVault
    {
        public Guid Id {get; set;}
        public required string PrivateKey {get; set;}
        public required string PublicKey  {get; set;}
        public required DateTime CreatedAt {get; set;}
        public required DateTime Expiration {get; set;}
    }
}