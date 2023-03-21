using kc_webapi.Models;

namespace kc_webapi.Interfaces
{
    public interface IKcService
    {
        Task<HttpClient> getClient(KeycloakTokenResponse tokenModel);
        Task<HttpClient> getClientUser(KeycloakTokenResponse tokenModel);
        Task<KeycloakResponse<KeycloakTokenResponse>> getToken(KeyCloakTokenRequest tokenPayload);
        Task<KeycloakTokenResponse> getToken();
    }
}
