namespace BuildingBlocks.Identity.Jwt
{
    public class AppJwtOptions
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string SecretKey { get; set; }
        public int Expiration { get; set; }
        public int RefreshTokenExpiration { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }
}
