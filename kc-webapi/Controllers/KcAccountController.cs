using kc_webapi.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using kc_webapi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace kc_webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KcAccountController : ControllerBase
    {
        private readonly ILogger<KcAccountController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private IKcService _kcService;
        public KcAccountController (ILogger<KcAccountController> logger, IHttpClientFactory httpClientFactory, IKcService kc)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _kcService = kc;
        }
        private async Task<HttpClient> getClient ()
        {
            var tokenModel = await _kcService.getToken();
            var httpClient = _httpClientFactory.CreateClient("keycloak-rest-api");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenModel.AccessToken);
            return httpClient;
        }
        // GET: api/<KcAccountController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var client = await getClient();
            var res = await client.GetAsync("users");
            var content = await res.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<GetUserRepresentation>>(content);
            return Ok(result);
        }

        // GET api/<KcAccountController>/5
        [HttpGet("{uuid}")]
        public async Task<IActionResult> Get(string uuid)
        {
            var client = await getClient();
            var res = await client.GetAsync(string.Format("users/{0}", uuid));
            var content = await res.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<GetUserRepresentation>(content);
            return Ok(result);
        }

        // POST api/<KcAccountController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody(EmptyBodyBehavior = Microsoft.AspNetCore.Mvc.ModelBinding.EmptyBodyBehavior.Allow)] PostUserRepresentation userModel)
        {
            // step by step create-user, get user-id, get client-id, post-client_role-user, post-realm_role-user
            userModel.createdTimestamp = !userModel.createdTimestamp.HasValue ? DateTime.Now.Ticks : userModel.createdTimestamp;
            var client = await getClient();
            var res = await client.PostAsJsonAsync("users", userModel);
            var content = await res.Content.ReadAsStringAsync();
            // get-user-id
            if (res.StatusCode == System.Net.HttpStatusCode.Created)
            {
                res = await client.GetAsync(string.Format("users?username={0}", userModel.username));
                content = await res.Content.ReadAsStringAsync();
                var resultModel = JsonConvert.DeserializeObject<List<GetUserRepresentation>>(content);
                return Ok(resultModel);
            }
            else if (res.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                return Conflict(content);
            }
            return Ok(content);
        }

        // PUT api/<KcAccountController>/5
        [HttpPut("{uuid}")]
        public async Task<IActionResult> Put(string uuid, [FromBody(EmptyBodyBehavior = Microsoft.AspNetCore.Mvc.ModelBinding.EmptyBodyBehavior.Allow)] PutUserRepresentation userModel)
        {
            var client = await getClient();
            var res = await client.PutAsJsonAsync(string.Format("users/{0}", uuid), userModel);
            var content = await res.Content.ReadAsStringAsync();
            return Ok(content);
        }

        // DELETE api/<KcAccountController>/5
        [HttpDelete("{uuid}")]
        public async Task<IActionResult> Delete(string uuid)
        {
            var client = await getClient();
            var res = await client.DeleteAsync(string.Format("users/{0}", uuid));
            var content = await res.Content.ReadAsStringAsync();
            return Ok(content);
        }
    }
}
