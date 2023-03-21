namespace kc_webapi.Models
{
    public class KeyCloakTokenRequest
    {
        public string username { get; set; }
        public string password { get; set; }
        public string client_id { get; set; }
        public string grant_type { get; set; }
        public string refresh_token { get; set; }
    }
    public static class GrantType
    {
        public static readonly string ClientCredentials = "client_credentials";
        public static readonly string RefreshToken = "refresh_token";
        public static readonly string Password = "password";
        public static readonly string Logout = "logout";
    }
}
