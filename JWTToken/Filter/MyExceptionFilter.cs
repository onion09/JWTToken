using JWTToken.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace JWTToken.Filter
{
    
    public class MyExceptionFilter : IExceptionFilter
    {
        //param: ExceptionContext provides information about the exception that occurred during the processing of a request
        public void OnException(ExceptionContext context)
        {
            var exception = new ExceptionResponse()
            {
                StatusCode= 400,
                Message= context.Exception.Message + " From Filter",
            };

            context.Result = new JsonResult(exception);

        }
    }
}
