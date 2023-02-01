using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace JWTToken.Util
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]

    public class UpdateAuthoriaztion : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var update = context.HttpContext.Items["active"];
           
            if (update == null)
            {
                // not logged in
                context.Result = new JsonResult(new { message = "Unauthorized for active" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }

        }
    }
}
