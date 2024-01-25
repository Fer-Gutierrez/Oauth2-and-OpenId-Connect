using ImageGallery.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddJsonOptions(configure => 
        configure.JsonSerializerOptions.PropertyNamingPolicy = null);

//MIDDLEWARE PARA REGISTRAR EL ACCESS_TOKEN EN PROGRAM.CS  SIN TENER QUE CONFIGURARLO EN CADA LLAMADA.
//Usamos IdentityToken.aspnetcore para resgistrar los servicios que administran los tokens
builder.Services.AddAccessTokenManagement();
//Luego debemos agregar a cada HttpClient .AddUserAccessTokenHandler()-

// create an HttpClient used for accessing the API
builder.Services.AddHttpClient("APIClient", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ImageGalleryAPIRoot"]);
    client.DefaultRequestHeaders.Clear();
    client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
}).AddUserAccessTokenHandler(); //Esto permite que el access_token para este httpClient se transmita en cada request y ademas actualiza el access_token (con el refresh_token) automaticamente cuando se vence.

//Creamos un nuevo cliente para revocar los tokens generados
builder.Services.AddHttpClient("IDPClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:5001");
});


//Este middleware de Authenticacion debe contar con los mismos parametros configurados en el IDP para este CLIENT.
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.AccessDeniedPath = "/Authentication/AccessDenied"; //Configuramos cual será la vista de acceso denegado.
    })
    .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
    {
        options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.Authority = "https://localhost:5001";
        options.ClientId = "imagegalleryclient";
        options.ClientSecret = "Secret";
        options.ResponseType = "code";
        //options.Scope.Add("openid"); --> Los scopes estandares son captados predeterminadamente por el middleware -->linea 52: https://github.com/dotnet/aspnetcore/blob/main/src/Security/Authentication/OpenIdConnect/src/OpenIdConnectOptions.cs.
        //options.Scope.Add("profile"); --> scope estandar
        //options.CallbackPath = new PathString("signin-oidc"); --> El middleware configura signin-oidc de forma predeterminada (se debe agregar si la URL del callback cambia)
        //options.SignedOutCallbackPath = new PathString("signout-callback-oidc") --> El middleware tambien lo detecto por defecto(se debe agregar si la URL loguot del callback cambia)
        options.SaveTokens = true; //--> El middleware guarda el token recibido para poder usarlo luego.
        options.GetClaimsFromUserInfoEndpoint = true; //solicita los claims del usuario al endopint UserInfo del IDP --> cuando recibe la respuesta los guarda como claimsIdentity en la cookie.

        //ELIMINAR UN CLAIM ACTION:
        //por defecto el middleware filtra (elimina) el claim type aud que viene en el id_token.
        // ver a partir de la linea 52: https://github.com/dotnet/aspnetcore/blob/main/src/Security/Authentication/OpenIdConnect/src/OpenIdConnectOptions.cs.
        //Si necesito dicho claim type en mi app, podemos eliminar ese filtro de la siguiente manera:
        options.ClaimActions.Remove("aud");

        //ELIMINAR UN CLAIM QUE ME LLEGA EN EL ID_TOKEN:
        //por ejemplo los claims sid (session del DIP) y idp no son utilizados en las apps
        options.ClaimActions.DeleteClaim("sid");
        options.ClaimActions.DeleteClaim("idp");

        //SOLICITAR UN SCOPE ADICIONAL:
        //Previamente debemos tener configurado el scope configurado en el IDP y colocarlos AllowedScopes en el clientId.
        options.Scope.Add("roles");
        options.ClaimActions.MapJsonKey("role", "role"); //Si el claim no lo encontramos en los User.Claims debemos agregar la funcion de mapeo- llega en la respuesta del endpoin userInfo, pero middleware no lo mapea --> ver a partir de la linea 52: https://github.com/dotnet/aspnetcore/blob/main/src/Security/Authentication/OpenIdConnect/src/OpenIdConnectOptions.cs.

        //CONFIGURAMOS EL TOKEN VALIDATION PARAMETERS OPTION:
        //Para utilizar la funcionalidad de los claims en las pages y decoradores de authorizacion de pages.
        options.TokenValidationParameters = new()
        {
            RoleClaimType = "role",
            NameClaimType = "given_name"
        };
        //options.Scope.Add("imagegalleryapi.fullaccess");
        options.Scope.Add("imagegalleryapi.read");
        options.Scope.Add("imagegalleryapi.write");
        options.Scope.Add("country");
        options.ClaimActions.MapUniqueJsonKey("country", "country"); // Agregamos el claim a nuestro ClaimsIdentity (mapeo unico, por es posible residir en mas de un pais.)
        options.Scope.Add("offline_access"); //Para activar refreshToken

    });

//Eliminar el diccionario de mapeo de Claims:
//El middleware de authentication recibe el id_token y sus claims types dentro, pero el diccionario de mapeo que tiene incorporado desde la base jwt modifica los claim types.
//debemos desactivar dicho diccionario de mapeo para que los claim types lleguen como los envia el IDP:
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();


//POLITICAS:
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("UserCanAddImage", AuthorizationPolicies.CanAddImage());
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting(); //--> middleware que se encarga de mapear las rutas

app.UseAuthentication(); //siempre se agrega luego de UseRouting y UseAuthorization

app.UseAuthorization(); // --> middleware que se encarga en authorizar la entrada a cada endpoint.

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Gallery}/{action=Index}/{id?}");

app.Run();
