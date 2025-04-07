namespace uCondo.Planos.API.Settings
{
    public class OAuth2Settings
    {
        public required string Authority { get; set; }
        public required string Audience { get; set; }
        public required string ClientId { get; set; }
        public required string ClientSecret { get; set; }
    }
}