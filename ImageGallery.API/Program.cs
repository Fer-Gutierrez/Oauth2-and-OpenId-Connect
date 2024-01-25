using ImageGallery.API.Authorization;
using ImageGallery.API.DbContexts;
using ImageGallery.API.Services;
using ImageGallery.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(configure => configure.JsonSerializerOptions.PropertyNamingPolicy = null);

builder.Services.AddDbContext<GalleryContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration["ConnectionStrings:ImageGallerySQL"]);
});

// register the repository
builder.Services.AddScoped<IGalleryRepository, GalleryRepository>();
builder.Services.AddHttpContextAccessor(); //--> MustOwnImageHandler depende de AddHttpContextAccessor, por ello lo agregamos antes de inyectar el servicio (siguiente linea)
builder.Services.AddScoped<IAuthorizationHandler, MustOwnImageHandler>();

// register AutoMapper-related services
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
#region MIDDLEWARE SI UTILIZAMOS JWT TOKENS
    /*Middleware utilizado para JWT Tokens*/
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Authority"];
        options.Audience = "imagegalleryapi";

        //CONFIGURAMOS EL TOKEN VALIDATION PARAMETERS OPTION:
        //Esto tambien debe estar configurado desde el lado del cliente
        //Para utilizar la funcionalidad de los claims (role y name) en los decoradores de authorizacion de los endpoints.
        options.TokenValidationParameters = new()
        {
            RoleClaimType = "role",
            NameClaimType = "given_name",
            ValidTypes = new[] { "at+jwt" }//verificar el type header token - para evitar ataques de confusion de JWT
        };
    });
#endregion
#region MIDDLEWARE SI UTILIZAMOS REFERENCE TOKENS
/*Middlewate utilizado para Reference Tokens: IdentityModel.aspnetcore.Oauth2Instrospection*/
//.AddOAuth2Introspection(options =>
//{
//    options.Authority = builder.Configuration["Authority"];
//    options.ClientId =  "imagegalleryapi";
//    options.ClientSecret = "apiSecret"; //Secret detallado en ApiResourse
//    options.RoleClaimType = "role";
//    options.NameClaimType = "given_name";
//});
#endregion

//Eliminar el diccionario de mapeo de Claims:
//Debemos asegurarnos de que los calims que llegan a la API se ejecuten de la misma manera que lo hicimos en nuestro cliente
//Por lo tanto si en nuestro Cliente tenemos el siguiente codigo, estamos utilizando el mismo mapeo de claims:
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();


//POLITICAS:
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("UserCanAddImage", AuthorizationPolicies.CanAddImage());
    
    options.AddPolicy("ClientAppCanWrite", policy => policy.RequireClaim("scope", "imagegalleryapi.write"));

    //REGLA DE AUTHORIZACION COMPLEJA --> AuthorizationHandler
    options.AddPolicy("MustOwnImage", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.AddRequirements(new MustOwnImageRequirement()); //Requerimiento complejo que requiere un handler
    });

});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthentication(); //En la pipeline de la API UseAuthentication se coloca antes de Authorization y MapController-

app.UseAuthorization();

app.MapControllers();

app.Run();
