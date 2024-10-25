namespace PetCare.Shared.Config
{
    public class JwtConfig
    {
        public required string Secret { get; set; }
        public required string ValidIssuer { get; set; }
    }
}
