using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace Marvin.IDP;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResource("roles","Yours role(s)", new [] {"role"}),
            new IdentityResource("country","The cpuntry you are living in", new []{"country"})
        };

    public static IEnumerable<ApiResource> ApiResourses => 
        new ApiResource[]
        {
            new ApiResource("imagegalleryapi","Image Gallery API")
            {
                Scopes = {"imagegalleryapi.fullaccess", "imagegalleryapi.read", "imagegalleryapi.write"},
                UserClaims = {"role", "country"},
                ApiSecrets = {new Secret("apiSecret".Sha256())} //Necesario para utilizar Reference Tokens
            }
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
            {
                new ApiScope("imagegalleryapi.fullaccess"),
                new ApiScope("imagegalleryapi.read"),
                new ApiScope("imagegalleryapi.write")
            };

    public static IEnumerable<Client> Clients =>
        new Client[] 
            { 
                new Client
                {
                    ClientName = "Image Gallery",
                    ClientId = "imagegalleryclient",
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris =
                    {
                        "https://localhost:7184/signin-oidc"
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "roles",
                        //"imagegalleryapi.fullaccess",
                        "imagegalleryapi.read",
                        "imagegalleryapi.write",
                        "country"
                    },
                    ClientSecrets =
                    {
                        new Secret("Secret".Sha256())
                    },
                    //RequireConsent = true, // false
                    PostLogoutRedirectUris =
                    {
                        "https://localhost:7184/signout-callback-oidc"
                    },
                    //AlwaysIncludeUserClaimsInIdToken = true, // Esto no es recomendable:
                    //1. Por el largo del URI (id_token) --> en algunos navegadores puede arrojar error de longitud.
                    //2. Por el riesgo de que un atacante obtenga nuestro id_token y se haga con los User Claims
                    
                    //TOKEN'S LIFETIME
                    //IdentityTokenLifetime = por defecto viene 5 minutos
                    //AuthorizationCodeLifetime = por defecto viene 5 minutos
                    AccessTokenLifetime  = 120, //por defecto viene 1 hora

                    //REFRESH TOKEN
                    AllowOfflineAccess = true, //true para activar refresh_token
                    //AbsoluteRefreshTokenLifetime = vida absoluta de un refreshToken --> 30 dias por defecto
                    //SlidingRefreshTokenLifetime  //Es la vida util de cada refreshToken que se actualiza. 15 por defecto.
                    UpdateAccessTokenClaimsOnRefresh = true, //actualizar los claims en el access token cuando pide refresh.

                    //REFERENCE TOKEN:
                    //Es un tipo de token que se utiliza para manejar de mejor manera la vida util de lo tokens que se envian al client y API.
                    //Es un token que permace en IDP y se utiliza para validar el token que recibe una API en cada solicitud.
                    //reference tokens son validado en el instrospection endpoint
                    /*AccessTokenType = AccessTokenType.Reference //Por defecto viene JWT.*/


                    
                }
            };
}