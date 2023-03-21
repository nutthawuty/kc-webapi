using kc_webapi.Interfaces;
using kc_webapi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Web;

namespace kc_webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly ILogger<UserController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private IKcService _kcService;
        public UserController(ILogger<UserController> logger, IHttpClientFactory httpClientFactory, IKcService kcService)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _kcService = kcService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> userLogin([FromBody] KeyCloakTokenRequest tokenBody)
        {
            var token = await _kcService.getToken(tokenBody);
            return Ok(token);
        }

        [HttpPost]
        [Route("login/refresh")]
        public async Task<IActionResult> userRefresh([FromBody] KeyCloakTokenRequest tokenBody)
        {
            var token = await _kcService.getToken(tokenBody);
            return Ok(token);
        }

        [HttpGet]
        [Route("userinfo")]
        public async Task<IActionResult> userInfo()
        {
            string accesstoken = Request.Headers.Authorization;
            var client = await _kcService.getClientUser(new KeycloakTokenResponse() { AccessToken = accesstoken.Split("Bearer")[1] });
            var res = await client.GetAsync("userinfo");
            var content = await res.Content.ReadAsStringAsync();
            return Ok(content);
        }
        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> userLogout(KeyCloakTokenRequest tokenBody)
        {
            var token = await _kcService.getToken(tokenBody);
            return Ok(token);
        }
    }
}
