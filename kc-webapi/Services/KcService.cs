using kc_webapi.Controllers;
using kc_webapi.Interfaces;
using kc_webapi.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;

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
        private string baseUri = "http://localhost:8080/realms/education/protocol/openid-connect";
        public async Task<KeycloakTokenResponse> getToken()
        {
            var httpClient = _httpClientFactory.CreateClient();
            // add payload-data
            string grant_type = "client_credentials";
            string client_id = "logic-school";
            string client_secret = "7bu7YXvvdmWfl6LlzTy52f1vQJQGfUFx";
            string scope = "profile email";
            var data = new Dictionary<string, string>();
            data.Add("client_id", client_id);
            data.Add("client_secret", client_secret);
            data.Add("grant_type", grant_type);
            data.Add("scope", scope);
            // send request
            var req = new HttpRequestMessage(HttpMethod.Post, string.Format("{0}/token", baseUri)) { Content = new FormUrlEncodedContent(data) };
            var res = await httpClient.SendAsync(req);
            // read content & convert json to model
            var jsonStr = await res.Content.ReadAsStringAsync();
            var resModel = JsonConvert.DeserializeObject<KeycloakTokenResponse>(jsonStr);
            return resModel;
        }

        public async Task<KeycloakResponse<KeycloakTokenResponse>> getToken(KeyCloakTokenRequest tokenBody)
        {
            var httpClient = _httpClientFactory.CreateClient();
            string action = "token";
            // add payload-data
            var data = new Dictionary<string, string>();
            if (tokenBody.grant_type == GrantType.Password)
            {
                data.Add("client_id", tokenBody.client_id);
                data.Add("username", tokenBody.username);
                data.Add("password", tokenBody.password);
            }
            else if (tokenBody.grant_type == GrantType.RefreshToken || tokenBody.grant_type == GrantType.Logout)
            {
                data.Add("client_id", tokenBody.client_id);
                data.Add("refresh_token", tokenBody.refresh_token);
            }
            else if (tokenBody.grant_type == GrantType.ClientCredentials)
            {
                data.Add("client_id", "logic-school");
                data.Add("client_secret", "7bu7YXvvdmWfl6LlzTy52f1vQJQGfUFx");
            }
            if (tokenBody.grant_type != GrantType.Logout)
            {
                data.Add("grant_type", tokenBody.grant_type);
            }
            else
            {
                action = "logout";
            }
            // send request
            var req = new HttpRequestMessage(HttpMethod.Post, string.Format("{0}/{1}", baseUri, action)) { Content = new FormUrlEncodedContent(data) };
            var res = await httpClient.SendAsync(req);
            var content = await res.Content.ReadAsStringAsync();
            var result = new KeycloakResponse<KeycloakTokenResponse>();
            if (res.StatusCode == System.Net.HttpStatusCode.OK)
            {
                // read content & convert json to model
                var jsonStr = await res.Content.ReadAsStringAsync();
                var resModel = JsonConvert.DeserializeObject<KeycloakTokenResponse>(content);
                result.data = resModel;

            }
            else if (res.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                result.message = "logout-success";
            }
            else
            {
                result.message = await res.Content.ReadAsStringAsync();
            }
            return result;
        }

        public async Task<HttpClient> getClient(KeycloakTokenResponse tokenModel)
        {
            var httpClient = _httpClientFactory.CreateClient("keycloak-rest-api");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenModel.AccessToken);
            return httpClient;
        }

        public async Task<HttpClient> getClientUser(KeycloakTokenResponse tokenModel)
        {
            var httpClient = _httpClientFactory.CreateClient("keycloak-user");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenModel.AccessToken);
            return httpClient;
        }
    }
}
