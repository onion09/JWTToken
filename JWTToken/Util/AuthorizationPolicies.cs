using Microsoft.AspNetCore.Authorization;

namespace JWTToken.Util
{
    public static  class AuthorizationPolicies
    {
        public static AuthorizationPolicy RequireActivetClaim()
        {
            return new AuthorizationPolicyBuilder()
                .RequireClaim("active", "active")
                .Build();
        }
    }
}
