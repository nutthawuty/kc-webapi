using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace kc_webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KeycloakController : ControllerBase
    {
        private readonly ILogger<KeycloakController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        public KeycloakController(ILogger<KeycloakController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        [Route("token")]
        public async Task<KeycloakTokenResponse> getToken()
        {
            var httpClient = _httpClientFactory.CreateClient("keycloak-rest-api");
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

        [HttpGet]
        private async Task<string> CallRestApi(string action)
        {
            var tokenModel = await getToken();
            var httpclient = _httpClientFactory.CreateClient("keycloak-rest-api");
            httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenModel.AccessToken);
            var res = await httpclient.GetAsync(action);
            var result = await res.Content.ReadAsStringAsync();
            return result;
        }

        [HttpGet]
        [Route("users")]
        public async Task<IActionResult> getUsers(string? userid)
        {
            var result = await CallRestApi(!string.IsNullOrEmpty(userid) ? "users/" + userid : "users");
            return Ok(result);
        }


        [HttpGet]
        [Route("clients")]
        public async Task<IActionResult> getClients()
        {
            var result = await CallRestApi("clients");
            return Ok(result);
        }

        [HttpGet]
        [Route("clients/{clientid}/roles")]
        public async Task<IActionResult> getClientRoles(string clientid = "e48b2629-7413-4eab-98d0-6ec26040c92c")
        {
            var result = await CallRestApi(string.Format("clients/{0}/roles", clientid));
            return Ok(result);
        }

        [HttpGet]
        [Route("roles")]
        public async Task<IActionResult> getRoles()
        {
            var result = await CallRestApi(string.Format("roles"));
            return Ok(result);
        }

        [HttpGet]
        [Route("users/{uuid}/roles/{clientid}")]
        public async Task<IActionResult> getUserRolesClients(string uuid, string clientid = "e48b2629-7413-4eab-98d0-6ec26040c92c")
        {
            var json = await CallRestApi(string.Format("users/{0}/role-mappings/clients/{client-uuid}", uuid, clientid));
            return Ok(json);
        }
        [HttpGet]
        [Route("users/{uuid}")]
        public async Task<IActionResult> getUserRoles(string uuid)
        {
            var json = await CallRestApi(string.Format("users/{0}/role-mappings", uuid));
            return Ok(json);
        }
    }
}
