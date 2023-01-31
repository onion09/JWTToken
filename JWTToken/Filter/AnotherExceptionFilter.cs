using JWTToken.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace JWTToken.Filter
{
    public class AnotherExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var exception = new ExceptionResponse()
            {
                StatusCode = 400,
                Message = context.Exception.Message + " From Filter Attribute",
            };

            context.Result = new JsonResult(exception);

        }
    }
}
