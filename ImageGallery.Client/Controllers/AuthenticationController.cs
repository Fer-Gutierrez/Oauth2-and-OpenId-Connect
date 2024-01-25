using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace ImageGallery.Client.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AuthenticationController(IHttpClientFactory clientFactory)
        {
            _httpClientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
        }

        [Authorize]
        public async Task Logout()
        {
           
            //Revocar los tokens generados:
            var client = _httpClientFactory.CreateClient("IDPClient");

            //Consultamos DiscoveryDocument de nuestro IDP:
            var discoveryDocumentResponse = await client.GetDiscoveryDocumentAsync();
            if(discoveryDocumentResponse.IsError) throw new Exception(discoveryDocumentResponse.Error);

            //Revocamos el access token (solo si es de tipo reference)
            var accessTokenRevocationResponse = await client.RevokeTokenAsync(new TokenRevocationRequest()
            {
                Address = discoveryDocumentResponse.RevocationEndpoint,
                ClientId = "imagegalleryclient",
                ClientSecret = "Secret",
                Token = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken)
            });
            if (accessTokenRevocationResponse.IsError) throw new Exception(accessTokenRevocationResponse.Error);

            //Revocamos el refresh token (solo si es de tipo reference)
            var refreshTokenRevocationResponse = await client.RevokeTokenAsync(new TokenRevocationRequest()
            {
                Address = discoveryDocumentResponse.RevocationEndpoint,
                ClientId = "imagegalleryclient",
                ClientSecret = "Secret",
                Token = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken)
            });
            if (refreshTokenRevocationResponse.IsError) throw new Exception(refreshTokenRevocationResponse.Error);
        

            //Limpiamos la cookie local que guarda el token
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            //Tabien debemos cerrar sesion en el IDP:
            //"OpenIdConnectDefaults.AuthenticationScheme" oidc
            //Generará una redireccion al endpoint end_session de IDP =>  IDP borrará su propia cookie.
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);

        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
