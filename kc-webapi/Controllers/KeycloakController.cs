using kc_webapi.Interfaces;
using kc_webapi.Models;
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
        private IKcService _kcService;
        public KeycloakController(ILogger<KeycloakController> logger, IHttpClientFactory httpClientFactory, IKcService kc)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _kcService = kc;
        }

        [HttpGet]
        [Route("token")]
        public async Task<KeycloakTokenResponse> getToken()
        {
            return await _kcService.getToken();
        }

        [HttpGet]
        private async Task<string> callRestApi(string action)
        {
            var tokenModel = await _kcService.getToken();
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
            var result = await callRestApi(!string.IsNullOrEmpty(userid) ? "users/" + userid : "users");
            return Ok(result);
        }


        [HttpGet]
        [Route("clients")]
        public async Task<IActionResult> getClients()
        {
            var result = await callRestApi("clients");
            return Ok(result);
        }

        [HttpGet]
        [Route("clients/{clientid}/roles")]
        public async Task<IActionResult> getClientRoles(string clientid = "e48b2629-7413-4eab-98d0-6ec26040c92c")
        {
            var result = await callRestApi(string.Format("clients/{0}/roles", clientid));
            return Ok(result);
        }

        [HttpGet]
        [Route("roles")]
        public async Task<IActionResult> getRoles()
        {
            var result = await callRestApi(string.Format("roles"));
            return Ok(result);
        }

        [HttpGet]
        [Route("users/{uuid}/roles/{clientid}")]
        public async Task<IActionResult> getUserRolesClients(string uuid, string clientid = "e48b2629-7413-4eab-98d0-6ec26040c92c")
        {
            var json = await callRestApi(string.Format("users/{0}/role-mappings/clients/{client-uuid}", uuid, clientid));
            return Ok(json);
        }
        [HttpGet]
        [Route("users/{uuid}")]
        public async Task<IActionResult> getUserRoles(string uuid)
        {
            var json = await callRestApi(string.Format("users/{0}/role-mappings", uuid));
            return Ok(json);
        }
    }
}
