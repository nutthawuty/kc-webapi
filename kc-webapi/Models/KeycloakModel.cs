using Newtonsoft.Json;

namespace kc_webapi.Models
{
    class KeycloakPayload
    {
        public string client_id { get; set; }
        public string client_secrent { get; set; }
        public string grant_type { get; set; }
        public string scope { get; set; }
    }
    public class KeycloakResponse<T>
    {
        public string message { get; set; }
        public T data { get; set; }
    }
    public class KeycloakTokenResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("refresh_expires_in")]
        public int RefreshExpiresIn { get; set; }

        [JsonProperty("not-before-policy")]
        public int NotBeforePolicy { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }
    }
}
