using kc_webapi.Controllers;
using kc_webapi.Interfaces;
using kc_webapi.Models;
using Newtonsoft.Json;

namespace kc_webapi.Services
{
    public class KcService : IKcService
    {
        private ILogger<KcAccountController> _logger;
        private IHttpClientFactory _httpClientFactory;
        public KcService(ILogger<KcAccountController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<KeycloakTokenResponse> getToken()
        {
            var httpClient = _httpClientFactory.CreateClient();
            // add payload-data
            string grant_type = "client_credentials";
            string client_id = "logic-school";
            string client_secret = "He7S3RVTZhdil4asBBRspHGzPFJjjlOe";
            string scope = "profile email";
            var data = new Dictionary<string, string>();
            data.Add("client_id", client_id);
            data.Add("client_secret", client_secret);
            data.Add("grant_type", grant_type);
            data.Add("scope", scope);
            // send request
            var req = new HttpRequestMessage(HttpMethod.Post, "http://localhost:8080/realms/education/protocol/openid-connect/token") { Content = new FormUrlEncodedContent(data) };
            var res = await httpClient.SendAsync(req);
            // read content & convert json to model
            var jsonStr = await res.Content.ReadAsStringAsync();
            var resModel = JsonConvert.DeserializeObject<KeycloakTokenResponse>(jsonStr);
            return resModel;
        }
    }
}
