namespace PetCare.Shared.Config
{
    public class JWTConfig
    {
        public required string Secret { get; set; }
        public required string ValidIssuer { get; set; }
    }
}
