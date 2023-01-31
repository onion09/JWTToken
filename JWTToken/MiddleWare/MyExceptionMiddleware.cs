using JWTToken.Exceptions;
using JWTToken.Models;
using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;
using System.Net;

namespace JWTToken.Middleware
{
    public class MyExceptionMiddleware
    {
        //represents the handling function for an HTTP request. 
        private readonly RequestDelegate _requestDelegate;

        private readonly ILogger<MyExceptionMiddleware> _logger;

        public MyExceptionMiddleware(RequestDelegate requestDelegate, ILogger<MyExceptionMiddleware> logger)
        {
            _requestDelegate = requestDelegate;
            _logger = logger;
        }

        //process request and generate response
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _requestDelegate(context); // invoke the delegate and allow it to handle request and generate response 
            }
            catch (Exception ex)
            {
                await HandleException(context, ex);

                var response = context.Response;
                response.ContentType= "application/json"; //accept json request
                int statusCode;
                switch(ex)
                {
                    case UserNotFoundException e:
                        statusCode = (int)HttpStatusCode.BadRequest; 
                        break;
                    case InvalidInputException e:
                        statusCode = (int)HttpStatusCode.NotAcceptable;
                        break;
                    default:
                        statusCode = (int)HttpStatusCode.InternalServerError; 
                        break;   
                }

                var exceptionResponse = new ExceptionResponse()
                {
                    StatusCode = statusCode,
                    Message = ex.Message + "From Exception Middleware",
                };

                //generate the response
                await response.WriteAsync(exceptionResponse.ToString());

            }
        }
        private Task HandleException(HttpContext context, Exception ex)
        {
            _logger.LogError(ex.ToString());
            var errorMessageObject = new { Message = ex.Message, Code = "system_error" };

            var errorMessage = JsonConvert.SerializeObject(errorMessageObject);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return context.Response.WriteAsync(errorMessage);
        }
    }
}
