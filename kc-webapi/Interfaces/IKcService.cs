using kc_webapi.Models;

namespace kc_webapi.Interfaces
{
    public interface IKcService
    {
        Task<KeycloakTokenResponse> getToken();
    }
}
