using Microsoft.AspNetCore.Authorization;

namespace ImageGallery.Authorization
{
    public static class AuthorizationPolicies
    {
        public static AuthorizationPolicy CanAddImage()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireClaim("country", "belgium")
                .RequireClaim("country", "beligum",  "nederland") //Cuando necesitamos validar mas de un valor
                .RequireRole("PayingUser")
                .Build();
        }
    }
}
