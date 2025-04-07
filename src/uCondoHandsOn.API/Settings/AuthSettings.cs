namespace uCondo.Planos.API.Settings
{
    public class AuthSettings
    {
        public required string Authority { get; set; }
        public required string Audience { get; set; }
        public required string ClientId { get; set; }
        public required string ClientSecret { get; set; }
    }
}