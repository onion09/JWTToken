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
            string userId = jwtUtils.ValidateToken(token);
            if (userId != null)
            {
                context.Items["UserId"] = userId;


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
