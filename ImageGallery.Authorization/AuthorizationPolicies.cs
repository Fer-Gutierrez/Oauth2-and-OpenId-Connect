using Microsoft.AspNetCore.Authorization;

namespace ImageGallery.Authorization
{
    public static class AuthorizationPolicies
    {
        public static AuthorizationPolicy CanAddImage()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireClaim("country", "be")
                .RequireClaim("country", "be",  "in") //Cuando necesitamos validar mas de un valor
                .RequireRole("PayingUser")
                .Build();
        }
    }
}
