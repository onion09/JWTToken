using JWTToken.Util;
using System.Net;

namespace JWTToken.MiddleWare
{
    public class JwtMiddleware
    {
        //going to next middleware
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IJwtUtils jwtUtils)
        {
            //get token first
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            //validate token
            var permissions  = jwtUtils.ValidatePermissions(token);
            if (permissions != null)
            {
                for(var i = 0; i < permissions.Count-1; i++)
                {
                    var permission = permissions[i];
                    context.Items[$"{permission}"] = permissions[i];
                }
                context.Items["UserId"] = permissions.Last();
            }
            await _next(context);
            /*
            else {
                await ReturnErrorResponse(context);
            }
            */
        }
        //Console.WriteLine("Userid {0}", userId);



        private async Task ReturnErrorResponse(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await context.Response.WriteAsync("error message!");
        }
    }
}
