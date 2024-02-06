using Marvin.IDP.DbContexts;
using Marvin.IDP.Entities;
using Marvin.IDP.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Marvin.IDP;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        // uncomment if you want to add a UI
        builder.Services.AddRazorPages();

        builder.Services.AddIdentityServer(options =>
            {
                // https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/api_scopes#authorization-based-on-scopes
                options.EmitStaticAudienceClaim = true;
            })
            .AddProfileService<LocalUserProfileService>() //Agregamos este servicio para devolver los User Claims solicitados
            .AddInMemoryIdentityResources(Config.IdentityResources)
            .AddInMemoryApiResources(Config.ApiResourses)
            .AddInMemoryApiScopes(Config.ApiScopes)
            .AddInMemoryClients(Config.Clients);
            //.AddTestUsers(TestUsers.Users);

        builder.Services.AddDbContext<IdentityDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("MarvinIDPConn"));
        });

        builder.Services.AddScoped<ILocalUserService, LocalUserService>();
        builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

        return builder.Build();
    }
    
    public static WebApplication ConfigurePipeline(this WebApplication app)
    { 
        app.UseSerilogRequestLogging();
    
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        // uncomment if you want to add a UI
        app.UseStaticFiles();
        app.UseRouting();
            
        app.UseIdentityServer();

        // uncomment if you want to add a UI
        app.UseAuthorization();
        app.MapRazorPages().RequireAuthorization();

        return app;
    }
}
